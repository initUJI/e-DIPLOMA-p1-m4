using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Necesario para usar los botones de UI

public class ObjectSwitcher : MonoBehaviour
{
    // Lista p�blica de objetos que quieres alternar
    public List<GameObject> objectsToSwitch;

    // Botones de avance y retroceso asignados desde el Inspector
    public Button nextButton;
    public Button previousButton;

    // �ndice para mantener la referencia al objeto actual visible
    private int currentIndex = 0;

    void Start()
    {
        // Asignar las funciones a los botones desde el Inspector
        nextButton.onClick.AddListener(NextObject);
        previousButton.onClick.AddListener(PreviousObject);

        // Aseg�rate de que solo el primer objeto est� activo al inicio
        UpdateActiveObject();
    }

    // Funci�n para avanzar al siguiente objeto
    public void NextObject()
    {
        // Incrementamos el �ndice y lo "envolvemos" si se pasa del final
        currentIndex++;
        if (currentIndex >= objectsToSwitch.Count)
        {
            currentIndex = 0;
        }
        UpdateActiveObject();
    }

    // Funci�n para retroceder al objeto anterior
    public void PreviousObject()
    {
        // Decrementamos el �ndice y lo "envolvemos" si es menor que 0
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = objectsToSwitch.Count - 1;
        }
        UpdateActiveObject();
    }

    // Actualizamos el objeto activo en funci�n del �ndice actual
    private void UpdateActiveObject()
    {
        // Desactiva todos los objetos primero
        for (int i = 0; i < objectsToSwitch.Count; i++)
        {
            objectsToSwitch[i].SetActive(false);
        }

        // Activa solo el objeto en la posici�n actual
        if (objectsToSwitch.Count > 0)
        {
            objectsToSwitch[currentIndex].SetActive(true);
        }
    }
}
