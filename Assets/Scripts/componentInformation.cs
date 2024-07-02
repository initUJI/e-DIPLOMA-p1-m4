using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class componentInformation : MonoBehaviour
{
    public string[] valueName;
    public string[] value;
    public string[] symbol;

    [HideInInspector]
    public GameObject attachedMenu;

    private List<GameObject> valueTextsList = new List<GameObject>(); 

    // Start is called before the first frame update
    void Start()
    {
         actualiceMenu();
    }

    void actualiceMenu()
    {
        if (attachedMenu != null)
        {
            for (int i = 0; i < attachedMenu.transform.childCount; i++)
            {
                GameObject child = attachedMenu.transform.GetChild(i).gameObject;
                if (child.name == "ComponentName")
                {
                    child.GetComponent<TextMeshProUGUI>().text = name.Replace("(Clone)", "");
                }
                else if (child.name.Contains("ValueText"))
                {
                    valueTextsList.Add(child);
                }
            }

            int j = 0;
            for (int i = 0; i < valueName.Length; i++)
            {
                if (j < valueTextsList.Count)
                {
                    valueTextsList[j].GetComponent<TextMeshProUGUI>().text = $"{valueName[i]}: {value[i]}{symbol[i]}";
                    j++;
                }
            }
        }
        
    }

    public void f_modifyValue(int i, string value = "")
    {
        this.value[i] = value;
        actualiceMenu();
    }

    // "4-Digit Display"
    //
    //  0->  valueName = "Numbers"
    //       value = "0"
    //       symbol = ""


    // "DHT11"
    //
    //  0->  valueName = "Ambient temperature"
    //       symbol = "°C"
    //
    //  1->  valueName = "Relative humidity"
    //       symbol = "%"


    // "Grove Chainable RGB LED"
    //
    //  0->  valueName = "Color R"  0 to 255
    //       symbol = ""
    //
    //  1->  valueName = "Color G"
    //       symbol = ""
    //
    //  2->  valueName = "Color B"
    //       symbol = ""
    //
    //  3->  valueName = "Glow"
    //       symbol = ""
    //
    //  4->  valueName = "Patterns"
    //       value = "None"
    //       symbol = ""


    // "Grove LED bar"
    //
    //  0->  valueName = "Percentage illuminated"
    //       value = "0"
    //       symbol = "%"


    // "LED Socket Kit"
    //
    //  0->  valueName = "Color R"  0 to 255
    //       symbol = ""
    //
    //  1->  valueName = "Color G"
    //       symbol = ""
    //
    //  2->  valueName = "Color B"
    //       symbol = ""
    //
    //  3->  valueName = "Glow"
    //       symbol = ""
    //
    //  4->  valueName = "Patterns"
    //       value = "None"
    //       symbol = ""


    // "Light Sensor"
    //
    //  0->  valueName = "Light intensity"
    //       symbol = "lm"
    //
    //  1->  valueName = "Brightness level"
    //       symbol = "%"


    // "Moisture Sensor"
    //
    //  0->  valueName = "Volumetric soil water content"
    //       symbol = "%"
    //
    //  1->  valueName = "Ambient relative humidity"
    //       symbol = "%"


    // "Rotary Angle Sensor"
    //
    //  0->  valueName = "Angle of rotation"
    //       symbol = "°"
    //
    //  1->  valueName = "Angular velocity"
    //       symbol = "rad"


    // "Sound Sensor"
    //
    //  0->  valueName = "Sound level"
    //       symbol = "cB"
    //
    //  1->  valueName = "Sound frequency"
    //       symbol = "Hz"


    // "Ultrasonic Sensor"
    //
    //  0->  valueName = "Distance"  0 to 255
    //       symbol = "cm"
    //
    //  1->  valueName = "Flight time"
    //       symbol = "s"
    //
    //  2->  valueName = "Speed of sound"
    //       symbol = "m/S"
 
}
