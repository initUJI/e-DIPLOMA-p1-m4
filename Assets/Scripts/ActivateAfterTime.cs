using UnityEngine;

public class ActivateAfterTime : MonoBehaviour
{
    public GameObject targetObject; // Objeto a desactivar
    public float delayTime = 5f;    // Tiempo en segundos antes de reactivar el objeto
    private string activationKey = "ObjectActivated"; // Clave para PlayerPrefs

    void Start()
    {
        // Verificar si ya se ha activado en esta sesión (almacenado en PlayerPrefs)
        if (PlayerPrefs.GetInt(activationKey, 0) == 0)
        {
            // Desactivarlo la primera vez que se carga la escena
            if (targetObject.activeSelf)
            {
                targetObject.SetActive(false);
                // Reactivarlo después del tiempo definido
                Invoke("ActivateObject", delayTime);
            }
        }
    }

    void ActivateObject()
    {
        // Reactivar el objeto
        targetObject.SetActive(true);

        // Guardar en PlayerPrefs que el objeto ya ha sido activado
        PlayerPrefs.SetInt(activationKey, 1);
        PlayerPrefs.Save(); // Asegurarse de que se guarde el cambio
    }

    // Eliminar la clave de PlayerPrefs al cerrar la aplicación
    void OnApplicationQuit()
    {
        // Borrar la clave de PlayerPrefs al cerrar la app para que el comportamiento se repita en la próxima sesión
        PlayerPrefs.DeleteKey(activationKey);
        PlayerPrefs.Save(); // Asegurar que el cambio se guarde antes de cerrar
    }
}
