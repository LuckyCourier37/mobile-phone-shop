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


    private bool canRotate = true;
    [SerializeField] private BackpackUI backpackUI;

    void Start()
    {
        // ��������� ����� ������ ���� ��� ��� ������
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    void Update()
    {
        // ���� ������� ��� ������, ������� �� �������� � ��������� ������������
        if (heldItem != null)
        {
            FollowMouseWithCollision();
            if (Input.GetKeyDown(KeyCode.E))
            {
                DropItem();  // �������� �������
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                StoreInBackpack();
            }

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

            return;
        }

        // ������� ��� �� ������ ������ ����� ������� ������
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Item"))
            {
                currentItem = hit.collider.gameObject;

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

                // �������� ������� ������� ��� ������� ��������
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItem();
                }
            }
        }
        else if (lastItem != null)
        {
            // ���������� �������, ���� ��� �� ��������� �� ������
            if (lastItem.TryGetComponent<ObjectHighlight>(out var lastHighlight))
            {
                lastHighlight.DisableOutline();
            }
            lastItem = null;
        }
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
    }

    private void FollowMouseWithCollision()
    {
        // �������� ������� ������� �� ������
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = holdDistance;  // ������������� ������� ��� ������� ����� ����������

        // ������������ ������� ������ � ������� ����������
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);

        // ���� � ������� ���� Rigidbody, ���������� MovePosition ��� �����������
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, targetPos);
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

    void StoreInBackpack()
    {
        if (heldItem != null && backpackUI != null)
        {
            backpackUI.AddItemToBackpack(heldItem); // ������� ������� � ������
            heldItem.SetActive(false); // ��������� ������� � �����
            heldItem = null; // ������� ������� �� ���
        }
    }
}
