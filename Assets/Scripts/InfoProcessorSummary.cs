using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoProcessorSummary : MonoBehaviour
{
    public GameObject results;
    public TextMeshProUGUI text;
    [SerializeField]
    private GameObject finishButton;
    [SerializeField]
    private GameObject reloadButton;
    [SerializeField]
    private GameObject confirmationPanel;

    [SerializeField]
    private InfoProcessor[] infoProcessors;

    private EventLogger logger;
    void Start()
    {
        logger = FindObjectOfType<EventLogger>();
        string summary = CreateSummary();
        results.SetActive(false);
        //Debug.Log(summary);
    }

    public void openConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
    }

    public void closeConfirmationPanel()
    {
        confirmationPanel.SetActive(false);
    }

    public void showSummary()
    {
        results.SetActive(true);
        text.text = CreateSummary();
        finishButton.SetActive(false);
        if (reloadButton != null)
        {
            reloadButton.SetActive(true);
        }
        
        confirmationPanel.SetActive(false);
        logger.LogEvent("Finish button pressed with results: " + text.text);
        GetComponent<buttonTest>().desactiveModelTargets();
    }

    // Método para marcar el modo de componentes como completado
    public void SetComponentsModeCompleted(bool completed)
    {
        PlayerPrefs.SetInt("ComponentsModeCompleted", completed ? 1 : 0);
        PlayerPrefs.Save();  // Guarda los cambios en PlayerPrefs
        Debug.Log("Modo componentes completado: " + completed);
    }

    string CreateSummary()
    {
        StringBuilder summaryBuilder = new StringBuilder();

        foreach (InfoProcessor infoProcessor in infoProcessors)
        {
            string objectName = infoProcessor.gameObject.name;
            if (objectName.Contains("Ultrasonic") || objectName.Contains("Arduino") ||
                objectName.Contains("BaseShield") || objectName.Contains("DHT11"))
            {
                string formattedName = FormatName(objectName);
                int solvedCount = infoProcessor.solvedCount;
                int collisionCount = infoProcessor.uiCollisionDetectors.Count;
                summaryBuilder.AppendFormat("{0}: {1}/{2}   ", formattedName, solvedCount, collisionCount);
            }
        }

        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            SetComponentsModeCompleted(true);
        }
        
        return summaryBuilder.ToString().TrimEnd();
    }

    string FormatName(string objectName)
    {
        if (objectName.Contains("Ultrasonic"))
        {
            return "Ultrasonic";
        }
        else if (objectName.Contains("Arduino"))
        {
            return "Arduino";
        }
        else if (objectName.Contains("BaseShield"))
        {
            return "BaseShield";
        }
        else if (objectName.Contains("DHT11"))
        {
            return "DHT11";
        }
        return objectName;
    }
}

