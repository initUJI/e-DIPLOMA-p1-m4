using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.UIElements;

public class buttonTest : MonoBehaviour
{
    private EventLogger eventLogger;
    private List<List<TextMeshProUGUI>> nameTexts = new List<List<TextMeshProUGUI>>();
    private List<GameObject> parentObjects = new List<GameObject>();

    [SerializeField]
    private List<GameObject> components = new List<GameObject>();
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject finishButton;

    void Start()
    {
        foreach (GameObject go in components) {
            // Al iniciar, guarda todos los TextMeshProUGUI con nombre "NameText" en el array
            Debug.Log($"Start component: {go}");
            CaptureAllNameTexts(go.transform.GetChild(0).gameObject);
        }

        // Desactiva los objetos padres de los TextMeshProUGUI capturados
        DeactivateParentObjects();
    }

    void CaptureAllNameTexts(GameObject parent)
    {
        List<TextMeshProUGUI> nameTextList = new List<TextMeshProUGUI>();
        List<TextMeshProUGUI> newList = FindNameTextComponents(parent, nameTextList);

        if (newList.Count > 0)
        {
            Debug.Log($"CaptureAllNameTexts newList count: {newList.Count}");
            nameTexts.Add(newList);
        }
    }

    List<TextMeshProUGUI> FindNameTextComponents(GameObject parent, List<TextMeshProUGUI> nameTextList)
    {
        // Si el objeto está desactivado, primero lo activamos temporalmente para poder encontrar sus hijos
        bool wasActive = parent.activeSelf;
        if (!wasActive)
        {
            parent.SetActive(true);
        }

        // Buscamos todos los componentes TextMeshProUGUI en el objeto padre
        TextMeshProUGUI[] textComponents = parent.GetComponentsInChildren<TextMeshProUGUI>(true);
        Debug.Log($"FindNameTextComponents textComponents count: {textComponents.Length}");
        foreach (TextMeshProUGUI textComponent in textComponents)
        {
            if (textComponent.name == "NameText")
            {
                nameTextList.Add(textComponent);
                GameObject parentObject = textComponent.gameObject.transform.parent.gameObject;
                if (!parentObjects.Contains(parentObject))
                {
                    parentObjects.Add(parentObject);
                }
            }
        }

        // Restauramos el estado original del objeto
        if (!wasActive)
        {
            parent.SetActive(false);
        }

        return nameTextList;
    }

    void DeactivateParentObjects()
    {
        Debug.Log($"DeactivateParentObjects parentObjects count: {parentObjects.Count}");

        // Desactiva los objetos padres de los TextMeshProUGUI capturados
        foreach (GameObject parent in parentObjects)
        {
            parent.SetActive(false);
        }
    }

    public void startClick()
    {
        Debug.Log($"startClick");
        ClearAllText();
        Debug.Log($"startClick ClearAllText");
        foreach (GameObject go in components)
        {
            ActivateLastChildInHierarchy(go.transform);
        }

        startButton.SetActive(false);
        finishButton.SetActive(true);
        DeactivateLocalizeStringEvent();
        eventLogger = FindObjectOfType<EventLogger>();
        eventLogger.LogEvent("Start button pressed");
    }

    public void ClearAllText()
    {
        // Limpia los textos de todos los componentes guardados en el array
        foreach (List<TextMeshProUGUI> array in nameTexts)
        {
            foreach (TextMeshProUGUI textComponent in array)
            {
                textComponent.text = string.Empty;
            }
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
        foreach (List<TextMeshProUGUI> array in nameTexts)
        {
            foreach (TextMeshProUGUI textComponent in array)
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
}