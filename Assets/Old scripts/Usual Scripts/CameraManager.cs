using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject FirstCamera; // ��������� ������� (����������� ������)
    [SerializeField] private GameObject Person;// ������ FirstPersonController
    [SerializeField] private GameObject LastCamera; //  ������ PlayerCamera(������ ������ FirstPersonController) - ������ ���������
    [SerializeField] private OptionsUI settings; // ������ �� ������ Options
    [SerializeField]private bool tumbler = false; // ���� ��� ��������� ��������� ������
    [SerializeField] private GameObject BackPanelUI; // ���� ���������
    [SerializeField] private GameObject TitleScreen; // ������ ������(���������) Title Screen - �������� ������ "Start Button", "Options", "Title"
    [SerializeField] private GameObject ExitButton; // // ������ Exit to Window
    [SerializeField] private GameObject PersonScript; // ������ FirstPersonController
    [SerializeField] private GameObject RevertButton; // ������ Revert
    [SerializeField] private GameObject StartButton; // ������ Start Button
    [SerializeField] private GameObject Controls;  //  ������ Controls - �������� � ���� ������ � ������� ������ ���� ��������

    
    [SerializeField] private GameObject flagPanelSettings; //���� - ������ ������(���������) Panel, ������ ���������� ���� �� ����� �������� ����������.
    private bool gameIsStarted = false;  // ����, ��������������� � ��������� ����(�������� ��� ���������)
    private Vector3 SparePosition = new Vector3(20f, 1f, -16f); // ��������� ��������� ��� ���������
   
    

    // Update is called once per frame
    void Update() 
    {
        if (Input.GetKeyDown(KeyCode.O) && gameIsStarted)  // �������� ������ �� ����
        {

            GeneralView(); // ����� ������ ���������� �� ������� ������� �

        }

        if (Person.gameObject.transform.position.y < -100f ) // �������� �� ��������� �������
        {
            Person.gameObject.transform.position = SparePosition; // ������� � ��������� ���������
        }
    }
    
    public void StartTheGame() // ��������� �����
    {
        FirstCamera.SetActive(false); // ���������� ������� ������
        Person.SetActive(true); // ��������� ��������� � ��� ������

        gameIsStarted = true; // ���� ��������
    }

    private void ChangeCamera() // ������� ��������� ��������� ������
    {
        if (!tumbler)
        {
            FirstCamera.SetActive(true); // ��������� ����������� ������
            LastCamera.SetActive(false); // ���������� ������ ���������
        }
        else if (tumbler) {
            FirstCamera.SetActive(false);  // ���������� ����������� ������
            LastCamera.SetActive(true);   // ��������� ������ ��������� 

        }
       
    }

    public bool GetMeaningTumbler() // ������� ��������� �����
    {
        return tumbler;
    }
    public bool GetStatusGame() // ������� ������� ����
    {
        return gameIsStarted;
    }

    public void ChangeGameStatus(bool param) // ��������� ������� ����
    {
        gameIsStarted = param;
    }

    private void GeneralView() // ������� ������ ���� �� ����� �������� ����������
        // � ������ ������ ���������� �� ����� ����
    {
        if (BackPanelUI.activeSelf == false) // �������� ������ �� ���������
        {
            if (!tumbler) //
            {
                settings.ShowOptionsCopy(Person.gameObject.transform.position); // ��������� ���� �� ����� �������� ����������  � ������ ��������� ����������� ������
                ChangeCamera(); // �������� ����������� ������ � ��������� ������ ���������
                tumbler = true;
            }
            else
            {

                settings.CloseOptions(); // ����������� ���� �� ����� �������� ���������� � ���������� ������ � ����������� �����

                ChangeCamera(); // �������� ������ ���������  � ���������  ����������� ������
                tumbler = false;
            }
        }
    }

    public void openMenu() // ������� �������� �������� ���� ����, �������� ������ �� ����� ����
    {
        if (BackPanelUI.activeSelf == false && flagPanelSettings.activeSelf == false) // �������� ������ �� ��������� � ������� �� ���� � �������� ����������
        {
            if (!tumbler) // ����� ��������� ������� ����
            {
                Cursor.lockState = CursorLockMode.None; // ������������ ������
                Cursor.visible = true; // ������ ������ �������
                ChangeCamera(); // �������� ����������� ������ � ��������� ������ ���������
                TitleScreen.SetActive(true); // ��������� ������ "Start Button", "Options", "Title"
                ExitButton.SetActive(true);  // ��������� ������  Exit to Window
                PersonScript.SetActive(false); // ���������� ��������� � ���� ��� ��������� �����������
                tumbler = true;

                RevertButton.SetActive(true); // ��������� ������ Revert
                Controls.SetActive(false); // ��������� ������ � ������� ������ ���� ��������
                gameIsStarted=false; // ���� ��������������
            }
            else if(flagPanelSettings.activeSelf == false) // �������� ������� �� ���� � �������� ����������. ��� ��������� ������� ����
            {
                Cursor.lockState = CursorLockMode.Locked; // �������� ������� �� ������ ������
                Cursor.visible = false; // ������ ������ ���������
                ExitButton.SetActive(false);  // ���������� ������  Exit to Window
                TitleScreen.SetActive(false); // ���������� ������ "Start Button", "Options", "Title"
                PersonScript.SetActive(true); // ��������� ��������� � ���� ��� ��������� �����������
                ChangeCamera(); // �������� ������ ���������  � ���������  ����������� ������
                tumbler = false;

                Controls.SetActive(true); // �������� ������ � ������� ������ ���� ��������
                RevertButton.SetActive(false); // ���������� ������ Revert
                gameIsStarted = true; // ���� ��������
            }
        }
    }

}
