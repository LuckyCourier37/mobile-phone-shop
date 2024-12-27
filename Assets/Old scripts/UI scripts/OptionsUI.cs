using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    // Start is called before the first frame update
    private Button button; // ������ ������ ������� �� ��������� Button
    [SerializeField] private GameObject PanelControls; // ������ ������(���������) Panel, ������ ���������� ���� �� ����� �������� ����������.
    private Vector3 startPos; // ����� �������� ��������� ��������� ������� ��������� ������.
    
    [SerializeField] private GameObject Maincamera; // ������ �� ������� ������ - Maincamera
    [SerializeField] private GameObject startButton; // ������ Start Button
    [SerializeField] private GameObject title; // ������ �� ��������� ���� Title - ����� ���������� ���������, �������� �����.
    [SerializeField] private GameObject buttonExit; // ������ Exit to Window
    [SerializeField] private GameObject titleControls; //  ������ Controls - �������� � ���� ������ � ������� ������ ���� ��������

    void Start()
    {

        button = GetComponent<Button>();
        button.onClick.AddListener(ShowOptions);  // �������������, ���������� ������� ������

        startPos = Maincamera.transform.position; // ���������� �������������� ������� ������� ������
         
    }

    // Update is called once per frame
    
    public void ShowOptions() // ������� ������� ������ Options
    {
        Debug.Log(button.name + " Was clicked");
        
        PanelControls.SetActive(true); // ��������� ���� � �������� ����������
        
        try
        {
            // �������� ���������� � SomeButton
            startButton.SetActive(false); // ������ �������� � ��������
        }
        catch (MissingReferenceException e)
        {
            // ���� ������ ��� ���������, ����� ���������� � ������� ���������
            Debug.Log("������ SomeButton �� ����������: " + e.Message);
        }

        title.SetActive(false); // ���������� ��������� �����
        buttonExit.SetActive(false); // ���������� ������  Exit to Window
        gameObject.SetActive(false); // ���������� ������ Options
        Maincamera.transform.position += new Vector3(0f, 0f, 100f); // ���������� ������� ������ �� 100 ������ ����� ��� Z
    }

    public void RevertOptions() // ������ ������� ����������� � ������ Revert
    {

        PanelControls.SetActive(false); // ���������� ���� � �������� ����������

        try
        {
            // �������� ���������� � SomeButton
            startButton.SetActive(true); // ������ �������� � ��������
        }
        catch (MissingReferenceException e)
        {
            // ���� ������ ��� ���������, ����� ���������� � ������� ���������
            Debug.Log("������ SomeButton �� ����������: " + e.Message);
        }
        title.SetActive(true); // ��������� ��������� �����
        gameObject.SetActive(true);  // ��������� ������ Options
        buttonExit.SetActive(true); // ��������� ������  Exit to Window
        Maincamera.transform.position = startPos;

    }

    public void CloseOptions() // ��� ������� ���������� �� ������� CameraManager, �������� ����� ������� �����.
    {                         // ����������� ���� �� ����� �������� ����������(��� ����� �����) � (���������� ������ � ����������� �����) - � ���� �������� � ������� CameraManager

        Maincamera.transform.position = startPos; // ������� ��������� �������(�����������) ������ � �������� ���������
        PanelControls.SetActive(false); // ���������� ���� � �������� ����������
        titleControls.SetActive(true); // ��������� ������ � ������� ������ ���� ��������

    }
    public void ShowOptionsCopy(Vector3 mainPos) // ��� ������� ���������� �� ������� CameraManager, �������� ����� ������� �����.
    {                                            // ��������� ���� �� ����� �������� ����������  � ������ ��������� ����������� ������.

        PanelControls.SetActive(true); // ��������� ���� � �������� ����������
        Maincamera.transform.position = mainPos + new Vector3(0f, 15f, 0f); // ��������� ��������� �������(�����������) ������
        titleControls.SetActive(false); // ���������� ������ � ������� ������ ���� ��������

    }

   
}
