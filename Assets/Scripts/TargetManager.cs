using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public GameObject[] targetObjects; // Lista de objetos que controlan el estado de los demás
    public GameObject[] allObjects; // Todos los objetos a verificar

    void Update()
    {
        // Verifica si alguno de los objetos de control está activo
        bool anyTargetActive = false;
        foreach (GameObject target in targetObjects)
        {
            if (target.activeInHierarchy)
            {
                anyTargetActive = true;
                break;
            }
        }

        // Activa o desactiva todos los objetos cuyo nombre contenga "Target" o "target" basado en el estado de los objetos de control
        if (anyTargetActive)
        {
            SetTargetsActive(false);
        }
        else
        {
            SetTargetsActive(true);
        }
    }

    void SetTargetsActive(bool isActive)
    {
        // Itera sobre cada objeto en la lista allObjects y verifica si su nombre contiene "Target" o "target"
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Target") || obj.name.Contains("target"))
            {
                obj.SetActive(isActive);
            }
        }
    }
}
