using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndCondition : MonoBehaviour
{
    private HashSet<GameObject> itemsInTruckBed = new HashSet<GameObject>();  // Множество для отслеживания предметов в кузове
    private int totalItemsCount;  // Общее количество предметов

    void Start()
    {
        // Считаем все объекты с тегом "HouseholdItem"
        totalItemsCount = GameObject.FindGameObjectsWithTag("Item").Length;
    }

    void OnTriggerEnter(Collider other)
    {
        // Проверяем, что объект имеет нужный тег
        if (other.CompareTag("Item"))
        {
            // Добавляем предмет в множество, если он касается кузова
            itemsInTruckBed.Add(other.gameObject);
            CheckEndCondition();
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Удаляем предмет из множества, если он покидает кузов
        if (other.CompareTag("Item"))
        {
            itemsInTruckBed.Remove(other.gameObject);
        }
    }

    void CheckEndCondition()
    {
        // Если количество предметов в кузове равно общему количеству, завершаем игру
        if (itemsInTruckBed.Count == totalItemsCount)
        {
            EndGame();
        }
    }

    void EndGame()
    {
        Debug.Log("Все предметы загружены! Игра завершена.");
        // Здесь можно добавить логику завершения игры, например, переход на экран победы
    }
}
