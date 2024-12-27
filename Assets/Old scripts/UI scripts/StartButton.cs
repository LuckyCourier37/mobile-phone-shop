using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    private Button button;
   [SerializeField] private  CameraManager Manager; // скрипт объекта Manager
    //  private PlayerController player;
    [SerializeField] private GameObject titleControls; //  объект Controls - содержит в себе плашку с кнопкой вызова меню настроек
    [SerializeField] private GameObject Revert;  // кнопка Revert
    [SerializeField] private GameObject buttonExit;  // кнопка Exit to Window
    [SerializeField] private GameObject titleScreen; // пустой объект(держатель) Title Screen - содержит кнопки "Start Button", "Options", "Title"
    [SerializeField] private CanvasFirst script;  // скрипт объекта Canvas 
    
    
    void Start() 
    {
        button = GetComponent<Button>();
       
     //   player = GameObject.Find("Player").GetComponent<PlayerController>();

        button.onClick.AddListener(SetDifficulty); // Инициализация, присвоение функции кнопке
    }

    // Update is called once per frame
   

    private void SetDifficulty()
    {
        Debug.Log(button.name + " Was clicked");
     //   player.StartGame();
       Manager.StartTheGame(); // Передаем информацию в скрипт CameraManager

        titleControls.SetActive(true); //  включаем плашку с кнопкой
        buttonExit.SetActive(false); // отключаем кнопку Exit

        Revert.SetActive(false); // Отключаем кнопку Revert
        titleScreen.gameObject.SetActive(false); // Отключаем начальное меню
        script.GameIsStarted(true); // Передаем информацию о запуске сцены в скрипт CanvasFirst


        Destroy(gameObject); // уничтожаем кнопку StartButton, т.е. самого себя

    }
}
