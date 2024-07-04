using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private EventLogger logger;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponent<Canvas>();
        logger = FindFirstObjectByType<EventLogger>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        originalRotation = rectTransform.rotation;
        logger.LogEvent("Start draging: " + transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePos))
        {
            rectTransform.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Puedes agregar cualquier l�gica adicional aqu�, como soltar el objeto en una zona espec�fica
        logger.LogEvent("End draging: " + transform.GetChild(1).GetComponent<TextMeshProUGUI>().text);
    }

    public void SetRotation(Quaternion newRotation)
    {
        rectTransform.rotation = newRotation;
    }

    public void ResetPosition()
    {
        rectTransform.anchoredPosition = originalPosition;
        rectTransform.rotation = originalRotation;
    }

    // M�todos auxiliares para convertir direcciones en funci�n de la rotaci�n local
    public Vector3 GetForward()
    {
        return rectTransform.forward;
    }

    public Vector3 GetRight()
    {
        return rectTransform.right;
    }

    public Vector3 GetLeft()
    {
        return -rectTransform.right;
    }

    public Vector3 GetUp()
    {
        return rectTransform.up;
    }

    public Vector3 GetDown()
    {
        return -rectTransform.up;
    }
}
