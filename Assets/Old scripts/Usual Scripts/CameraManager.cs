using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject FirstCamera; // Положение главной (статической камеры)
    [SerializeField] private GameObject Person;// Объект FirstPersonController
    [SerializeField] private GameObject LastCamera; //  Объект PlayerCamera(вложен внутрь FirstPersonController) - камера персонажа
    [SerializeField] private OptionsUI settings; // Скрипт из кнопки Options
    [SerializeField]private bool tumbler = false; // Флаг для изменения положения камеры
    [SerializeField] private GameObject BackPanelUI; // Меню инвентаря
    [SerializeField] private GameObject TitleScreen; // пустой объект(держатель) Title Screen - содержит кнопки "Start Button", "Options", "Title"
    [SerializeField] private GameObject ExitButton; // // Кнопка Exit to Window
    [SerializeField] private GameObject PersonScript; // Объект FirstPersonController
    [SerializeField] private GameObject RevertButton; // кнопка Revert
    [SerializeField] private GameObject StartButton; // Кнопка Start Button
    [SerializeField] private GameObject Controls;  //  объект Controls - содержит в себе плашку с кнопкой вызова меню настроек

    
    [SerializeField] private GameObject flagPanelSettings; //флаг - пустой объект(держатель) Panel, внутри содержится окно со всеми кнопками управления.
    private bool gameIsStarted = false;  // Флаг, сигнализирующий о состоянии игры(запущена или выключена)
    private Vector3 SparePosition = new Vector3(20f, 1f, -16f); // Начальное положение дял персонажа
   
    

    // Update is called once per frame
    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.O) && gameIsStarted)  // Проверка начата ли игра
        {

            GeneralView(); // Вызов клавиш управления по нажатию клавиши О

        }

        if (Person.gameObject.transform.position.y < -100f ) // Проверка на свободное падение
        {
            Person.gameObject.transform.position = SparePosition; // Возврат в начальное положение
        }
    }
    
    public void StartTheGame() // Инициация сцены
    {
        FirstCamera.SetActive(false); // Отключение главной камеры
        Person.SetActive(true); // Включение персонажа и его камеры

        gameIsStarted = true; // Игра началась
    }

    private void ChangeCamera() // Функция изменения положения камеры
    {
        if (!tumbler)
        {
            FirstCamera.SetActive(true); // Включение статической камеры
            LastCamera.SetActive(false); // Отключение камеры персонажа
        }
        else if (tumbler) {
            FirstCamera.SetActive(false);  // Отключение статической камеры
            LastCamera.SetActive(true);   // Включение камеры персонажа 

        }
       
    }

    public bool GetMeaningTumbler() // Возврат положения флага
    {
        return tumbler;
    }
    public bool GetStatusGame() // Возврат статуса игры
    {
        return gameIsStarted;
    }

    public void ChangeGameStatus(bool param) // Изменение статуса игры
    {
        gameIsStarted = param;
    }

    private void GeneralView() // Функция вызова окна со всеми кнопками управления
        // В данном случае вызывается во время игры
    {
        if (BackPanelUI.activeSelf == false) // Проверка открыт ли инвентарь
        {
            if (!tumbler) //
            {
                settings.ShowOptionsCopy(Person.gameObject.transform.position); // Открываем окно со всеми кнопками управления  и меняем положение статической камеры
                ChangeCamera(); // Включаем статическую камеру и отключаем камеру персонажа
                tumbler = true;
            }
            else
            {

                settings.CloseOptions(); // Сворачиваем окно со всеми кнопками управления и возвращаем камеру в действующую сцену

                ChangeCamera(); // Включаем камеру персонажа  и отключаем  статическую камеру
                tumbler = false;
            }
        }
    }

    public void openMenu() // Функция открытия главного меню игры, доступна только во время игры
    {
        if (BackPanelUI.activeSelf == false && flagPanelSettings.activeSelf == false) // Проверка закрыт ли инвентарь и закрыто ли окно с кнопками управления
        {
            if (!tumbler) // Здесь открываем главное меню
            {
                Cursor.lockState = CursorLockMode.None; // Разблокируем курсор
                Cursor.visible = true; // Делаем курсор видимым
                ChangeCamera(); // Включаем статическую камеру и отключаем камеру персонажа
                TitleScreen.SetActive(true); // Включение кнопок "Start Button", "Options", "Title"
                ExitButton.SetActive(true);  // Включение кнопки  Exit to Window
                PersonScript.SetActive(false); // Отключение персонажа и всех его вложенных компонентов
                tumbler = true;

                RevertButton.SetActive(true); // Включение кнопки Revert
                Controls.SetActive(false); // Отключаем плашку с кнопкой вызова меню настроек
                gameIsStarted=false; // игра приостановлена
            }
            else if(flagPanelSettings.activeSelf == false) // Проверка закрыто ли окно с кнопками управления. Тут закрываем главное меню
            {
                Cursor.lockState = CursorLockMode.Locked; // фиксация курсора по центру экрана
                Cursor.visible = false; // Делаем курсор невидимым
                ExitButton.SetActive(false);  // Отключение кнопки  Exit to Window
                TitleScreen.SetActive(false); // Отключение кнопок "Start Button", "Options", "Title"
                PersonScript.SetActive(true); // Включение персонажа и всех его вложенных компонентов
                ChangeCamera(); // Включаем камеру персонажа  и отключаем  статическую камеру
                tumbler = false;

                Controls.SetActive(true); // Включаем плашку с кнопкой вызова меню настроек
                RevertButton.SetActive(false); // Отключение кнопки Revert
                gameIsStarted = true; // Игра запущена
            }
        }
    }

}
