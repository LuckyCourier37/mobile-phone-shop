using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject FirstCamera;
    [SerializeField] private GameObject Person;
    [SerializeField] private GameObject LastCamera;
    [SerializeField] private OptionsUI settings;
    [SerializeField]private bool tumbler = false;
    [SerializeField] private GameObject PanelUI;
    [SerializeField] private GameObject GeneralMenu;
    [SerializeField] private GameObject ExitButton;
    [SerializeField] private GameObject PersonScript;
    [SerializeField] private GameObject RevertButton;
    [SerializeField] private GameObject StartButton;
    [SerializeField] private GameObject Controls;
    [SerializeField] private GameObject flagtoCloseMenu;
    [SerializeField] private GameObject flagPanelSettings;
    private bool gameIsStarted = false;
    private Vector3 SparePosition = new Vector3(20f, 1f, -16f);
    private bool MenuIsOpened = false;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && gameIsStarted)
        {

            GeneralView();

        }

        if (Person.gameObject.transform.position.y < -100f )
        {
            Person.gameObject.transform.position = SparePosition;
        }
    }
    
    public void StartTheGame()
    {
        FirstCamera.SetActive(false);
        Person.SetActive(true);

        gameIsStarted = true;
    }

    private void ChangeCamera()
    {
        if (!tumbler)
        {
            FirstCamera.SetActive(true);
            LastCamera.SetActive(false);
        }
        else if (tumbler) {
            FirstCamera.SetActive(false);
            LastCamera.SetActive(true);

        }
       
    }

    public bool GetMeaningTumbler()
    {
        return tumbler;
    }
    public bool GetStatusGame()
    {
        return gameIsStarted;
    }

    public void ChangeGameStatus(bool param)
    {
        gameIsStarted = param;
    }

    private void GeneralView()
    {
        if (PanelUI.activeSelf == false)
        {
            if (!tumbler)
            {
                settings.ShowOptionsCopy(Person.gameObject.transform.position);
                ChangeCamera();
                tumbler = true;
            }
            else
            {

                settings.CloseOptions();

                ChangeCamera();
                tumbler = false;
            }
        }
    }

    public void openMenu()
    {
        if (PanelUI.activeSelf == false && flagPanelSettings.activeSelf == false)
        {
            if (!tumbler)
            {
                Cursor.lockState = CursorLockMode.None; // Разблокируем курсор
                Cursor.visible = true;
                ChangeCamera();
                GeneralMenu.SetActive(true);
                ExitButton.SetActive(true);
                PersonScript.SetActive(false);
                tumbler = true;

                RevertButton.SetActive(true);
                Controls.SetActive(false);
                gameIsStarted=false;
            }
            else if(flagtoCloseMenu.activeSelf == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                ExitButton.SetActive(false);
                GeneralMenu.SetActive(false);
                PersonScript.SetActive(true);
                ChangeCamera();
                tumbler = false;

                Controls.SetActive(true);
                RevertButton.SetActive(false) ;
                gameIsStarted=true;
            }
        }
    }

}
