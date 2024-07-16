using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireConnector : MonoBehaviour
{
    public float wireWidth = 0.1f;    // Ancho del cable
    public float endpointOffset = 0.001f; // Offset para la posición del cable desde el extremo

    private Transform startTransform;
    private Transform endTransform;

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
            // Evitar conexiones no permitidas
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
}
