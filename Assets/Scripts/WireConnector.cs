using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireConnector : MonoBehaviour
{
    public float wireWidth = 0.1f;    // Ancho del cable
    public float endpointOffset = 0.2f; // Offset para la posición del cable desde el extremo

    private Transform startTransform;
    private Transform endTransform;
    private GameObject wireObject;
    private List<LineRenderer> wires = new List<LineRenderer>();

    private Color[] wireColors = { Color.black, Color.red, Color.white, Color.yellow };

    void Start()
    {
        // Crear un objeto contenedor para los cables
        wireObject = new GameObject("WireObject");

        // Crear los LineRenderers para los cuatro cables
        for (int i = 0; i < 4; i++)
        {
            LineRenderer lineRenderer = wireObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = wireColors[i];
            lineRenderer.endColor = wireColors[i];
            lineRenderer.startWidth = wireWidth;
            lineRenderer.endWidth = wireWidth;
            lineRenderer.positionCount = 2;
            wires.Add(lineRenderer);
        }
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
        // Detectar clics del ratón (para pruebas en el editor)
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
                Debug.Log("wire detected");
                AssignTransform(hit.transform);
            }
        }
    }

    void AssignTransform(Transform selectedTransform)
    {
        if (startTransform == null)
        {
            startTransform = selectedTransform;
            // Activar todos los hijos del objeto con el tag Wire en el punto inicial
            SetChildrenActive(startTransform, true);
        }
        else if (endTransform == null && selectedTransform != startTransform)
        {
            endTransform = selectedTransform;
            // Activar todos los hijos del objeto con el tag Wire en el punto final
            SetChildrenActive(endTransform, true);

            // Dibujar el cable entre los dos puntos
            DrawWire(startTransform, endTransform);

            // Reset para poder asignar nuevos extremos
            startTransform = null;
            endTransform = null;
        }
    }

    void SetChildrenActive(Transform parent, bool active)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(active);
        }
    }

    void DrawWire(Transform start, Transform end)
    {
        Vector3 startPoint = start.position - start.forward * endpointOffset;
        Vector3 endPoint = end.position - end.forward * endpointOffset;

        Vector3 direction = (endPoint - startPoint).normalized;
        Vector3 perpendicular = Vector3.Cross(direction, Vector3.up).normalized * wireWidth * 2;

        for (int i = 0; i < 4; i++)
        {
            Vector3 offset = perpendicular * (i - 1.5f);
            wires[i].SetPosition(0, startPoint + offset);
            wires[i].SetPosition(1, endPoint + offset);
        }
    }
}
