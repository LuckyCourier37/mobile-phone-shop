using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackUI : MonoBehaviour
{

    public Transform itemsGrid; // ����� UI ��� ����������� ������
    public List<GameObject> itemPrefabs; // ��������� �������� ������
    public Color selectedColor = Color.yellow; // ���� ��� ��������� ��������� ������
    public Color defaultColor = Color.white; // ���� ��� ����������� ������

    private Dictionary<GameObject, GameObject> backpackItems = new Dictionary<GameObject, GameObject>(); // ������� ��� �������� ��������� � �� ������
    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>(); // ������� ��� �������� ������������ ���������
    private List<GameObject> itemIconsList = new List<GameObject>(); // ������ ������ ��������� ��� ���������
    private int selectedIndex = 0; // ������ �������� ���������� ��������
    [SerializeField] private GameObject PersonPosition;
    [SerializeField] private GameObject PanelUI;
    [SerializeField] private CameraManager manager;
    public float highlightScale = 1.5f;
    private bool isHighlighted = false; // ���� ��� ������������ ��������� ���������

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && manager.GetMeaningTumbler() == false && manager.GetStatusGame())
        {
            if (PanelUI.activeSelf == true) {
                PanelUI.SetActive(false); // ����������� �����
            }
            else PanelUI.SetActive(true);

        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveSelection(-1); // ����������� �����
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveSelection(1); // ����������� ����
        }
        else if (Input.GetKeyDown(KeyCode.Q) && PanelUI.activeSelf == true) // "Q" ��� �������� ��������
        {
            if (itemIconsList.Count > 0 && selectedIndex >= 0 && selectedIndex < itemIconsList.Count)
            {
                GameObject item = FindOriginalItemByIcon(itemIconsList[selectedIndex]);
                if (item != null)
                {
                    RemoveItemFromBackpack(item);
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.V) && PanelUI.activeSelf == true) // "Space" ��� ���������� ���������� ������
        {
            ToggleHighlight();
        }
    }

    // ������� ��� ���������� �������� � ������
    public void AddItemToBackpack(GameObject item)
    {
        GameObject itemIconPrefab = itemPrefabs.Find(prefab => prefab.name == item.name);

        if (itemIconPrefab != null)
        {
            GameObject newItemIcon = Instantiate(itemIconPrefab, itemsGrid);
            backpackItems[item] = newItemIcon; // ��������� ����� ����� ��������� � �������
            itemIconsList.Add(newItemIcon); // ��������� ������ � ������ ��� ���������

            // ��������� �������� �������
            originalScales[newItemIcon] = newItemIcon.transform.localScale;

            item.SetActive(false); // �������� ������� � �����
            UpdateSelectionVisual(); // ��������� ���������
        }
    }

    // ������� ��� �������� �������� �� �������
    public void RemoveItemFromBackpack(GameObject item)
    {
        if (backpackItems.ContainsKey(item))
        {
            item.SetActive(true);
            item.GetComponent<Rigidbody>().useGravity = true;
            item.transform.position = PersonPosition.transform.position + PersonPosition.transform.forward * 2;

            GameObject itemIcon = backpackItems[item];
            itemIconsList.Remove(itemIcon); // ������� ������ �� ������ ���������
            originalScales.Remove(itemIcon); // ������� �� ������� ���������

            Destroy(itemIcon);
            backpackItems.Remove(item);
            selectedIndex = Mathf.Clamp(selectedIndex, 0, itemIconsList.Count - 1); // ��������� ������
            UpdateSelectionVisual(); // ��������� ���������
        }
    }

    // ����� ��� ����������� ���������
    private void MoveSelection(int direction)
    {
        if (itemIconsList.Count == 0) return;
        selectedIndex = (selectedIndex + direction + itemIconsList.Count) % itemIconsList.Count; // ����������� �������
        isHighlighted = false;
        UpdateSelectionVisual();
    }

    // ����� ��� ����������� ���������� ���������
    private void UpdateSelectionVisual()
    {
        for (int i = 0; i < itemIconsList.Count; i++)
        {
            GameObject iconObject = itemIconsList[i];
            Image iconImage = iconObject.GetComponent<Image>();
            iconImage.color = (i == selectedIndex) ? selectedColor : defaultColor;

            // ������������� ������� ��� ���������� ������
            Vector3 baseScale = originalScales[iconObject];
            iconObject.transform.localScale = (i == selectedIndex && isHighlighted) ? baseScale * highlightScale : baseScale;
        }
    }

    // ��������������� ����� ��� ������ ������������� �������� �� ������
    private GameObject FindOriginalItemByIcon(GameObject icon)
    {
        foreach (var pair in backpackItems)
        {
            if (pair.Value == icon)
            {
                return pair.Key; // ���������� ������������ �������, ��������������� ������
            }
        }
        return null;
    }

    private void ToggleHighlight()
    {
        isHighlighted = !isHighlighted; // ����������� ��������� ���������
        UpdateSelectionVisual(); // ��������� ���������� �����������
    }
}
