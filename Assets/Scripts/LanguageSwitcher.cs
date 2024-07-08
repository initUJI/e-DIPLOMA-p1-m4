using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class LanguageSwitcher : MonoBehaviour
{
    public Sprite spanishFlagSprite; // La imagen de la bandera de Espa�a
    public Sprite englishFlagSprite; // La imagen de la bandera del Reino Unido
    public Button languageButton;    // El bot�n que se usar� para cambiar de idioma
    public Image childImage;         // La imagen hija del bot�n que se debe cambiar

    private bool isSpanish = true;   // Variable para controlar el idioma actual

    void Start()
    {
        // Configurar el bot�n para llamar a SwitchLanguage cuando se haga clic
        languageButton.onClick.AddListener(SwitchLanguage);

        // Asegurarse de que la imagen hija tenga la imagen correcta al inicio
        UpdateChildImage();
    }

    void SwitchLanguage()
    {
        isSpanish = !isSpanish; // Cambiar el idioma

        // Cambiar el idioma utilizando el sistema de localizaci�n de Unity
        if (isSpanish)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(locale => locale.Identifier.Code == "es-ES");
        }
        else
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(locale => locale.Identifier.Code == "en");
        }

        // Actualizar la imagen hija del bot�n
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
