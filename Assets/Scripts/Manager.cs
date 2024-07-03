using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public GameObject optionPrefab;
    public float moveStep = 0.01f;
    public float checkDelay = 0.1f;

    public void CreateOptionName(TextMeshProUGUI text, Transform initialPos)
    {
        GameObject optionInstance = InstantiateOption(initialPos);
        SetOptionText(optionInstance, text);
        StartCoroutine(MoveUpUntilNoCollision(optionInstance));
    }

    private GameObject InstantiateOption(Transform initialPos)
    {
        // Instanciar el prefab en la posición inicial
        return Instantiate(optionPrefab, initialPos.position, initialPos.rotation);
    }

    private void SetOptionText(GameObject optionInstance, TextMeshProUGUI text)
    {
        // Establecer el texto en el componente TextMeshProUGUI
        optionInstance.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().fontSize = text.fontSize;
        optionInstance.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = text.text;
    }

    private IEnumerator MoveUpUntilNoCollision(GameObject optionInstance)
    {
        Collider optionCollider = GetOptionCollider(optionInstance);
        if (optionCollider == null)
        {
            yield break;
        }

        float initialYPosition = optionInstance.transform.position.y;
        float maxHeight = 0.5f;

        while (IsColliding(optionCollider, optionInstance.transform.rotation) && !HasReachedMaxHeight(optionInstance.transform.position.y, initialYPosition, maxHeight))
        {
            MoveOptionUp(optionInstance);
            yield return new WaitForSeconds(checkDelay);
        }
    }

    private Collider GetOptionCollider(GameObject optionInstance)
    {
        Collider collider = optionInstance.GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("El prefab no tiene un Collider.");
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
                Debug.Log($"Colisión detectada con: {hitCollider.gameObject.name}");
                return true;
            }
        }
        return false;
    }

    private bool HasReachedMaxHeight(float currentY, float initialY, float maxHeight)
    {
        if (currentY - initialY >= maxHeight)
        {
            Debug.LogWarning("Se ha alcanzado la altura máxima permitida.");
            return true;
        }
        return false;
    }

    private void MoveOptionUp(GameObject optionInstance)
    {
        optionInstance.transform.position += new Vector3(0, moveStep, 0);
    }
}

