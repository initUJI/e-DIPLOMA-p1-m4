using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EventLogger : MonoBehaviour
{
    [Serializable]
    public class EventData
    {
        public string userId;
        public DateTime eventDateTime;
        public string eventName;
        public long unixTime;
    }

    [Serializable]
    public class EventLog
    {
        public List<EventData> events = new List<EventData>();
    }

    private EventLog eventLog = new EventLog();
    private string dataFolderPath;
    private string filePath;
    private string userId;
    private DateTime sessionStartTime;

    void Start()
    {
        // Establece la ruta de la carpeta Data
        dataFolderPath = Path.Combine(Application.persistentDataPath, "Data");

        // Crea la carpeta Data si no existe
        if (!Directory.Exists(dataFolderPath))
        {
            Directory.CreateDirectory(dataFolderPath);
        }

        // Debug del camino exacto de la carpeta
        Debug.Log("La carpeta de datos es: " + dataFolderPath);
    }

    // Método para iniciar la sesión y establecer el userId
    public void StartSession(string userId)
    {
        this.userId = userId;
        sessionStartTime = DateTime.Now;

        // Establece el nombre del archivo JSON usando el ID de usuario y la fecha con hora
        string fileName = $"{userId}_{sessionStartTime.ToString("yyyyMMdd_HHmmss")}.json";
        filePath = Path.Combine(dataFolderPath, fileName);

        // Carga el registro de eventos si existe
        LoadEventLog();

        // Debug del camino exacto
        Debug.Log("El archivo de eventos se guardará en: " + filePath);
    }

    // Método para registrar un nuevo evento
    public void LogEvent(string eventName)
    {
        if (string.IsNullOrEmpty(userId))
        {
            Debug.Log("Error: La sesión no ha sido iniciada. La ID por defecto será test.");
            userId = "test";

            // Establece el nombre del archivo JSON usando el ID de usuario y la fecha con hora
            string fileName = $"{userId}_{sessionStartTime.ToString("yyyyMMdd_HHmmss")}.json";
            filePath = Path.Combine(dataFolderPath, fileName);

            // Carga el registro de eventos si existe
            LoadEventLog();

            // Debug del camino exacto
            Debug.Log("El archivo de eventos se guardará en: " + filePath);
        }

        EventData newEvent = new EventData
        {
            userId = userId,
            eventDateTime = DateTime.Now,
            eventName = eventName,
            unixTime = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds()
        };

        eventLog.events.Add(newEvent);
        SaveEventLog();
    }

    // Método para guardar el registro de eventos en un archivo JSON
    private void SaveEventLog()
    {
        string json = JsonUtility.ToJson(eventLog, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Registro de eventos guardado en: " + filePath);
    }

    // Método para cargar el registro de eventos desde un archivo JSON
    private void LoadEventLog()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            eventLog = JsonUtility.FromJson<EventLog>(json);
            Debug.Log("Registro de eventos cargado desde: " + filePath);
        }
    }
}
