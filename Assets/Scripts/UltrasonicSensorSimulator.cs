using UnityEngine;
using TMPro;
using System.Globalization;

public class UltrasonicSensorSimulator : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // Referencia al componente TextMeshPro
    public float updateInterval = 1.0f; // Intervalo de actualizaci�n en segundos
    public float minDistance = 0.0f; // Distancia m�nima que puede medir el sensor
    public float maxDistance = 5.0f; // Distancia m�xima que puede medir el sensor
    public string unit = "m"; // Unidad de medida, por ejemplo, "m" para metros

    private float nextUpdateTime;
    private string distanceTextFormat;

    void Start()
    {
        SetLanguageBasedText();
        nextUpdateTime = Time.time + updateInterval;
        SimulateSensor(); // Inicializa la primera simulaci�n
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
        if (systemLanguage == "es") // Espa�ol
        {
            distanceTextFormat = "Distancia Ultrasonica: {0} {1}";
        }
        else // Ingl�s u otros idiomas
        {
            distanceTextFormat = "Ultrasonic Distance: {0} {1}";
        }
    }

    void SimulateSensor()
    {
        // Genera una distancia simulada aleatoria entre el rango m�nimo y m�ximo
        float simulatedDistance = Random.Range(minDistance, maxDistance);

        // Formatea el texto con la distancia y la unidad
        textMeshPro.text = string.Format(distanceTextFormat, simulatedDistance.ToString("F4"), unit);

        // Puedes hacer lo que necesites con la distancia simulada aqu�
        //Debug.Log(string.Format(distanceTextFormat, simulatedDistance.ToString("F4"), unit));
    }
}
