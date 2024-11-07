using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public List<GameObject> tutorialSteps; // Lista de GameObjects para cada paso del tutorial
    public List<GameObject> infos; // Lista de GameObjects para cada paso del tutorial
    public List<TextMeshProUGUI> nameTexts;
    public GameObject playButton;
    public GameObject finishButton;
    public TextMeshProUGUI resultsText;
    public Manager manager;

    private int currentIndex = -1; // Índice del paso actual del tutorial

    private bool sensorDetectedBool = false;
    private bool infoSelected = false;
    private bool playPressed = false;
    private bool optionSelected = false;
    private bool optionColocated = false;
    private bool tutorialFinished = false;

    private void Update()
    {
        if (oneInfoVisible() && sensorDetectedBool && !infoSelected)
        {
            ActivateNextStep();
            infoSelected = true;
            playButton.SetActive(true);
        }
        else if(infoSelected && !playButton.activeInHierarchy && !playPressed)
        {
            ActivateNextStep();
            playPressed = true;
        }
        else if (playPressed && manager.GetCurrentlyHighlighted() && !optionSelected)
        {
            ActivateNextStep();
            optionSelected = true;
            finishButton.SetActive(false);
        }
        else if (optionSelected && checkTexts() && !optionColocated)
        {
            ActivateNextStep();
            finishButton.SetActive(true);
            optionColocated = true;
        }
        else if (optionColocated && resultsText.text.Length > 0 && !tutorialFinished)
        {
            ActivateNextStep();
            tutorialFinished = true;
        }
    }

    void Start()
    {
        manager.SetPortraitOrientation();
        playButton.SetActive(false);
        if (tutorialSteps.Count > 0)
        {
            ActivateNextStep(); // Activa el primer paso
        }
        else
        {
            Debug.LogWarning("La lista de tutorialSteps está vacía.");
        }
    }

    private bool checkTexts()
    {
        foreach (var text in nameTexts) 
        {
            if (text.text.Length > 0)
            {
                return true;
            }
        }
        return false;
    }

    private bool oneInfoVisible()
    {
        foreach (GameObject i in infos)
        {
            if (i != null && i.activeInHierarchy) { return true; }
        }
        return false;
    }

    // Método para activar el siguiente paso y desactivar el actual
    public void ActivateNextStep()
    {
        if (currentIndex >= 0 && currentIndex < tutorialSteps.Count)
        {
            // Desactiva el paso actual
            tutorialSteps[currentIndex].SetActive(false);
        }

        // Incrementa el índice para avanzar al siguiente paso
        currentIndex++;

        // Verifica si el índice actual está dentro del rango de la lista
        if (currentIndex < tutorialSteps.Count)
        {
            // Activa el siguiente paso
            tutorialSteps[currentIndex].SetActive(true);
            Debug.Log("Paso del tutorial activado: " + currentIndex);
        }
        else
        {
            Debug.Log("Último paso alcanzado. No hay más pasos en el tutorial.");
        }
    }

    public void sensorDetected()
    {
        if (!sensorDetectedBool) 
        {
            ActivateNextStep();
        }
        sensorDetectedBool = true;
    }
}
