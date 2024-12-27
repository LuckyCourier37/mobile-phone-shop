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


    private bool canRotate = true; // Флаг, регулирующий вращение предмета. Предотвращает двойное вращение по нажатию клавиши R
    [SerializeField] private BackpackUI backpackUI; // Скрипт объекта Backpack Canvas
    private bool dropItem = false; // Флаг, учасвстует в создании задержки между выполнением функции DropItem() и функции HandleRaycastHighlight()

    void Start()
    {
        // Вычисляем центр экрана один раз при старте
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    void Update()
    {
        HandleInteractionInput();
        HandleItemRotation();

        if (heldItem != null) return; // Если предмет уже лежит в руках, то прекращаем вызов следующего кода.

        if(dropItem ==  false) // Проверка, выпущен ли предмет из рук
        {
            HandleRaycastHighlight();
        }
       



    }

    private void HandleInteractionInput() // Если предмет уже лежит в "руках".
    {
        if (heldItem != null) // Проверка поднят ли предмет
        {
            FollowMouseWithCollision(); // Следование предмета за курсором

            // Отпустить предмет по нажатию "E"
            if (Input.GetKeyDown(KeyCode.E))
            {
                DropItem();  // Опускаем предмет
            }

            // Сложить предмет в рюкзак по нажатию клавиши "F"
            if (Input.GetKeyDown(KeyCode.F))
            {
                StoreInBackpack();
            }

        }
    }

    private void HandleItemRotation() // Вращение предмета в "руках"
    {
        // Проверяем вращение для удерживаемого предмета
        if (heldItem != null)
        {
            // Проверка нажатия клавиши "R" для поворота
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
        }
    }

    private void HandleRaycastHighlight() // создаёт луч, направленный от центра экрана вперёд.
            // Если луч попадает на объект с тегом "Item", включается подсветка и даётся возможность поднять предмет по нажатию клавиши E.
    {       // Если луч не попадает на предмет, подсветка сбрасывается.

        // Создаем луч от центра экрана через позицию камеры
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit; //  переменная RaycastHit, в которую будет сохранена информация о столкновении, если луч пересечёт Collider

        if (Physics.Raycast(ray, out hit, interactionDistance) && hit.collider.CompareTag("Item"))
        {    // Если луч пересекает collider объекта, то информация о столкновении сохраняется в RaycastHit hit.

            currentItem = hit.collider.gameObject;
            UpdateItemHighlight();

            // Проверка нажатия клавиши для подъема предмета
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUpItem();
            }
        }
        else
        {
            ResetLastItemHighlight(); // Работает в случае когда отходишь от всех предметов,  отключает выделение с последнего предмета
        }
    }
    private void UpdateItemHighlight() // Включает подсветку (highlight) текущего предмета.
                                       // Подсветка служит визуальной подсказкой, что этот предмет может быть подобран.
    {
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
    }
    private void ResetLastItemHighlight() // Убирает подсветку с последнего выделенного предмета.
    {
        if (lastItem != null && lastItem.TryGetComponent<ObjectHighlight>(out var lastHighlight))
        {
            lastHighlight.DisableOutline(); // Отключаем обводку предмета
        }
        lastItem = null;
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
        
        StartCoroutine(Delay());
    }

    private void FollowMouseWithCollision() // Функция, организующая следование предмета за курсором.
    {                                      // Предмет в данном случае лежит в руках

        // Получаем позицию курсора на экране
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = holdDistance;  // Устанавливаем глубину для позиции перед персонажем

        // Конвертируем позицию экрана в мировую координату
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);

        // Если у объекта есть Rigidbody, используем MovePosition для перемещения
        Rigidbody rb = heldItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, targetPos); // Вычисление дистанции между персонажем и предметом
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

    void StoreInBackpack() // Функция Выполняет роль переноса вещи из рук в рюкзак
    {
        if (heldItem != null && backpackUI != null)
        {
            backpackUI.AddItemToBackpack(heldItem); // Передаём предмет в рюкзак
            heldItem.SetActive(false); // Отключаем предмет в сцене
            heldItem = null; // Убираем предмет из рук
        }
    }

    IEnumerator Delay() // Необходимая задержка между выполением функции DropItem() и функции HandleRaycastHighlight()
    {  // Без неё начинаются проблемы с переключением состояний подобрал, положил. Предмет просто не опускается на землю.
       // В предыдущих скриптах срабатывала директива return (для выхода из метода Update()), здесь почему - то не работает.
        dropItem = true;
        yield return new WaitForSeconds(0.2f);
        dropItem = false;
    }
}
