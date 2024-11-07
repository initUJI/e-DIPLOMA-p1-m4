using System.Collections;
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

    // Método para establecer la orientación de la pantalla a retrato
    public void SetPortraitOrientation()
    {
        // Establece la orientación de la pantalla a Portrait
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

    // Función para desiluminar todos los objetos con brillo activo, excepto los translúcidos
    public void UnhighlightAll()
    {
        // Buscar todos los objetos con el componente ImageOutline
        ImageOutline[] allOutlines = FindObjectsOfType<ImageOutline>();

        // Desactivar el brillo en cada uno de ellos, excepto en los translúcidos
        foreach (ImageOutline outline in allOutlines)
        {
            if (!outline.CheckIfTranslucent()) // Solo desiluminar si no es translúcido
            {
                outline.RemoveGlow();  // Quitar brillo del objeto
            }
        }

        // Limpiar las referencias de los objetos resaltados
        currentlyHighlighted = null;
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

    // Función pública para cerrar la aplicación
    public void QuitApplication()
    {
        Application.Quit();
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
    }

    void GetChildren(GameObject obj, List<GameObject> allObjects)
    {
        // Añade el objeto actual a la lista
        allObjects.Add(obj);

        // Recorre cada hijo y llama recursivamente a esta función
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
           // Debug.LogError("The prefab does not have a Collider.");
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
                //Debug.Log($"Collision detected with: {hitCollider.gameObject.name}");
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
        if (textMeshProUGUIID != null && tMP_InputFieldID != null)
        {
            textMeshProUGUIID.text = tMP_InputFieldID.text;
        }

    }

    public void backToMenu()
    {
        SceneManager.LoadScene(0);
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

            // Para entrada táctil en Android o dispositivos móviles
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
            if (clickedObject != null)
            {

                if (clickedObject.name == "NameText")
                {
                    // No hacer nada, no desactivar el brillo si se pulsó "NameText"
                    return;
                }
            }
            ImageOutline clickedImageOutline = clickedObject != null ? clickedObject.GetComponent<ImageOutline>() : null;
            UICollisionDetector clickedUIColisionDetector = clickedObject != null ? clickedObject.GetComponent<UICollisionDetector>() : null;

            if (clickedImageOutline != null)
            {
                // Si es un ImageOutline, manejar el brillo dentro del mismo ImageOutline
                clickedImageOutline.OnPointerClick(null); // Simular el clic directamente
            }
            else if (clickedImageOutline == null && clickedUIColisionDetector == null)
            {
                StartCoroutine(WaitHalfSecond());
            }
        }
    }

    private IEnumerator WaitHalfSecond()
    {
        // Esperar medio segundo
        yield return new WaitForSeconds(0.3f);

        // Código a ejecutar después de la espera
        Debug.Log("Medio segundo ha pasado.");
        //UnhighlightAll();
    }

    public void RemoveRedGlowFromAll()
    {
        // Buscar todos los objetos con el componente ImageOutline
        ImageOutline[] allOutlines = FindObjectsOfType<ImageOutline>();

        // Iterar sobre todos los objetos con ImageOutline
        foreach (ImageOutline outline in allOutlines)
        {
            // Verificar si el objeto está resaltado en rojo
            if (outline.CheckIfHighlightedRed())
            {
                // Desiluminar el objeto
                outline.RemoveRedGlow();
            }
        }
    }

}
