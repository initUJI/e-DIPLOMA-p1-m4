using UnityEngine;
using TMPro;

public class NumericKeyboardController : MonoBehaviour
{
    public TMP_InputField targetText;  // El campo de texto donde se mostrará la entrada

    public void OnNumberButtonPressed(TextMeshProUGUI number)
    {
        targetText.text += number.text;  // Añade el número presionado al texto objetivo
    }

    public void OnDeleteButtonPressed()
    {
        if (targetText.text.Length > 0)
        {
            targetText.text = targetText.text.Substring(0, targetText.text.Length - 1);  // Elimina el último carácter
        }
    }
}
