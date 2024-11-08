using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlight : MonoBehaviour
{
   
    [SerializeField] private Material outlineMaterial;  // Материал с эффектом обводки
    private Material[] originalMaterials;               // Оригинальные материалы
    private Material[] outlineMaterials;                // Материалы с обводкой
    private Renderer objRenderer;
    private Vector3 NativeSize;


    void Start()
    {
        NativeSize = gameObject.transform.localScale;

        objRenderer = GetComponent<Renderer>();

        // Сохраняем исходные материалы и создаем массив для обводки
        originalMaterials = objRenderer.materials;
        outlineMaterials = new Material[originalMaterials.Length];

        // Заполняем массив обводки
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            outlineMaterials[i] = outlineMaterial;
        }
    }

    public void EnableOutline()
    {
        // Устанавливаем массив материалов с обводкой
        objRenderer.materials = outlineMaterials;
    }

    public void DisableOutline()
    {
        // Возвращаем исходные материалы
        objRenderer.materials = originalMaterials;
    }

    public Vector3 InitialSize()
    {
        return NativeSize;
    }
}
