using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    // Start is called before the first frame update
    private Button button; // Ссылка внутри скрипта на компонент Button
    [SerializeField] private GameObject PanelControls; // пустой объект(держатель) Panel, внутри содержится окно со всеми кнопками управления.
    private Vector3 startPos; // Здесь хранится стартовое положение главной статичной камеры.
    
    [SerializeField] private GameObject Maincamera; // Ссылка на главную камеру - Maincamera
    [SerializeField] private GameObject startButton; // Кнопка Start Button
    [SerializeField] private GameObject title; // Ссылка на текстовое поле Title - здесь содержится заголовок, название сцены.
    [SerializeField] private GameObject buttonExit; // Кнопка Exit to Window
    [SerializeField] private GameObject titleControls; //  объект Controls - содержит в себе плашку с кнопкой вызова меню настроек

    void Start()
    {

        button = GetComponent<Button>();
        button.onClick.AddListener(ShowOptions);  // Инициализация, присвоение функции кнопке

        startPos = Maincamera.transform.position; // Сохранение первоначальной позиции главной камеры
         
    }

    // Update is called once per frame
    
    public void ShowOptions() // Главная функция кнопки Options
    {
        Debug.Log(button.name + " Was clicked");
        
        PanelControls.SetActive(true); // Активация окна с кнопками управления
        
        try
        {
            // Пытаемся обратиться к SomeButton
            startButton.SetActive(false); // Пример действия с объектом
        }
        catch (MissingReferenceException e)
        {
            // Если объект был уничтожен, ловим исключение и выводим сообщение
            Debug.Log("Объект SomeButton не существует: " + e.Message);
        }

        title.SetActive(false); // Отключение заголовка сцены
        buttonExit.SetActive(false); // Отключение кнопки  Exit to Window
        gameObject.SetActive(false); // Отключение кнопки Options
        Maincamera.transform.position += new Vector3(0f, 0f, 100f); // Перемещаем главную камеру на 100 единиц вдоль оси Z
    }

    public void RevertOptions() // Данная функция прикреплена к кнопке Revert
    {

        PanelControls.SetActive(false); // Отключение окна с кнопками управления

        try
        {
            // Пытаемся обратиться к SomeButton
            startButton.SetActive(true); // Пример действия с объектом
        }
        catch (MissingReferenceException e)
        {
            // Если объект был уничтожен, ловим исключение и выводим сообщение
            Debug.Log("Объект SomeButton не существует: " + e.Message);
        }
        title.SetActive(true); // Включение заголовка сцены
        gameObject.SetActive(true);  // Включение кнопки Options
        buttonExit.SetActive(true); // Включение кнопки  Exit to Window
        Maincamera.transform.position = startPos;

    }

    public void CloseOptions() // Эта функция вызывается из скрипта CameraManager, доступна после запуска сцены.
    {                         // Сворачивает окно со всеми кнопками управления(эта часть здесь) и (возвращает камеру в действующую сцену) - а этот фрагмент в скрипте CameraManager

        Maincamera.transform.position = startPos; // Возврат положения главной(статической) камеры в исходное положение
        PanelControls.SetActive(false); // Отключение окна с кнопками управления
        titleControls.SetActive(true); // включение плашкт с кнопкой вызова меню настроек

    }
    public void ShowOptionsCopy(Vector3 mainPos) // Эта функция вызывается из скрипта CameraManager, доступна после запуска сцены.
    {                                            // Открывает окно со всеми кнопками управления  и меняет положение статической камеры.

        PanelControls.SetActive(true); // Активация окна с кнопками управления
        Maincamera.transform.position = mainPos + new Vector3(0f, 15f, 0f); // Изменение положение главной(статической) камеры
        titleControls.SetActive(false); // отключение плашки с кнопкой вызова меню настроек

    }

   
}
