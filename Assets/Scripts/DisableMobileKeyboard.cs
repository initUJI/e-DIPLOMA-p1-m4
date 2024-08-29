using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisableMobileKeyboard : MonoBehaviour
{
    public TMP_InputField inputField;

    void Start()
    {
        // Desactivar el teclado solo en plataformas móviles
        if (Application.isMobilePlatform)
        {
            inputField.shouldHideMobileInput = true;
        }
    }
}
