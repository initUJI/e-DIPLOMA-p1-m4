using UnityEngine;
using Vuforia;

public class ModelTargetRotationChecker : MonoBehaviour
{
    public ModelTargetBehaviour modelTargetBehaviour;  // Referencia al ModelTargetBehaviour
    public GameObject warningObject;  // El objeto que se activará si la rotación no es adecuada
    public float allowedAngleDeviation = 45f;  // Tolerancia en la rotación (en grados) para todos los ejes

    void Update()
    {
        // Comprobar si el modelo está rastreado
        if (modelTargetBehaviour.TargetStatus.Status == Status.TRACKED || modelTargetBehaviour.TargetStatus.Status == Status.EXTENDED_TRACKED)
        {
            // Obtener la rotación del modelo en ángulos de Euler
            Vector3 targetRotation = modelTargetBehaviour.transform.eulerAngles;

            // Comprobar si la rotación en cada eje está dentro de los límites permitidos
            bool isOutOfBoundsX = Mathf.Abs(NormalizeAngle(targetRotation.x)) > allowedAngleDeviation;
            bool isOutOfBoundsY = Mathf.Abs(NormalizeAngle(targetRotation.y)) > allowedAngleDeviation;
            bool isOutOfBoundsZ = Mathf.Abs(NormalizeAngle(targetRotation.z)) > allowedAngleDeviation;

            // Si alguna de las rotaciones está fuera del rango permitido, activamos el objeto de advertencia
            if (isOutOfBoundsX || isOutOfBoundsY || isOutOfBoundsZ)
            {
                if (!warningObject.activeSelf)
                {
                    warningObject.SetActive(true);  // Activar el objeto si no está ya activado
                    Debug.Log("El modelo está fuera del rango permitido de rotación. Activando objeto de advertencia.");
                }
            }
            else
            {
                if (warningObject.activeSelf)
                {
                    warningObject.SetActive(false);  // Desactivar el objeto si está dentro del rango permitido
                    Debug.Log("El modelo está dentro del rango permitido de rotación. Desactivando objeto de advertencia.");
                }
            }
        }
        else
        {
            // Si el modelo no está rastreado, asegurarnos de que el objeto de advertencia esté desactivado
            if (warningObject.activeSelf)
            {
                warningObject.SetActive(false);
                Debug.Log("El modelo no está rastreado. Desactivando objeto de advertencia.");
            }
        }
    }

    // Función para normalizar ángulos entre -180 y 180 grados
    private float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180)
            angle -= 360;
        return angle;
    }
}
