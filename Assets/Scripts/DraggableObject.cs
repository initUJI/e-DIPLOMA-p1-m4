using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private bool isDragging;
    private Vector3 startTouchPosition;
    private Vector3 startObjectPosition;
    private BoxCollider boxCollider;

    void Start()
    {
        // Obtener el componente BoxCollider del objeto
        boxCollider = GetComponent<BoxCollider>();

        if (boxCollider == null)
        {
            Debug.LogError("El objeto no tiene un BoxCollider.");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (boxCollider != null)
        {
            // Verificar si el toque inicial está dentro del BoxCollider
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, Camera.main.WorldToScreenPoint(transform.position).z));
            if (boxCollider.bounds.Contains(touchPosition))
            {
                isDragging = true;
                startTouchPosition = Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
                startObjectPosition = transform.position;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging)
        {
            // Usar la posición del toque para dispositivos táctiles y ratón para el editor
            Vector3 currentTouchPosition = Input.touchCount > 0 ? (Vector3)Input.GetTouch(0).position : Input.mousePosition;
            Vector3 touchDelta = currentTouchPosition - startTouchPosition;

            // Convertir el desplazamiento en unidades del mundo usando la cámara
            Vector3 worldDelta = Camera.main.ScreenToWorldPoint(new Vector3(touchDelta.x, touchDelta.y, Camera.main.WorldToScreenPoint(startObjectPosition).z))
                                - Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.WorldToScreenPoint(startObjectPosition).z));

            Vector3 newPosition = startObjectPosition + worldDelta;
            newPosition.z = startObjectPosition.z; // Mantener la posición original en el eje Z
            transform.position = newPosition;
        }
    }
}
