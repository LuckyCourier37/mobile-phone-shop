using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RaycastInteraction : MonoBehaviour
{
    public float interactionDistance = 2.0f; // Дистанция для взаимодействия
    private GameObject currentItem;
    private GameObject lastItem;
    private GameObject heldItem;
    private Vector3 screenCenter;
    private float holdDistance = 1.5f; // Дистанция удержания предмета
    [SerializeField] private float minDistance;  // Минимальная дистанция между предметом и персонажем


    private bool canRotate = true;
    [SerializeField] private BackpackUI backpackUI;

    void Start()
    {
        // Вычисляем центр экрана один раз при старте
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    void Update()
    {
        // Если предмет уже поднят, следуем за курсором с проверкой столкновений
        if (heldItem != null)
        {
            FollowMouseWithCollision();
            if (Input.GetKeyDown(KeyCode.E))
            {
                DropItem();  // Опускаем предмет
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                StoreInBackpack();
            }

            if (Input.GetKey(KeyCode.R) && canRotate)
            {
                // Поворачиваем предмет вокруг мировой оси Y
                // Проверяем, удерживается ли клавиша Shift
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                {
                    // Вращаем на 90 градусов, если Shift нажат
                    heldItem.transform.Rotate(Vector3.up, 90, Space.World);
                }
                else
                {
                    // Вращаем на 45 градусов, если Shift не нажат
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

        // Создаем луч от центра экрана через позицию камеры
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            if (hit.collider.CompareTag("Item"))
            {
                currentItem = hit.collider.gameObject;

                if (currentItem != lastItem)
                {
                    // Убираем обводку с предыдущего предмета
                    if (lastItem != null && lastItem.TryGetComponent<ObjectHighlight>(out var lastHighlight))
                    {
                        lastHighlight.DisableOutline();
                    }

                    // Включаем обводку на новом предмете
                    if (currentItem.TryGetComponent<ObjectHighlight>(out var currentHighlight))
                    {
                        currentHighlight.EnableOutline();
                    }

                    lastItem = currentItem;
                }

                // Проверка нажатия клавиши для подъема предмета
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PickUpItem();
                }
            }
        }
        else if (lastItem != null)
        {
            // Сбрасываем обводку, если луч не направлен на объект
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

        // Устанавливаем физику объекта для перемещения с учетом столкновений
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Делаем объект некенематическим для учета физики
            rb.useGravity = false;  // Отключаем гравитацию, чтобы он не падал
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        // Сбрасываем масштаб объекта на исходное значение, чтобы избежать изменений

        if (heldItem.TryGetComponent<ObjectHighlight>(out var command))
        {
            heldItem.transform.localScale = command.InitialSize();
        }

    }

    private void DropItem()
    {
        // Отключаем физику объекта и возвращаем его в обычное состояние
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false; // Делаем объект некенематическим для учета физики.
            rb.useGravity = true;  // Включаем гравитацию
        }

        heldItem = null;
    }

    private void FollowMouseWithCollision()
    {
        // Получаем позицию курсора на экране
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = holdDistance;  // Устанавливаем глубину для позиции перед персонажем

        // Конвертируем позицию экрана в мировую координату
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);

        // Если у объекта есть Rigidbody, используем MovePosition для перемещения
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, targetPos);
            if (distanceToPlayer > minDistance)
            {
                // Двигаем предмет с учетом физики, не допуская приближения ближе минимального расстояния
                rb.MovePosition(targetPos);
            }
            else
            {
                // Устанавливаем предмет на минимальной дистанции от персонажа
                rb.MovePosition(transform.position + (targetPos - transform.position).normalized * minDistance);
            }
        }
    }

    void StoreInBackpack()
    {
        if (heldItem != null && backpackUI != null)
        {
            backpackUI.AddItemToBackpack(heldItem); // Передаём предмет в рюкзак
            heldItem.SetActive(false); // Отключаем предмет в сцене
            heldItem = null; // Убираем предмет из рук
        }
    }
}
