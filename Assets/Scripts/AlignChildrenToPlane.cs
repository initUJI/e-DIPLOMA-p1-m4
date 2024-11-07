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

        // Obtener la dirección hacia adelante del objeto de referencia
        Vector3 forwardDirection = referenceUIObject.forward;

        foreach (Transform child in transform)
        {
            if (child.GetComponent<RectTransform>() != null)
            {
                // Obtener la posición mundial actual del hijo
                Vector3 worldPosition = child.position;

                // Ajustar la posición en la dirección "hacia adelante" con un desplazamiento de -0.003
                worldPosition -= forwardDirection * 0.003f;

                // Asignar la nueva posición mundial al hijo
                child.position = worldPosition;

                // Asignar la misma rotación que el objeto de referencia
                child.rotation = referenceUIObject.rotation;
            }
        }*/
    }
}
