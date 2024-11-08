using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
    // Start is called before the first frame update
    private Button button;
    [SerializeField] private GameObject PanelControls;
    private Vector3 startPos;
    private Vector3 endPos;
    [SerializeField] private GameObject Mcamera;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject buttonExit;
    [SerializeField] private GameObject titleControls;
    
    void Start()
    {

        button = GetComponent<Button>();
        button.onClick.AddListener(ShowOptions);

        startPos = Mcamera.transform.position;
         
    }

    // Update is called once per frame
    
    public void ShowOptions()
    {
        Debug.Log(button.name + " Was clicked");
        
        PanelControls.SetActive(true);
        
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

        title.SetActive(false);
        buttonExit.SetActive(false);
       gameObject.SetActive(false);
        Mcamera.transform.position += new Vector3(0f, 0f, 100f);
    }

    public void CloseOptions()
    {
        Mcamera.transform.position = startPos;
        PanelControls.SetActive(false);
        titleControls.SetActive(true);

    }
    public void ShowOptionsCopy(Vector3 mainPos)
    {
        PanelControls.SetActive(true);
        Mcamera.transform.position = mainPos + new Vector3(0f, 15f, 0f);
        titleControls.SetActive(false);

    }

    public void RevertOptions()
    {

        PanelControls.SetActive(false);
       
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
        title.SetActive(true);
        gameObject.SetActive(true);
        buttonExit.SetActive(true);
        Mcamera.transform.position = startPos;

    }
}
