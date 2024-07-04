using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Collections;
using Vuforia;
using System.Text.RegularExpressions;

public class UICollisionDetector : MonoBehaviour
{
    public string tagToCheck = "OptionName";  // La etiqueta que deseas comprobar en otros elementos
    public Manager manager;
    private string correctText;
    public InfoProcessor infoProcessor;

    private EventLogger logger;

    [HideInInspector]
    public bool solved;

    private void Start()
    {
        logger = FindFirstObjectByType<EventLogger>();
        solved = false;
        correctText = GetComponent<TextMeshProUGUI>().text;
        manager = FindFirstObjectByType<Manager>();
        Initialize();
    }

    private void Update()
    {
        DetectCollisions();
    }

    private void Initialize()
    {
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
        Debug.Log($"correct text: {correctText}");
        Debug.Log($"actual text: {GetComponent<TextMeshProUGUI>().text}");

        solved = ProcessString(GetComponent<TextMeshProUGUI>().text) == ProcessString(correctText);
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

        if (GetComponent<TextMeshProUGUI>().text != string.Empty)
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

        logger.LogEvent("Tag " + sourceTextComponent.text + "placed in " + correctText + "space");

        targetTextComponent.text = sourceTextComponent.text;
        targetTextComponent.fontSize = sourceTextComponent.fontSize;

        CheckSolvedStatus();
        infoProcessor.UpdateSolvedCount();
        infoProcessor.UpdateStatusText();
        infoProcessor.CheckAndDeactivateInfoObjects();
    }

    public static string ProcessString(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        // Convertir a minúsculas
        string lowerCaseString = input.ToLower();

        // Eliminar espacios y puntuación
        string result = Regex.Replace(lowerCaseString, @"[\s\p{P}]", "");

        return result;
    }
}



