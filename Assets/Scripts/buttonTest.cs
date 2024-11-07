using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;

public class buttonTest : MonoBehaviour
{
    private EventLogger eventLogger;
    private List<List<TextMeshProUGUI>> nameTexts = new List<List<TextMeshProUGUI>>();
    private List<GameObject> parentObjects = new List<GameObject>();

    [SerializeField]
    private List<GameObject> modelTargets = new List<GameObject>();

    [SerializeField]
    private List<GameObject> components = new List<GameObject>();
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private GameObject finishButton;

    [SerializeField]
    private GameObject optionsprefabArduino;
    [SerializeField]
    private GameObject optionsprefabUltrasonic;
    [SerializeField]
    private GameObject optionsprefabDHT11;
    [SerializeField]
    private GameObject optionsprefabShield;

    private bool start;
    private GameObject objectDetecting;

    void Start()
    {
        foreach (GameObject go in components) {
            // Al iniciar, guarda todos los TextMeshProUGUI con nombre "NameText" en el array
           // Debug.Log($"Start component: {go}");
            CaptureAllNameTexts(go.transform.GetChild(0).gameObject);
        }

        // Desactiva los objetos padres de los TextMeshProUGUI capturados
        DeactivateParentObjects();
        start = false;
    }

    void CaptureAllNameTexts(GameObject parent)
    {
        List<TextMeshProUGUI> nameTextList = new List<TextMeshProUGUI>();
        List<TextMeshProUGUI> newList = FindNameTextComponents(parent, nameTextList);

        if (newList.Count > 0)
        {
            //Debug.Log($"CaptureAllNameTexts newList count: {newList.Count}");
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
        //Debug.Log($"FindNameTextComponents textComponents count: {textComponents.Length}");
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
        //Debug.Log($"DeactivateParentObjects parentObjects count: {parentObjects.Count}");

        // Desactiva los objetos padres de los TextMeshProUGUI capturados
        foreach (GameObject parent in parentObjects)
        {
            parent.SetActive(false);
        }
    }

    public void startClick()
    {
        ClearAllText();
        // Debug.Log($"startClick ClearAllText");

        startButton.SetActive(false);
        finishButton.SetActive(true);
        DeactivateLocalizeStringEvent();
        eventLogger = FindObjectOfType<EventLogger>();
        eventLogger.LogEvent("Start button pressed");
        Canvas.ForceUpdateCanvases();

        start = true;
        if (objectDetecting != null)
        {
            colocateOptions(objectDetecting.transform);
        }
    }

    public void colocateOptions(Transform parent)
    {
        objectDetecting = parent.gameObject;
        if (start)
        {
            GameObject options = null;
            Vector3 eulerAngles = Vector3.zero;
            if (parent.name.Contains("Arduino"))
            {
                options = optionsprefabArduino;
            }
            else if (parent.name.Contains("DHT11"))
            {
                options = optionsprefabDHT11;
                eulerAngles = new Vector3(-90, 0, 0);
            }
            else if (parent.name.Contains("Ultrasonic"))
            {
                options = optionsprefabUltrasonic;
            }
            else if (parent.name.Contains("Shield"))
            {
                options = optionsprefabShield;
                eulerAngles = new Vector3(-90, 0, 0);
            }

            options.transform.parent = parent;
            options.transform.position = parent.transform.position;
            options.transform.localEulerAngles = eulerAngles;
        }
    }

    public void removeOptions(Transform parent)
    {
        objectDetecting = null;
        if (start)
        {
            GameObject options = null;
            if (parent.name.Contains("Arduino"))
            {
                options = optionsprefabArduino;
            }
            else if (parent.name.Contains("DHT11"))
            {
                options = optionsprefabDHT11;
            }
            else if (parent.name.Contains("Ultrasonic"))
            {
                options = optionsprefabUltrasonic;
            }
            else if (parent.name.Contains("Shield"))
            {
                options = optionsprefabShield;
            }

            options.transform.localPosition = new Vector3(4f, 0f, 0.06f);
        }
    }

    public void desactiveModelTargets()
    {
        foreach (GameObject m in modelTargets)
        {
            m.SetActive(false);
        }
        foreach (GameObject c in components)
        {
            Destroy(c);
        }
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
                    //Debug.LogWarning($"No LocalizeStringEvent component found on {textComponent.gameObject.name}.");
                }
            }
        }
    }
}