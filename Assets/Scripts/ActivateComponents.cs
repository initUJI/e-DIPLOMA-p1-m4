using UnityEngine;
using UnityEngine.UI;

public class ActivateComponents : MonoBehaviour
{
    void OnEnable()
    {
        // Llamado cuando el GameObject es activado
        ActivateAllComponents();
    }

    void ActivateAllComponents()
    {
        // Obtener todos los componentes del GameObject actual
        Component[] components = GetComponents<Component>();

        foreach (Component component in components)
        {
            if (component is Behaviour)
            {
                // Activar el componente si es un Behaviour (como MonoBehaviour, Renderer, etc.)
                ((Behaviour)component).enabled = true;
                Debug.Log($"{component.GetType().Name} activado en {gameObject.name}");
            }
            else if (component is Renderer)
            {
                // Activar el Renderer si est� desactivado
                ((Renderer)component).enabled = true;
                Debug.Log($"Renderer activado en {gameObject.name}");
            }
            else if (component is Canvas)
            {
                // Activar el Canvas si est� desactivado
                ((Canvas)component).enabled = true;
                Debug.Log($"Canvas activado en {gameObject.name}");
            }
            else if (component is Collider)
            {
                // Activar el Collider si est� desactivado
                ((Collider)component).enabled = true;
                Debug.Log($"Collider activado en {gameObject.name}");
            }
            else if (component is Collider2D)
            {
                // Activar el Collider2D si est� desactivado
                ((Collider2D)component).enabled = true;
                Debug.Log($"Collider2D activado en {gameObject.name}");
            }
            // Agregar m�s condiciones si es necesario para otros tipos de componentes.
        }
    }
}
