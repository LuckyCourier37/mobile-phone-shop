using System.Collections;
using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CanvasFirst : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CameraManager manager; // Скрипт объекта Manager
    private bool GameStarted = false; // Флаг, сигнализирующий о состоянии игры(запущена или выключена)
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameStarted) // Вызов меню по нажатию клавиши Escape
        {

           manager.openMenu();
        }
    }

    public void GameIsStarted(bool param) // Функция меняющая состояние флага GameStarted.
    {
        GameStarted = param;
    }

    public void ExitGameFunc() // Функция завершающая работу приложения
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
