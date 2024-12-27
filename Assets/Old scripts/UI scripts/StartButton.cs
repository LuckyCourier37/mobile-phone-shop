using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    private Button button;
   [SerializeField] private  CameraManager Manager; // ������ ������� Manager
    //  private PlayerController player;
    [SerializeField] private GameObject titleControls; //  ������ Controls - �������� � ���� ������ � ������� ������ ���� ��������
    [SerializeField] private GameObject Revert;  // ������ Revert
    [SerializeField] private GameObject buttonExit;  // ������ Exit to Window
    [SerializeField] private GameObject titleScreen; // ������ ������(���������) Title Screen - �������� ������ "Start Button", "Options", "Title"
    [SerializeField] private CanvasFirst script;  // ������ ������� Canvas 
    
    
    void Start() 
    {
        button = GetComponent<Button>();
       
     //   player = GameObject.Find("Player").GetComponent<PlayerController>();

        button.onClick.AddListener(SetDifficulty); // �������������, ���������� ������� ������
    }

    // Update is called once per frame
   

    private void SetDifficulty()
    {
        Debug.Log(button.name + " Was clicked");
     //   player.StartGame();
       Manager.StartTheGame(); // �������� ���������� � ������ CameraManager

        titleControls.SetActive(true); //  �������� ������ � �������
        buttonExit.SetActive(false); // ��������� ������ Exit

        Revert.SetActive(false); // ��������� ������ Revert
        titleScreen.gameObject.SetActive(false); // ��������� ��������� ����
        script.GameIsStarted(true); // �������� ���������� � ������� ����� � ������ CanvasFirst


        Destroy(gameObject); // ���������� ������ StartButton, �.�. ������ ����

    }
}
