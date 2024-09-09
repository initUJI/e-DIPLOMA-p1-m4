using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class InfoProcessorSummary : MonoBehaviour
{

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
        text.text = CreateSummary();
        finishButton.SetActive(false);
        reloadButton.SetActive(true);
        confirmationPanel.SetActive(false);
        logger.LogEvent("Finish button pressed with results: " + text.text);
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

