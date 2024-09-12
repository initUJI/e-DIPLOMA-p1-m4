using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Necesario para usar los botones de UI

public class ObjectSwitcher : MonoBehaviour
{
    // Lista pública de objetos que quieres alternar
    public List<GameObject> objectsToSwitch;

    // Botones de avance y retroceso asignados desde el Inspector
    public Button nextButton;
    public Button previousButton;

    // Índice para mantener la referencia al objeto actual visible
    private int currentIndex = 0;

    void Start()
    {
        // Asignar las funciones a los botones desde el Inspector
        nextButton.onClick.AddListener(NextObject);
        previousButton.onClick.AddListener(PreviousObject);

        // Asegúrate de que solo el primer objeto esté activo al inicio
        UpdateActiveObject();
    }

    // Función para avanzar al siguiente objeto
    public void NextObject()
    {
        // Incrementamos el índice y lo "envolvemos" si se pasa del final
        currentIndex++;
        if (currentIndex >= objectsToSwitch.Count)
        {
            currentIndex = 0;
        }
        UpdateActiveObject();
    }

    // Función para retroceder al objeto anterior
    public void PreviousObject()
    {
        // Decrementamos el índice y lo "envolvemos" si es menor que 0
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = objectsToSwitch.Count - 1;
        }
        UpdateActiveObject();
    }

    // Actualizamos el objeto activo en función del índice actual
    private void UpdateActiveObject()
    {
        // Desactiva todos los objetos primero
        for (int i = 0; i < objectsToSwitch.Count; i++)
        {
            objectsToSwitch[i].SetActive(false);
        }

        // Activa solo el objeto en la posición actual
        if (objectsToSwitch.Count > 0)
        {
            objectsToSwitch[currentIndex].SetActive(true);
        }
    }
}
