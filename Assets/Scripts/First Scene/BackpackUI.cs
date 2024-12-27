using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackUI : MonoBehaviour
{

    public Transform itemsGrid; // Сетка UI для отображения иконок
    public List<GameObject> itemPrefabs; // Коллекция префабов иконок
    public Color selectedColor = Color.yellow; // Цвет для выделения выбранной иконки
    public Color defaultColor = Color.white; // Цвет для невыбранных иконок

    private Dictionary<GameObject, GameObject> backpackItems = new Dictionary<GameObject, GameObject>(); // Словарь для хранения предметов и их иконок
    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>(); // Словарь для хранения оригинальных масштабов
    private List<GameObject> itemIconsList = new List<GameObject>(); // Список иконок предметов для навигации
    private int selectedIndex = 0; // Индекс текущего выбранного предмета
    [SerializeField] private GameObject PersonPosition;
    [SerializeField] private GameObject PanelUI; // Объект BackPackPanel - окно инвентаря
    [SerializeField] private CameraManager manager; // Флаг для проверки состояния камеры
    public float highlightScale = 1.5f;
    private bool isHighlighted = false; // Флаг для отслеживания состояния выделения

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && manager.GetMeaningTumbler() == false && manager.GetStatusGame()) // Проверка флага изменения положения камеры и проверка статуса игры
        {
            if (PanelUI.activeSelf == true) { // Проверка открыт или закрыт инвентарь
                PanelUI.SetActive(false); // Перемещение вверх
            }
            else PanelUI.SetActive(true);

        }

        if (Input.GetKeyDown(KeyCode.UpArrow)) // В этом блоке осуществляется перебор предметов в инвентаре
        {
            MoveSelection(-1); // Перемещение вверх
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelection(1); // Перемещение вниз
        }
        else if (Input.GetKeyDown(KeyCode.Q) && PanelUI.activeSelf == true) // "Q" для удаления предмета
        {
            if (itemIconsList.Count > 0 && selectedIndex >= 0 && selectedIndex < itemIconsList.Count) // проверка, что selectedIndex находится в допустимых
                                                                                                      // границах списка itemIconsList.
            {
                GameObject item = FindOriginalItemByIcon(itemIconsList[selectedIndex]); // Поиск оригинального предмета по выбранной иконке:
                if (item != null)
                {
                    RemoveItemFromBackpack(item); // Удаление предмета из рюкзака:

                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.V) && PanelUI.activeSelf == true) // "V" для увеличения выделенной иконки
        {
            ToggleHighlight();
        }
    }

    // Функция для добавления предмета в рюкзак
    public void AddItemToBackpack(GameObject item)
    {
        GameObject itemIconPrefab = itemPrefabs.Find(prefab => prefab.name == item.name); // Если подходящий префаб иконки найден, он сохраняется в переменной itemIconPrefab

        if (itemIconPrefab != null) // Проверка наличия префаба
        {
            GameObject newItemIcon = Instantiate(itemIconPrefab, itemsGrid); // Здесь мы создаём новый экземпляр префаба itemIconPrefab и добавляем его в itemsGrid.

            backpackItems[item] = newItemIcon; // Сохраняем связь между предметом и иконкой
            // это словарь, который связывает оригинальный объект item и его иконку newItemIcon

            itemIconsList.Add(newItemIcon); // Добавляем иконку в список для навигации

            // Сохраняем исходный масштаб
            originalScales[newItemIcon] = newItemIcon.transform.localScale; // originalScales — это словарь, который хранит исходный масштаб каждой иконки.

            item.SetActive(false); // Скрываем предмет в сцене
            UpdateSelectionVisual(); // Обновляем выделение
        }
    }

    // Функция для удаления предмета из рюкзака
    public void RemoveItemFromBackpack(GameObject item) // Эта функция эффективно управляет удалением предметов, организует отображение и сохраняет структурированность рюкзака.
    {
        if (backpackItems.ContainsKey(item)) // проверяем, действительно ли указанный предмет присутствует в рюкзаке.
        {
            item.SetActive(true); // Включаем предмет
            item.GetComponent<Rigidbody>().useGravity = true; // Включаем действие гравитации на предмет
            item.transform.position = PersonPosition.transform.position + PersonPosition.transform.forward * 2; // Выкладывание предмета перед персонажем

            GameObject itemIcon = backpackItems[item]; // Получение иконки предмета из словаря: Мы получаем её из словаря backpackItems, чтобы затем удалить из UI.
            itemIconsList.Remove(itemIcon); //  Удаляем иконку из списка itemIconsList, который используется для навигации в рюкзаке.
            originalScales.Remove(itemIcon); // Удаляем иконку из словаря originalScales, где хранится её оригинальный масштаб.

            Destroy(itemIcon); // Уничтожаем объект иконки, удаляя её из интерфейса рюкзака.
            backpackItems.Remove(item); //  удаляет оригинальный предмет из словаря backpackItems, так как иконка больше не нужна.

            selectedIndex = Mathf.Clamp(selectedIndex, 0, itemIconsList.Count - 1); // Обновляем индекс. selectedIndex корректируется, чтобы оставаться в пределах доступных иконок.
            UpdateSelectionVisual(); // Обновляем выделение
        }
    }

    // Метод для перемещения выделения
    private void MoveSelection(int direction) // Основная цель этой функции — изменить выделение текущей иконки в рюкзаке на одну позицию вверх
                                              // или вниз в зависимости от переданного параметра direction.
    {
        if (itemIconsList.Count == 0) return; // Если список пуст, выходим из функции

        selectedIndex = (selectedIndex + direction + itemIconsList.Count) % itemIconsList.Count; // циклическое перемещение индекса по элементам списка itemIconsList,
        // когда selectedIndex изменяется вверх или вниз

        isHighlighted = false; //  Сбрасываем флаг увеличения при смене выделения
        UpdateSelectionVisual(); // Обновляем визуальное выделение
    }

    // Метод для визуального обновления выделения
    private void UpdateSelectionVisual() // Меняет цвет выбранной иконки на selectedColor и возвращает цвет остальных к defaultColor.
    { // Увеличивает масштаб выделенной иконки, если она находится в состоянии isHighlighted.

        for (int i = 0; i < itemIconsList.Count; i++) // Цикл проходит по всему списку itemIconsList, в котором хранятся все иконки предметов в рюкзаке.
        {
            // Получение текущей иконки и её компонента Image:
            GameObject iconObject = itemIconsList[i]; // iconObject — текущая иконка (объект UI) в списке itemIconsList.
            Image iconImage = iconObject.GetComponent<Image>();  // компонент Image иконки, который управляет её визуальными свойствами (например, цветом).
            iconImage.color = (i == selectedIndex) ? selectedColor : defaultColor;  // Изменение цвета выделенной иконки

            // Устанавливаем масштаб для выделенной иконки
            Vector3 baseScale = originalScales[iconObject]; // baseScale берётся из словаря originalScales, в котором сохранён исходный масштаб каждой иконки при добавлении.
            iconObject.transform.localScale = (i == selectedIndex && isHighlighted) ? baseScale * highlightScale : baseScale; // Если текущая иконка — это выбранная (i == selectedIndex)
             // и она также находится в состоянии выделения (isHighlighted), её масштаб увеличивается на значение highlightScale.

        }
    }

    // Вспомогательный метод для поиска оригинального предмета по иконке
    private GameObject FindOriginalItemByIcon(GameObject icon) // Позволяет найти оригинальный предмет в рюкзаке по иконке, пройдя через словарь backpackItems.
    {                                          // icon — это GameObject, представляющий иконку предмета

        foreach (var pair in backpackItems) // Цикл foreach проходит по всем парам (ключ и значение) в словаре.
        {
            if (pair.Value == icon) // Для каждой пары pair в backpackItems  проверяем, совпадает ли
                                    // значение pair.Value (иконка предмета) с переданным в функцию параметром icon
            {
                return pair.Key; // Возвращаем оригинальный предмет, соответствующий иконке
            }
        }
        return null; // отсутствие соответствующего оригинального предмета.
    }

    private void ToggleHighlight() // Включение увеличения масштаба выделенной иконки
    {
        isHighlighted = !isHighlighted; // Переключаем состояние выделения
        UpdateSelectionVisual(); // Обновляем визуальное отображение
    }
}
