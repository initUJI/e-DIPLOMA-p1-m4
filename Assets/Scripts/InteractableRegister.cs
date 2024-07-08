using UnityEngine;

public class InteractableRegister : MonoBehaviour
{
    private ModeSwitcher modeSwitcher;

    void Awake()
    {
        // Encontrar el objeto ModeSwitcher en la escena
        modeSwitcher = FindObjectOfType<ModeSwitcher>();
        if (modeSwitcher == null)
        {
            Debug.LogError("ModeSwitcher no encontrado en la escena.");
        }
    }

    void OnEnable()
    {
        if (modeSwitcher != null)
        {
            modeSwitcher.RegisterInteractable(gameObject);
        }
    }

    /*void OnDisable()
    {
        if (modeSwitcher != null)
        {
            modeSwitcher.UnregisterInteractable(gameObject);
        }
    }*/
}
