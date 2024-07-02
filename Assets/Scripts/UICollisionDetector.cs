using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICollisionDetector : MonoBehaviour
{
    public string tagToCheck = "OptionName";  // La etiqueta que deseas comprobar en otros elementos

    private string correctText;

    [HideInInspector]
    public bool solved;

    private void Start()
    {
        solved = false;
        correctText = GetComponent<TextMeshProUGUI>().text;

        // Asegúrate de que el objeto tiene un BoxCollider 3D
        if (GetComponent<BoxCollider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }

    private void Update()
    {
        if (GetComponent<TextMeshProUGUI>().text == correctText)
        {
            solved = true;
        }
        else
        {
            solved = false;
        }

        // Buscar todos los objetos con la etiqueta especificada
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tagToCheck);

        // Filtrar objetos que sean hijos del último hijo del objeto raíz
        Transform root = transform.root;
        Transform lastChildOfRoot = root.GetChild(root.childCount - 1);
        List<GameObject> filteredObjects = new List<GameObject>();

        foreach (GameObject obj in objectsWithTag)
        {
            if (obj.transform.parent == lastChildOfRoot)
            {
                filteredObjects.Add(obj);
            }
        }

        foreach (GameObject obj in filteredObjects)
        {
            BoxCollider otherCollider = obj.GetComponent<BoxCollider>();
            if (otherCollider != null && GetComponent<BoxCollider>().bounds.Intersects(otherCollider.bounds))
            {
                Debug.Log("El elemento UI objetivo está colisionando con " + obj.name);

                // Transferir el texto y tamaño de fuente del objeto colisionado
                if (GetComponent<TextMeshProUGUI>().text != "")
                {
                    //manager.CreateOptionName(GetComponent<TextMeshProUGUI>().text, transform);
                }

                GetComponent<TextMeshProUGUI>().text = obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
                GetComponent<TextMeshProUGUI>().fontSize = obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>().fontSize;

                // Destruir el objeto colisionado
                Destroy(obj);
            }
        }
    }
}


