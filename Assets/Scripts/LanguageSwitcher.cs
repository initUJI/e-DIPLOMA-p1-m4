using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageSwitcher : MonoBehaviour
{
    public Sprite spanishFlagSprite; // La imagen de la bandera de España
    public Sprite englishFlagSprite; // La imagen de la bandera del Reino Unido
    public Button languageButton;    // El botón que se usará para cambiar de idioma
    public Image childImage;         // La imagen hija del botón que se debe cambiar

    private bool isSpanish = true;   // Variable para controlar el idioma actual

    void Start()
    {
        // Configurar el botón para llamar a SwitchLanguage cuando se haga clic
        languageButton.onClick.AddListener(SwitchLanguage);

        // Asegurarse de que la imagen hija tenga la imagen correcta al inicio
        UpdateChildImage();
    }

    void SwitchLanguage()
    {
        isSpanish = !isSpanish; // Cambiar el idioma

        // Cambiar el idioma utilizando el sistema de localización de Unity
        if (isSpanish)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(locale => locale.Identifier.Code == "es-ES");
        }
        else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(locale => locale.Identifier.Code == "en");
        }

        // Actualizar la imagen hija del botón
        UpdateChildImage();
    }

    void UpdateChildImage()
    {
        if (isSpanish)
        {
            childImage.sprite = englishFlagSprite;
        }
        else
        {
            childImage.sprite = spanishFlagSprite;
        }
    }
}
