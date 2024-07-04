using UnityEngine;
using Vuforia;

public class ObjectTouchDetector : MonoBehaviour
{
    private Camera arCamera;

    void Start()
    {
        // Encuentra la AR Camera en la escena
        arCamera = Camera.main;
        if (arCamera == null)
        {
            Debug.LogError("AR Camera not found.");
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

    void DetectTouch(Vector3 screenPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Touched object: " + hit.transform.name);
            Debug.Log("Touched point: " + hit.point);
            // Aquí puedes añadir más lógica para manejar el toque en el objeto

            if (hit.transform.childCount > 0 && hit.transform.GetChild(0).CompareTag("Info"))
            {
                GameObject info = hit.transform.GetChild(0).gameObject;
                DisableAllTaggedObjects(hit.transform.parent, "Info");

                if (info.activeInHierarchy)
                {
                    info.SetActive(false);
                }
                else
                {
                    info.SetActive(true);
                }
            }
        }
    }

    void DisableAllTaggedObjects(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            // Comprueba si el objeto tiene la etiqueta "Info"
            if (child.CompareTag(tag))
            {
                // Desactiva el objeto
                child.gameObject.SetActive(false);
            }

            // Llama recursivamente a la función para los hijos del objeto actual
            DisableAllTaggedObjects(child, tag);
        }
    }
}