
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;

public class buttonTest : MonoBehaviour
{
    private EventLogger eventLogger;
    private TextMeshProUGUI[] nameTexts;
    private List<GameObject> parentObjects = new List<GameObject>();

    void Start()
    {
        // Al iniciar, guarda todos los TextMeshProUGUI con nombre "NameText" en el array
        CaptureAllNameTexts();

        // Desactiva los objetos padres de los TextMeshProUGUI capturados
        DeactivateParentObjects();
    }

    void CaptureAllNameTexts()
    {
        // Encuentra todos los objetos con el componente TextMeshProUGUI, incluyendo los inactivos
        TextMeshProUGUI[] allTextComponents = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        List<TextMeshProUGUI> nameTextList = new List<TextMeshProUGUI>();

        // Filtra solo aquellos con el nombre "NameText"
        foreach (TextMeshProUGUI textComponent in allTextComponents)
        {
            if (textComponent.name == "NameText")
            {
                nameTextList.Add(textComponent);
                // Añade el objeto padre a la lista
                parentObjects.Add(textComponent.gameObject.transform.parent.gameObject);
            }
        }

        // Guarda el resultado en el array
        nameTexts = nameTextList.ToArray();
    }

    void DeactivateParentObjects()
    {
        // Desactiva todos los objetos padres
        foreach (GameObject parent in parentObjects)
        {
            parent.SetActive(false);
        }
    }

    public void startClick()
    {
        ClearAllText();
        ActivateLastChildInHierarchy(transform.parent.parent.transform);
        gameObject.SetActive(false);
        DeactivateLocalizeStringEvent();
        eventLogger = FindFirstObjectByType<EventLogger>();
        eventLogger.LogEvent("Start button pressed");
    }

    public void ClearAllText()
    {
        // Limpia los textos de todos los componentes guardados en el array
        foreach (TextMeshProUGUI textComponent in nameTexts)
        {
            textComponent.text = string.Empty;
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

    public void DeactivateLocalizeStringEvent()
    {
        foreach (TextMeshProUGUI textComponent in nameTexts)
        {
            LocalizeStringEvent localizeStringEvent = textComponent.GetComponent<LocalizeStringEvent>();
            if (localizeStringEvent != null)
            {
                localizeStringEvent.enabled = false;
            }
            else
            {
                Debug.LogWarning($"No LocalizeStringEvent component found on {textComponent.gameObject.name}.");
            }
        }
    }
}

