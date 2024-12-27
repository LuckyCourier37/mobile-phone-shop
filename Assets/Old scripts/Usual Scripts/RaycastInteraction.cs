using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaycastInteraction : MonoBehaviour
{
    public float interactionDistance = 2.0f; // ��������� ��� ��������������
    private GameObject currentItem;
    private GameObject lastItem;
    private GameObject heldItem;
    private Vector3 screenCenter;
    private float holdDistance = 1.5f; // ��������� ��������� ��������
    [SerializeField] private float minDistance;  // ����������� ��������� ����� ��������� � ����������


    private bool canRotate = true; // ����, ������������ �������� ��������. ������������� ������� �������� �� ������� ������� R
    [SerializeField] private BackpackUI backpackUI; // ������ ������� Backpack Canvas
    private bool dropItem = false; // ����, ���������� � �������� �������� ����� ����������� ������� DropItem() � ������� HandleRaycastHighlight()

    void Start()
    {
        // ��������� ����� ������ ���� ��� ��� ������
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    void Update()
    {
        HandleInteractionInput();
        HandleItemRotation();

        if (heldItem != null) return; // ���� ������� ��� ����� � �����, �� ���������� ����� ���������� ����.

        if(dropItem ==  false) // ��������, ������� �� ������� �� ���
        {
            HandleRaycastHighlight();
        }
       



    }

    private void HandleInteractionInput() // ���� ������� ��� ����� � "�����".
    {
        if (heldItem != null) // �������� ������ �� �������
        {
            FollowMouseWithCollision(); // ���������� �������� �� ��������

            // ��������� ������� �� ������� "E"
            if (Input.GetKeyDown(KeyCode.E))
            {
                DropItem();  // �������� �������
            }

            // ������� ������� � ������ �� ������� ������� "F"
            if (Input.GetKeyDown(KeyCode.F))
            {
                StoreInBackpack();
            }

        }
    }

    private void HandleItemRotation() // �������� �������� � "�����"
    {
        // ��������� �������� ��� ������������� ��������
        if (heldItem != null)
        {
            // �������� ������� ������� "R" ��� ��������
            if (Input.GetKey(KeyCode.R) && canRotate)
            {
                // ������������ ������� ������ ������� ��� Y
                // ���������, ������������ �� ������� Shift
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    // ������� �� 90 ��������, ���� Shift �����
                    heldItem.transform.Rotate(Vector3.up, 90, Space.World);
                }
                else
                {
                    // ������� �� 45 ��������, ���� Shift �� �����
                    heldItem.transform.Rotate(Vector3.up, 45, Space.World);
                }
                canRotate = false;
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                canRotate = true;
            }
        }
    }

    private void HandleRaycastHighlight() // ������ ���, ������������ �� ������ ������ �����.
            // ���� ��� �������� �� ������ � ����� "Item", ���������� ��������� � ����� ����������� ������� ������� �� ������� ������� E.
    {       // ���� ��� �� �������� �� �������, ��������� ������������.

        // ������� ��� �� ������ ������ ����� ������� ������
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit; //  ���������� RaycastHit, � ������� ����� ��������� ���������� � ������������, ���� ��� ��������� Collider

        if (Physics.Raycast(ray, out hit, interactionDistance) && hit.collider.CompareTag("Item"))
        {    // ���� ��� ���������� collider �������, �� ���������� � ������������ ����������� � RaycastHit hit.

            currentItem = hit.collider.gameObject;
            UpdateItemHighlight();

            // �������� ������� ������� ��� ������� ��������
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpItem();
            }
        }
        else
        {
            ResetLastItemHighlight(); // �������� � ������ ����� �������� �� ���� ���������,  ��������� ��������� � ���������� ��������
        }
    }
    private void UpdateItemHighlight() // �������� ��������� (highlight) �������� ��������.
                                       // ��������� ������ ���������� ����������, ��� ���� ������� ����� ���� ��������.
    {
        if (currentItem != lastItem)
        {
            // ������� ������� � ����������� ��������
            if (lastItem != null && lastItem.TryGetComponent<ObjectHighlight>(out var lastHighlight))
            {
                lastHighlight.DisableOutline();
            }

            // �������� ������� �� ����� ��������
            if (currentItem.TryGetComponent<ObjectHighlight>(out var currentHighlight))
            {
                currentHighlight.EnableOutline();
            }

            lastItem = currentItem;
        }
    }
    private void ResetLastItemHighlight() // ������� ��������� � ���������� ����������� ��������.
    {
        if (lastItem != null && lastItem.TryGetComponent<ObjectHighlight>(out var lastHighlight))
        {
            lastHighlight.DisableOutline(); // ��������� ������� ��������
        }
        lastItem = null;
    }

    private void PickUpItem()
    {
        heldItem = currentItem;

        // ������������� ������ ������� ��� ����������� � ������ ������������
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // ������ ������ ���������������� ��� ����� ������
            rb.useGravity = false;  // ��������� ����������, ����� �� �� �����
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        // ���������� ������� ������� �� �������� ��������, ����� �������� ���������
        if (heldItem.TryGetComponent<ObjectHighlight>(out var command))
        {
            heldItem.transform.localScale = command.InitialSize();
        }

    }

    private void DropItem()
    {
        // ��������� ������ ������� � ���������� ��� � ������� ���������
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // ������ ������ ���������������� ��� ����� ������.
            rb.useGravity = true;  // �������� ����������
        }
       
        heldItem = null;
        
        StartCoroutine(Delay());
    }

    private void FollowMouseWithCollision() // �������, ������������ ���������� �������� �� ��������.
    {                                      // ������� � ������ ������ ����� � �����

        // �������� ������� ������� �� ������
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = holdDistance;  // ������������� ������� ��� ������� ����� ����������

        // ������������ ������� ������ � ������� ����������
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);

        // ���� � ������� ���� Rigidbody, ���������� MovePosition ��� �����������
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, targetPos); // ���������� ��������� ����� ���������� � ���������
            if (distanceToPlayer > minDistance)
            {
                // ������� ������� � ������ ������, �� �������� ����������� ����� ������������ ����������
                rb.MovePosition(targetPos);
            }
            else
            {
                // ������������� ������� �� ����������� ��������� �� ���������
                rb.MovePosition(transform.position + (targetPos - transform.position).normalized * minDistance);
            }
        }
    }

    void StoreInBackpack() // ������� ��������� ���� �������� ���� �� ��� � ������
    {
        if (heldItem != null && backpackUI != null)
        {
            backpackUI.AddItemToBackpack(heldItem); // ������� ������� � ������
            heldItem.SetActive(false); // ��������� ������� � �����
            heldItem = null; // ������� ������� �� ���
        }
    }

    IEnumerator Delay() // ����������� �������� ����� ���������� ������� DropItem() � ������� HandleRaycastHighlight()
    {  // ��� �� ���������� �������� � ������������� ��������� ��������, �������. ������� ������ �� ���������� �� �����.
       // � ���������� �������� ����������� ��������� return (��� ������ �� ������ Update()), ����� ������ - �� �� ��������.
        dropItem = true;
        yield return new WaitForSeconds(0.2f);
        dropItem = false;
    }
}
