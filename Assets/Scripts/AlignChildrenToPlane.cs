using UnityEngine;

public class AlignChildrenToPlane : MonoBehaviour
{
    public RectTransform referenceUIObject; // Objeto UI de referencia

    void Start()
    {
        AlignChildren();
    }

    public void AlignChildren()
    {
        /*if (referenceUIObject == null)
        {
            Debug.LogError("Reference UI Object is not assigned.");
            return;
        }

        // Obtener la direcci�n hacia adelante del objeto de referencia
        Vector3 forwardDirection = referenceUIObject.forward;

        foreach (Transform child in transform)
        {
            if (child.GetComponent<RectTransform>() != null)
            {
                // Obtener la posici�n mundial actual del hijo
                Vector3 worldPosition = child.position;

                // Ajustar la posici�n en la direcci�n "hacia adelante" con un desplazamiento de -0.003
                worldPosition -= forwardDirection * 0.003f;

                // Asignar la nueva posici�n mundial al hijo
                child.position = worldPosition;

                // Asignar la misma rotaci�n que el objeto de referencia
                child.rotation = referenceUIObject.rotation;
            }
        }*/
    }
}
