using UnityEngine;

public class WireRegiste : MonoBehaviour
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
            modeSwitcher.RegisterWire(gameObject);
        }
    }

    /*void OnDisable()
    {
        if (modeSwitcher != null)
        {
            modeSwitcher.UnregisterWire(gameObject);
        }
    }*/
}
