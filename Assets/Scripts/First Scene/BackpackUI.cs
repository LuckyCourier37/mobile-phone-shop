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
    [SerializeField] private GameObject PanelUI;
    [SerializeField] private CameraManager manager;
    public float highlightScale = 1.5f;
    private bool isHighlighted = false; // Флаг для отслеживания состояния выделения

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && manager.GetMeaningTumbler() == false && manager.GetStatusGame())
        {
            if (PanelUI.activeSelf == true) {
                PanelUI.SetActive(false); // Перемещение вверх
            }
            else PanelUI.SetActive(true);

        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveSelection(-1); // Перемещение вверх
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelection(1); // Перемещение вниз
        }
        else if (Input.GetKeyDown(KeyCode.Q) && PanelUI.activeSelf == true) // "Q" для удаления предмета
        {
            if (itemIconsList.Count > 0 && selectedIndex >= 0 && selectedIndex < itemIconsList.Count)
            {
                GameObject item = FindOriginalItemByIcon(itemIconsList[selectedIndex]);
                if (item != null)
                {
                    RemoveItemFromBackpack(item);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.V) && PanelUI.activeSelf == true) // "Space" для увеличения выделенной иконки
        {
            ToggleHighlight();
        }
    }

    // Функция для добавления предмета в рюкзак
    public void AddItemToBackpack(GameObject item)
    {
        GameObject itemIconPrefab = itemPrefabs.Find(prefab => prefab.name == item.name);

        if (itemIconPrefab != null)
        {
            GameObject newItemIcon = Instantiate(itemIconPrefab, itemsGrid);
            backpackItems[item] = newItemIcon; // Сохраняем связь между предметом и иконкой
            itemIconsList.Add(newItemIcon); // Добавляем иконку в список для навигации

            // Сохраняем исходный масштаб
            originalScales[newItemIcon] = newItemIcon.transform.localScale;

            item.SetActive(false); // Скрываем предмет в сцене
            UpdateSelectionVisual(); // Обновляем выделение
        }
    }

    // Функция для удаления предмета из рюкзака
    public void RemoveItemFromBackpack(GameObject item)
    {
        if (backpackItems.ContainsKey(item))
        {
            item.SetActive(true);
            item.GetComponent<Rigidbody>().useGravity = true;
            item.transform.position = PersonPosition.transform.position + PersonPosition.transform.forward * 2;

            GameObject itemIcon = backpackItems[item];
            itemIconsList.Remove(itemIcon); // Убираем иконку из списка навигации
            originalScales.Remove(itemIcon); // Удаляем из словаря масштабов

            Destroy(itemIcon);
            backpackItems.Remove(item);
            selectedIndex = Mathf.Clamp(selectedIndex, 0, itemIconsList.Count - 1); // Обновляем индекс
            UpdateSelectionVisual(); // Обновляем выделение
        }
    }

    // Метод для перемещения выделения
    private void MoveSelection(int direction)
    {
        if (itemIconsList.Count == 0) return;
        selectedIndex = (selectedIndex + direction + itemIconsList.Count) % itemIconsList.Count; // Циклический переход
        isHighlighted = false;
        UpdateSelectionVisual();
    }

    // Метод для визуального обновления выделения
    private void UpdateSelectionVisual()
    {
        for (int i = 0; i < itemIconsList.Count; i++)
        {
            GameObject iconObject = itemIconsList[i];
            Image iconImage = iconObject.GetComponent<Image>();
            iconImage.color = (i == selectedIndex) ? selectedColor : defaultColor;

            // Устанавливаем масштаб для выделенной иконки
            Vector3 baseScale = originalScales[iconObject];
            iconObject.transform.localScale = (i == selectedIndex && isHighlighted) ? baseScale * highlightScale : baseScale;
        }
    }

    // Вспомогательный метод для поиска оригинального предмета по иконке
    private GameObject FindOriginalItemByIcon(GameObject icon)
    {
        foreach (var pair in backpackItems)
        {
            if (pair.Value == icon)
            {
                return pair.Key; // Возвращаем оригинальный предмет, соответствующий иконке
            }
        }
        return null;
    }

    private void ToggleHighlight()
    {
        isHighlighted = !isHighlighted; // Переключаем состояние выделения
        UpdateSelectionVisual(); // Обновляем визуальное отображение
    }
}
