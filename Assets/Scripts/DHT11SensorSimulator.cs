using UnityEngine;
using TMPro;
using System.Globalization;

public class DHT11SensorSimulator : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // Referencia al componente TextMeshPro
    public float updateInterval = 1.0f; // Intervalo de actualización en segundos
    public float minTemperature = 15.0f; // Temperatura mínima simulada
    public float maxTemperature = 30.0f; // Temperatura máxima simulada
    public float minHumidity = 20.0f; // Humedad mínima simulada
    public float maxHumidity = 80.0f; // Humedad máxima simulada

    private float nextUpdateTime;
    private string sensorTextFormat;

    void Start()
    {
        SetLanguageBasedText();
        nextUpdateTime = Time.time + updateInterval;
        SimulateSensor(); // Inicializa la primera simulación
    }

    void Update()
    {
        if (Time.time >= nextUpdateTime)
        {
            SimulateSensor();
            nextUpdateTime = Time.time + updateInterval;
        }
    }

    void SetLanguageBasedText()
    {
        // Detecta el idioma del sistema
        string systemLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

        // Configura el formato del texto basado en el idioma detectado
        if (systemLanguage == "es") // Español
        {
            sensorTextFormat = "Temperatura: {0}°C\nHumedad: {1}%";
        }
        else // Inglés u otros idiomas
        {
            sensorTextFormat = "Temperature: {0}°C\nHumidity: {1}%";
        }
    }

    void SimulateSensor()
    {
        // Genera una temperatura simulada aleatoria entre el rango mínimo y máximo
        float simulatedTemperature = Random.Range(minTemperature, maxTemperature);

        // Genera una humedad simulada aleatoria entre el rango mínimo y máximo
        float simulatedHumidity = Random.Range(minHumidity, maxHumidity);

        // Formatea el texto con la temperatura y la humedad
        textMeshPro.text = string.Format(sensorTextFormat, simulatedTemperature.ToString("F1"), simulatedHumidity.ToString("F1"));

        // Puedes hacer lo que necesites con los valores simulados aquí
        Debug.Log(string.Format(sensorTextFormat, simulatedTemperature.ToString("F1"), simulatedHumidity.ToString("F1")));
    }
}
