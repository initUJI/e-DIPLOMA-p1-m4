using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoProcessor : MonoBehaviour
{
    public TextMeshProUGUI statusText;  // Texto para mostrar el estado sobre el objeto raíz
    private List<UICollisionDetector> uiCollisionDetectors = new List<UICollisionDetector>();  // Lista para almacenar UICollisionDetectors
    private int solvedCount = 0;  // Contador de UICollisionDetectors con solved = true

    private void Start()
    {

        InitializeInfoObjects(transform.GetChild(0));
    }

    private void Update()
    {
            // Reiniciar contador de solucionados
            solvedCount = 0;

            // Contar UICollisionDetectors con solved = true
            foreach (var detector in uiCollisionDetectors)
            {
                if (detector.solved)
                {
                    solvedCount++;
                }
            }

            // Desactivar todos los objetos Info si todos los textos están llenos
            if (solvedCount == uiCollisionDetectors.Count && uiCollisionDetectors.Count>0)
            {
                DeactivateAllInfoObjects(transform.GetChild(0));

                // Mostrar el resultado encima del objeto raíz
                statusText.text = $"{solvedCount} / {uiCollisionDetectors.Count}";
            }
    }

    private void InitializeInfoObjects(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.childCount > 0) // Verificar si el objeto tiene hijos
            {
                Transform infoChild = child.GetChild(0);  // Obtener el primer hijo (Info)
                if (infoChild.CompareTag("Info"))
                {
                    // Buscar NameText y verificar su TextMeshProUGUI
                    Transform nameTextTransform = infoChild.Find("NameText");
                    if (nameTextTransform != null)
                    {
                        TextMeshProUGUI textComponent = nameTextTransform.GetComponent<TextMeshProUGUI>();
                        if (textComponent != null && !string.IsNullOrWhiteSpace(textComponent.text))
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
            }
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

