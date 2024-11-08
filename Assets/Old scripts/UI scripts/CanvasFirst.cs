using System.Collections;
using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CanvasFirst : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] CameraManager manager;
    private bool GameStarted = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameStarted)
        {

           manager.openMenu();
        }
    }

    public void GameIsStarted(bool param)
    {
        GameStarted = param;
    }

    public void ExitGameFunc()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }
}
