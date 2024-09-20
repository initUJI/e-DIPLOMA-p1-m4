using UnityEngine;
using Vuforia;

public class ModelTargetRotationChecker : MonoBehaviour
{
    public ModelTargetBehaviour modelTargetBehaviour;  // Referencia al ModelTargetBehaviour
    public GameObject warningObject;  // El objeto que se activar� si la rotaci�n no es adecuada
    public float allowedAngleDeviation = 45f;  // Tolerancia en la rotaci�n (en grados) para todos los ejes

    void Update()
    {
        // Comprobar si el modelo est� rastreado
        if (modelTargetBehaviour.TargetStatus.Status == Status.TRACKED || modelTargetBehaviour.TargetStatus.Status == Status.EXTENDED_TRACKED)
        {
            // Obtener la rotaci�n del modelo en �ngulos de Euler
            Vector3 targetRotation = modelTargetBehaviour.transform.eulerAngles;

            // Comprobar si la rotaci�n en cada eje est� dentro de los l�mites permitidos
            bool isOutOfBoundsX = Mathf.Abs(NormalizeAngle(targetRotation.x)) > allowedAngleDeviation;
            bool isOutOfBoundsY = Mathf.Abs(NormalizeAngle(targetRotation.y)) > allowedAngleDeviation;
            bool isOutOfBoundsZ = Mathf.Abs(NormalizeAngle(targetRotation.z)) > allowedAngleDeviation;

            // Si alguna de las rotaciones est� fuera del rango permitido, activamos el objeto de advertencia
            if (isOutOfBoundsX || isOutOfBoundsY || isOutOfBoundsZ)
            {
                if (!warningObject.activeSelf)
                {
                    warningObject.SetActive(true);  // Activar el objeto si no est� ya activado
                    Debug.Log("El modelo est� fuera del rango permitido de rotaci�n. Activando objeto de advertencia.");
                }
            }
            else
            {
                if (warningObject.activeSelf)
                {
                    warningObject.SetActive(false);  // Desactivar el objeto si est� dentro del rango permitido
                    Debug.Log("El modelo est� dentro del rango permitido de rotaci�n. Desactivando objeto de advertencia.");
                }
            }
        }
        else
        {
            // Si el modelo no est� rastreado, asegurarnos de que el objeto de advertencia est� desactivado
            if (warningObject.activeSelf)
            {
                warningObject.SetActive(false);
                Debug.Log("El modelo no est� rastreado. Desactivando objeto de advertencia.");
            }
        }
    }

    // Funci�n para normalizar �ngulos entre -180 y 180 grados
    private float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180)
            angle -= 360;
        return angle;
    }
}
