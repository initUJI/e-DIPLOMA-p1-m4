using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSwitcher : MonoBehaviour
{
    public Sprite wiringModeSprite; // La imagen del botón para el modo de cableado
    public Sprite studyModeSprite;  // La imagen del botón para el modo de estudio
    public GameObject manager;      // El objeto manager que tiene el script WireConnector
    public Button modeButton;       // El botón que se usará para cambiar de modo
    public Image childImage;        // La imagen hija del botón que se debe cambiar

    private bool isWiringMode = false; // Variable para controlar el modo actual
    private WireConnector wireConnector; // Referencia al script WireConnector

    private List<GameObject> interactableObjects = new List<GameObject>(); // Lista para almacenar objetos interactuables
    private List<GameObject> wireObjects = new List<GameObject>(); // Lista para almacenar objetos wire
    private List<GameObject> UIObjects = new List<GameObject>(); // Lista para almacenar objetos UI

    void Start()
    {
        wireConnector = manager.GetComponent<WireConnector>();
        // Configurar el botón para llamar a SwitchMode cuando se haga clic
        modeButton.onClick.AddListener(SwitchMode);

        // Inicializar listas con todos los objetos necesarios
        InitializeObjectLists();

        // Configurar el modo inicial (estudio)
        SetInitialMode();

        CleanupWiring();
    }

    void DeactivateWireChildren()
    {
        foreach (GameObject wireObject in wireObjects)
        {
            foreach (Transform child in wireObject.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    void InitializeObjectLists()
    {
        // Inicializar la lista de objetos interactuables
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
        foreach (GameObject obj in interactables)
        {
            RegisterInteractable(obj);
        }

        // Inicializar la lista de objetos wire
        GameObject[] wires = GameObject.FindGameObjectsWithTag("Wire");
        foreach (GameObject obj in wires)
        {
            RegisterWire(obj);
        }
    }

    void SetInitialMode()
    {
        Debug.Log("Setting initial mode to Study");

        // Modo de estudio (isWiringMode = false)
        isWiringMode = false;

        // Actualizar los estados de los objetos según el modo actual
        UpdateObjectStates();

        // Desactivar el script WireConnector en el manager
        wireConnector.enabled = false;
        wireConnector.cleanTransforms();
        DeactivateWireChildren();

        // Eliminar instancias creadas del extremo del cable y los LineRenderer
        CleanupWiring();

        // Actualizar la imagen hija del botón
        UpdateChildImage();
    }

    void SwitchMode()
    {
        isWiringMode = !isWiringMode; // Cambiar el modo
        Debug.Log("Switching mode to " + (isWiringMode ? "Wiring" : "Study"));

        // Actualizar los estados de los objetos según el modo actual
        UpdateObjectStates();

        // Activar/desactivar el script WireConnector en el manager
        wireConnector.enabled = isWiringMode;

        if (!isWiringMode)
        {
            wireConnector.cleanTransforms();
            DeactivateWireChildren();
        }

        // Si cambiamos al modo de estudio, limpiar los elementos de cableado
        CleanupWiring();
        deleteCableEndpoints();

        // Actualizar la imagen hija del botón
        Debug.Log("Ready to update image");
        UpdateChildImage();
    }

    void UpdateObjectStates()
    {
        Debug.Log("Updating object states for mode: " + (isWiringMode ? "Wiring" : "Study"));

        // Activar/desactivar objetos interactivos según el modo
        foreach (GameObject obj in interactableObjects)
        {
            if (obj != null) // Verificar que el objeto no sea nulo
            {
                Debug.Log("Setting Interactable object " + obj.name + " to " + (!isWiringMode));
                obj.SetActive(!isWiringMode);
            }
        }

        // Activar/desactivar objetos wire según el modo
        foreach (GameObject obj in wireObjects)
        {
            if (obj != null) // Verificar que el objeto no sea nulo
            {
                Debug.Log("Setting Wire object " + obj.name + " to " + isWiringMode);
                obj.SetActive(isWiringMode);
            }
        }

        // Activar/desactivar objetos UI según el modo
        foreach (GameObject obj in UIObjects)
        {
            if (obj != null) // Verificar que el objeto no sea nulo
            {
                Debug.Log("Setting UI object " + obj.name + " to " + isWiringMode);
                obj.SetActive(!isWiringMode);
            }
        }
    }

    void deleteCableEndpoints()
    {
        // Eliminar todos los objetos de extremo de cable
        GameObject[] cableEndpoints = GameObject.FindGameObjectsWithTag("CableEndpoint");
        foreach (GameObject endpoint in cableEndpoints)
        {
            Debug.Log("Destroying CableEndpoint " + endpoint.name);
            Destroy(endpoint);
        }
    }

    void CleanupWiring()
    {
        Debug.Log("Cleaning up wiring");

        // Eliminar todos los objetos de extremo de cable
        GameObject[] cableEndpoints = GameObject.FindGameObjectsWithTag("CableEndpoint");
        foreach (GameObject endpoint in cableEndpoints)
        {
            Debug.Log("Destroying CableEndpoint " + endpoint.name);
            Destroy(endpoint);
        }

        // Eliminar todos los LineRenderers
        LineRenderer[] lineRenderers = FindObjectsOfType<LineRenderer>();
        foreach (LineRenderer lineRenderer in lineRenderers)
        {
            Debug.Log("Destroying LineRenderer " + lineRenderer.name);
            Destroy(lineRenderer.gameObject);
        }
    }

    void UpdateChildImage()
    {
        Debug.Log("Updating button image for mode: " + (isWiringMode ? "Wiring" : "Study"));

        if (isWiringMode)
        {
            childImage.sprite = studyModeSprite;
        }
        else
        {
            childImage.sprite = wiringModeSprite;
        }
    }

    public void RegisterInteractable(GameObject obj)
    {
        if (!interactableObjects.Contains(obj))
        {
            interactableObjects.Add(obj);
            Debug.Log("Registered interactable object: " + obj.name);
        }
    }

    public void RegisterWire(GameObject obj)
    {
        if (!wireObjects.Contains(obj))
        {
            wireObjects.Add(obj);
            Debug.Log("Registered wire object: " + obj.name);
        }
    }

    public void RegisterUI(GameObject obj)
    {
        if (!UIObjects.Contains(obj))
        {
            UIObjects.Add(obj);
            Debug.Log("Registered UI object: " + obj.name);
        }
    }

    public void UnregisterInteractable(GameObject obj)
    {
        if (interactableObjects.Contains(obj))
        {
            interactableObjects.Remove(obj);
            Debug.Log("Unregistered interactable object: " + obj.name);
        }
    }

    public void UnregisterWire(GameObject obj)
    {
        if (wireObjects.Contains(obj))
        {
            wireObjects.Remove(obj);
            Debug.Log("Unregistered wire object: " + obj.name);
        }
    }
}
