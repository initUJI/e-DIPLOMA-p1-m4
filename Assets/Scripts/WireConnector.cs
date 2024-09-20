using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WireConnector : MonoBehaviour
{
    public float wireWidth = 0.1f;    // Ancho del cable
    public float endpointOffset = 0.001f; // Offset para la posici�n del cable desde el extremo
    public TextMeshProUGUI connectionStatusText; // Texto que se activar� al detectar la conexi�n correcta
    public TextMeshProUGUI incorrectConnectionText; // Texto que se activar� al detectar una conexi�n incorrecta
    public GameObject ultrasonicCanvas; // Canvas para el Ultrasonic Sensor
    public GameObject dht11Canvas; // Canvas para el DHT11 Sensor

    private Transform startTransform;
    private Transform endTransform;
    private bool ultrasonicConnected = false;
    private bool dht11Connected = false;
    private bool ultrasonicCorrect = false;
    private bool dht11Correct = false;
    private EventLogger logger;

    void Start()
    {
        // Asegurarse de que los Canvas y textos comiencen desactivados
        ultrasonicCanvas.SetActive(false);
        dht11Canvas.SetActive(false);
        connectionStatusText.gameObject.SetActive(false);
        incorrectConnectionText.gameObject.SetActive(false); // Desactivar el texto de conexi�n incorrecta
        logger = FindFirstObjectByType<EventLogger>();
    }

    void Update()
    {
        // Detectar toques en la pantalla
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                DetectTouch(touch.position);
            }
        }
        // Detectar clics del rat�n (para pruebas en el editor)
        else if (Input.GetMouseButtonDown(0))
        {
            DetectTouch(Input.mousePosition);
        }
    }

    public void cleanTransforms()
    {
        startTransform = null;
        endTransform = null;
    }

    void DetectTouch(Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Wire"))
            {
                AssignTransform(hit.transform);
            }
        }
    }

    void AssignTransform(Transform selectedTransform)
    {
        // Verificar los nombres de los objetos
        string selectedName = selectedTransform.parent.parent.name; // Subir dos niveles para obtener el nombre del sensor
        string startName = startTransform != null ? startTransform.parent.parent.name : "";

        if (startTransform == null)
        {
            if (selectedName.Contains("BaseShield") || selectedName.Contains("DHT11") || selectedName.Contains("Ultrasonic"))
            {
                startTransform = selectedTransform;
                // Activar la malla inicial del punto inicial
                SetChildrenActive(startTransform, true);
            }
        }
        else if (endTransform == null && selectedTransform != startTransform)
        {
            // Verificar si la conexi�n es permitida
            if (!((startName.Contains("DHT11") && selectedName.Contains("Ultrasonic")) ||
                  (startName.Contains("Ultrasonic") && selectedName.Contains("DHT11")) ||
                  (startName.Contains("BaseShield") && selectedName.Contains("BaseShield"))))
            {
                endTransform = selectedTransform;
                // Activar la malla inicial del punto final
                SetChildrenActive(endTransform, true);

                // Activar el cable correspondiente basado en los objetos seleccionados
                ActivateCable(startTransform, endTransform);

                // Reset para poder asignar nuevos extremos
                startTransform = null;
                endTransform = null;
            }
            else
            {
                // Si la conexi�n no es permitida, reemplazar el startTransform con el nuevo
                SetChildrenActive(startTransform, false); // Desactivar la malla inicial del punto anterior
                startTransform = selectedTransform;
                SetChildrenActive(startTransform, true); // Activar la nueva malla inicial
            }
        }
        else if (startTransform != null && endTransform != null)
        {
            startTransform = null;
            endTransform = null;

            startTransform = selectedTransform;
        }
    }

    void SetChildrenActive(Transform parent, bool active)
    {
        foreach (Transform child in parent)
        {
            if (child.name.Contains("Plane")) // Activar siempre las mallas
            {
                child.gameObject.SetActive(active);
            }
        }
    }

    void SetAllChildrenActive(Transform parent, bool active)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(active);
        }
    }

    void ActivateCable(Transform start, Transform end)
    {
        string startName = start.parent.parent.name; // Subir dos niveles para obtener el nombre del sensor
        string endName = end.parent.parent.name;

        Transform startColliders = start; // El collider es el padre de los cables y las mallas
        Transform endColliders = end;

        // Activar los cables y mallas en el objeto de inicio
        ActivateCablesForObject(startColliders, startName, endName);

        // Activar los cables y mallas en el objeto de final
        ActivateCablesForObject(endColliders, endName, startName);

        // Verificar conexiones espec�ficas
        CheckConnections(start, end);
    }

    void ActivateCablesForObject(Transform collidersParent, string parentName, string otherParentName)
    {
        if (collidersParent != null)
        {
            foreach (Transform child in collidersParent)
            {
                if (child.name.Contains("Plane")) // Activar siempre las mallas
                {
                    child.gameObject.SetActive(true);
                }
                else if (child.name.Contains("UltrasonicWire") || child.name.Contains("DHT11Wire"))
                {
                    if (parentName.Contains("BaseShield"))
                    {
                        if (otherParentName.Contains("Ultrasonic") && child.name.Contains("UltrasonicWire"))
                        {
                            child.gameObject.SetActive(true);
                        }
                        else if (otherParentName.Contains("DHT11") && child.name.Contains("DHT11Wire"))
                        {
                            child.gameObject.SetActive(true);
                        }
                        else
                        {
                            child.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        child.gameObject.SetActive(true);
                    }
                }
            }
        }
    }

    public void DeactivateAllActivePlanesAndWires()
    {
        GameObject[] cableObjects = GameObject.FindGameObjectsWithTag("Wire");

        foreach (GameObject cable in cableObjects)
        {
            SetAllChildrenActive(cable.transform, false);
        }

        startTransform = null;
        endTransform = null;
        ultrasonicConnected = false;
        dht11Connected = false;
        ultrasonicCorrect = false;
        dht11Correct = false;
        connectionStatusText.gameObject.SetActive(false);
        incorrectConnectionText.gameObject.SetActive(false); // Desactivar el texto de conexi�n incorrecta
        ultrasonicCanvas.SetActive(false); // Desactivar el canvas de Ultrasonic
        dht11Canvas.SetActive(false); // Desactivar el canvas de DHT11
    }

    void CheckConnections(Transform start, Transform end)
    {
        // Verificar la conexi�n correcta de Ultrasonic
        ultrasonicCorrect = (start.parent.parent.name.Contains("Ultrasonic") && end.name.Contains("Digital")) ||
                            (end.parent.parent.name.Contains("Ultrasonic") && start.name.Contains("Digital"));
        if (ultrasonicCorrect)
        {
            ultrasonicConnected = true;
            ultrasonicCanvas.SetActive(true); // Activar el canvas de Ultrasonic
            logger.LogEvent("Ultrasonic good connected");
        }

        // Verificar la conexi�n correcta de DHT11
        dht11Correct = (start.parent.parent.name.Contains("DHT11") && end.name.Contains("Digital")) ||
                       (end.parent.parent.name.Contains("DHT11") && start.name.Contains("Digital"));
        if (dht11Correct)
        {
            dht11Connected = true;
            dht11Canvas.SetActive(true); // Activar el canvas de DHT11
            logger.LogEvent("DHT11 good connected");
        }

        // Si ambos est�n conectados correctamente, mostrar el texto de conexi�n correcta
        if (ultrasonicConnected && dht11Connected && ultrasonicCorrect && dht11Correct)
        {
            connectionStatusText.gameObject.SetActive(true);
            incorrectConnectionText.gameObject.SetActive(false); // Ocultar el texto de conexi�n incorrecta
        }
        else if (ultrasonicConnected && dht11Connected && (!ultrasonicCorrect || !dht11Correct))
        {
            // Si ambos est�n conectados pero al menos uno est� mal conectado
            incorrectConnectionText.gameObject.SetActive(true);
            connectionStatusText.gameObject.SetActive(false); // Ocultar el texto de conexi�n correcta
        }
        else
        {
            // Si alguno se desconecta, desactivar ambos textos
            connectionStatusText.gameObject.SetActive(false);
            incorrectConnectionText.gameObject.SetActive(false);
        }
    }
}
