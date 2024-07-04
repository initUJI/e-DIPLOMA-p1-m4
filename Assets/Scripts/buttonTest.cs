
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class buttonTest : MonoBehaviour
{
    private EventLogger eventLogger;
    public void startClick()
    {
        ClearAllText(transform.parent.parent);
        ActivateLastChildInHierarchy(transform.parent.parent.transform);
        gameObject.SetActive(false);
        eventLogger = FindFirstObjectByType<EventLogger>();
        eventLogger.LogEvent("Start button pressed");
    }

    void ClearAllText(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.name == "NameText")
            {
                TextMeshProUGUI textComponent = child.GetComponent<TextMeshProUGUI>();
                if (textComponent != null)
                {
                    textComponent.text = string.Empty;
                }
            }

            // Llama recursivamente a la función para los hijos del objeto actual
            ClearAllText(child);
        }
    }

    void ActivateLastChildInHierarchy(Transform parent)
    {
        // Activar el último hijo si el objeto tiene hijos
        if (parent.childCount > 0)
        {
            Transform lastChild = parent.GetChild(parent.childCount - 1);
            lastChild.gameObject.SetActive(true);
        }
    }
}

