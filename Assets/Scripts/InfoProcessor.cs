using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoProcessor : MonoBehaviour
{
    public TextMeshProUGUI statusText;  // Texto para mostrar el estado sobre el objeto raíz
    public List<UICollisionDetector> uiCollisionDetectors = new List<UICollisionDetector>();  // Lista para almacenar UICollisionDetectors
    [HideInInspector]
    public int solvedCount = 0;  // Contador de UICollisionDetectors con solved = true

    private void AddUICollisionDetector(Transform infoChild)
    {
        // Buscar NameText de forma manual, incluso si está desactivado
        Transform nameTextTransform = FindChildRecursively(infoChild, "NameText");

        if (nameTextTransform != null)
        {
            TextMeshProUGUI textComponent = nameTextTransform.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                // Buscar y agregar UICollisionDetectors a la lista
                UICollisionDetector detector = nameTextTransform.GetComponent<UICollisionDetector>();
                if (detector != null)
                {
                    uiCollisionDetectors.Add(detector);
                }
            }
        }
    }

    private Transform FindChildRecursively(Transform parent, string childName)
    {
        // Recorremos todos los hijos del parent
        foreach (Transform child in parent)
        {
            // Si el nombre coincide, devolvemos el transform
            if (child.name == childName)
            {
                return child;
            }

            // Si no, buscamos en los hijos de este hijo de forma recursiva
            Transform result = FindChildRecursively(child, childName);
            if (result != null)
            {
                return result;
            }
        }

        // Si no encontramos nada, devolvemos null
        return null;
    }


    public void UpdateSolvedCount()
    {
        solvedCount = 0;  // Reiniciar contador de solucionados

        // Contar UICollisionDetectors con solved = true
        foreach (var detector in uiCollisionDetectors)
        {
            if (detector.solved)
            {
                solvedCount++;
            }
        }
        Debug.Log($"solved count: {solvedCount}");
    }

    public void UpdateStatusText()
    {
        // Mostrar el resultado encima del objeto raíz
        //statusText.text = $"{solvedCount} / {uiCollisionDetectors.Count}";
    }

    public void CheckAndDeactivateInfoObjects()
    {
        // Desactivar todos los objetos Info si todos los textos están llenos
        if (solvedCount == uiCollisionDetectors.Count && uiCollisionDetectors.Count > 0)
        {
            DeactivateAllInfoObjects(transform.GetChild(0));
        }
    }

    private void DeactivateAllInfoObjects(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.childCount > 0) // Verificar si el objeto tiene hijos
            {
                Transform infoChild = child.GetChild(0);  // Obtener el primer hijo (Info)
                if (infoChild.CompareTag("Info"))
                {
                    infoChild.gameObject.SetActive(false);
                }
            }
        }
    }
}


