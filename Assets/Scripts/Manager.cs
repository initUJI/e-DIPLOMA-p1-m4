using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public GameObject optionPrefab;
    public float moveStep = 0.005f;
    public float checkDelay = 0.001f;
    private EventLogger logger;

    public TextMeshProUGUI textMeshProUGUIID;
    public TMP_InputField tMP_InputFieldID;

    private GameObject currentlyHighlighted;  // Para guardar el objeto actualmente con brillo verde
    private GameObject redHighlighted;  // Para guardar el objeto actualmente con brillo rojo

    // M�todo para establecer la orientaci�n de la pantalla a retrato
    public void SetPortraitOrientation()
    {
        // Establece la orientaci�n de la pantalla a Portrait
        Screen.orientation = ScreenOrientation.Portrait;
    }
    public GameObject GetCurrentlyHighlighted()
    {
        return currentlyHighlighted;
    }

    public void SetCurrentlyHighlighted(GameObject gameObject)
    {
        currentlyHighlighted = gameObject;
    }

    // Funci�n para desiluminar todos los objetos con brillo activo, excepto los transl�cidos
    public void UnhighlightAll()
    {
        // Buscar todos los objetos con el componente ImageOutline
        ImageOutline[] allOutlines = FindObjectsOfType<ImageOutline>();

        // Desactivar el brillo en cada uno de ellos, excepto en los transl�cidos
        foreach (ImageOutline outline in allOutlines)
        {
            if (!outline.CheckIfTranslucent()) // Solo desiluminar si no es transl�cido
            {
                outline.RemoveGlow();  // Quitar brillo del objeto
            }
        }

        // Limpiar las referencias de los objetos resaltados
        currentlyHighlighted = null;
        redHighlighted = null;
    }

    public ImageOutline FindImageOutlineByText(string text)
    {
        ImageOutline[] allOutlines = FindObjectsOfType<ImageOutline>();
        foreach (ImageOutline outline in allOutlines)
        {
            TextMeshProUGUI tmp = outline.GetComponentInChildren<TextMeshProUGUI>();
            if (tmp != null && tmp.text == text)
            {
                return outline;
            }
        }
        return null;
    }

    // Funci�n p�blica para cerrar la aplicaci�n
    public void QuitApplication()
    {
        Debug.Log("Quitting application...");

        Application.Quit();

        // Si est�s en el editor de Unity, muestra un mensaje en la consola
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void Start()
    {
        logger = FindObjectOfType<EventLogger>();
        InitializeAllInfoProcessors();
        SetPortraitOrientation();
    }

    void InitializeAllInfoProcessors()
    {
        // Encuentra todos los objetos con el tag "ModelTarget"
        GameObject[] modelTargets = GameObject.FindGameObjectsWithTag("ModelTarget");

        // Lista para almacenar todos los objetos hijos
        List<GameObject> allChildObjects = new List<GameObject>();

        // Recorre cada objeto ModelTarget y sus hijos para agregar a la lista
        foreach (GameObject modelTarget in modelTargets)
        {
            GetChildren(modelTarget, allChildObjects);
        }

        // Itera sobre cada objeto hijo y busca componentes InfoProcessor
        foreach (GameObject obj in allChildObjects)
        {
            InfoProcessor infoProcessor = obj.GetComponent<InfoProcessor>();
            if (infoProcessor != null)
            {
                // Llama a la funci�n InitializeInfoObjects pasando el primer hijo del GameObject
                if (infoProcessor.gameObject.transform.childCount > 0)
                {
                    Transform firstChild = infoProcessor.gameObject.transform.GetChild(0);
                    infoProcessor.InitializeInfoObjects(firstChild);
                }
            }
        }
    }

    void GetChildren(GameObject obj, List<GameObject> allObjects)
    {
        // A�ade el objeto actual a la lista
        allObjects.Add(obj);

        // Recorre cada hijo y llama recursivamente a esta funci�n
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            GetChildren(obj.transform.GetChild(i).gameObject, allObjects);
        }
    }

    private Collider GetOptionCollider(GameObject optionInstance)
    {
        Collider collider = optionInstance.GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("The prefab does not have a Collider.");
        }
        return collider;
    }

    private bool IsColliding(Collider optionCollider, Quaternion rotation)
    {
        Vector3 currentCenter = optionCollider.bounds.center;
        Vector3 currentExtents = optionCollider.bounds.extents;

        Collider[] hitColliders = Physics.OverlapBox(currentCenter, currentExtents, rotation);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider != optionCollider)
            {
                Debug.Log($"Collision detected with: {hitCollider.gameObject.name}");
                return true;
            }
        }
        return false;
    }

    public void ReloadCurrentScene()
    {
        logger.LogEvent("Reload scene button pressed");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void infoPanel(GameObject infoPanel)
    {
        if (infoPanel.activeInHierarchy)
        {
            infoPanel.SetActive(false);
        }
        else
        {
            infoPanel.SetActive(true);
        }

        logger.LogEvent(infoPanel.name + " button pressed. Info panel active: " + infoPanel.activeInHierarchy);
    }

    public void equalTexts()
    {
        textMeshProUGUIID.text = tMP_InputFieldID.text;
    }

    private void Update()
    {
        equalTexts();

        // Detectar clic en cualquier parte de la pantalla o un toque en pantalla
        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {
            GameObject clickedObject = null;

            // Para entrada de mouse
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    clickedObject = hit.collider.gameObject;
                }
            }

            // Para entrada t�ctil en Android o dispositivos m�viles
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    clickedObject = hit.collider.gameObject;
                }
            }

            // Verificar si el objeto pulsado no es "NameText"
            if (clickedObject != null && clickedObject.name == "NameText")
            {
                // No hacer nada, no desactivar el brillo si se puls� "NameText"
                return;
            }

            // Verificar si el objeto clicado es un ImageOutline
            ImageOutline clickedImageOutline = clickedObject != null ? clickedObject.GetComponent<ImageOutline>() : null;

            if (clickedImageOutline != null)
            {
                // Si es un ImageOutline, manejar el brillo dentro del mismo ImageOutline
                clickedImageOutline.OnPointerClick(null); // Simular el clic directamente
            }
            else
            {
                // Si no es un ImageOutline, desactivar todos los brillos
                UnhighlightAll();
            }
        }
    }
}
