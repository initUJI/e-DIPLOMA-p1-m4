using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;

public class UICollisionDetector : MonoBehaviour
{
    public string tagToCheck = "OptionName";  // La etiqueta que deseas comprobar en otros elementos
    public Manager manager;
    private string correctText;
    private InfoProcessor infoProcessor;

    [HideInInspector]
    public bool solved;

    private void Start()
    {
        Initialize();
        infoProcessor = transform.root.GetComponent<InfoProcessor>();
    }

    private void Update()
    {
        DetectCollisions();
    }

    private void Initialize()
    {
        solved = false;
        correctText = GetComponent<TextMeshProUGUI>().text;
        manager = FindFirstObjectByType<Manager>();

        // Asegúrate de que el objeto tiene un BoxCollider 3D
        EnsureBoxCollider();
    }

    private void EnsureBoxCollider()
    {
        if (GetComponent<BoxCollider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();
        }
    }

    private void CheckSolvedStatus()
    {
        solved = GetComponent<TextMeshProUGUI>().text == correctText;
    }

    private void DetectCollisions()
    {
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tagToCheck);
        List<GameObject> filteredObjects = FilterObjectsByParent(objectsWithTag);

        foreach (GameObject obj in filteredObjects)
        {
            CheckAndHandleCollision(obj);
        }
    }

    private List<GameObject> FilterObjectsByParent(GameObject[] objectsWithTag)
    {
        List<GameObject> filteredObjects = new List<GameObject>();
        Transform root = transform.root;

        foreach (GameObject obj in objectsWithTag)
        {
            if (IsChildOf(root, obj.transform))
            {
                filteredObjects.Add(obj);
            }
        }

        return filteredObjects;
    }

    private bool IsChildOf(Transform parent, Transform child)
    {
        if (child == null)
            return false;

        if (child.parent == parent)
            return true;

        return IsChildOf(parent, child.parent);
    }
    private void CheckAndHandleCollision(GameObject obj)
    {
        BoxCollider otherCollider = obj.GetComponent<BoxCollider>();
        BoxCollider thisCollider = GetComponent<BoxCollider>();

        if (otherCollider != null && thisCollider.bounds.Intersects(otherCollider.bounds))
        {
            HandleCollision(obj);
        }
    }

    private void HandleCollision(GameObject obj)
    {

        if (GetComponent<TextMeshProUGUI>().text == string.Empty || GetComponent<TextMeshProUGUI>().text == "")
        {
            manager.CreateOptionName(GetComponent<TextMeshProUGUI>(), transform);
        }

        TransferTextAndFontSize(obj);

        Destroy(obj);
    }

    private void TransferTextAndFontSize(GameObject obj)
    {
        TextMeshProUGUI targetTextComponent = GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI sourceTextComponent = obj.transform.GetChild(1).GetComponent<TextMeshProUGUI>();

        targetTextComponent.text = sourceTextComponent.text;
        targetTextComponent.fontSize = sourceTextComponent.fontSize;

        CheckSolvedStatus();
        infoProcessor.UpdateSolvedCount();
        infoProcessor.UpdateStatusText();
        infoProcessor.CheckAndDeactivateInfoObjects();
    }
}



