using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LanguageSwitcher : MonoBehaviour
{
    public Sprite spanishFlagSprite;
    public Sprite englishFlagSprite;
    public Button languageButton;
    public Image childImage;

    private const string LanguagePrefKey = "SelectedLanguage";
    private bool isSpanish = false;

    void Start()
    {
       // Debug.Log("Inicializando LanguageSwitcher...");
        LoadLanguageFromPrefs();
        languageButton.onClick.AddListener(SwitchLanguage);
        UpdateChildImage();
       // Debug.Log("LanguageSwitcher inicializado correctamente.");
    }

    void SwitchLanguage()
    {
        //Debug.Log("Cambiando idioma...");
        isSpanish = !isSpanish;

        if (isSpanish)
        {
            //Debug.Log("Cambiando a español...");
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(locale => locale.Identifier.Code == "es-ES");
            PlayerPrefs.SetString(LanguagePrefKey, "es-ES");
        }
        else
        {
           // Debug.Log("Cambiando a inglés...");
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(locale => locale.Identifier.Code == "en");
            PlayerPrefs.SetString(LanguagePrefKey, "en");
        }

        PlayerPrefs.Save();
        UpdateChildImage();

        // Forzamos la actualización de textos en todos los objetos, incluyendo los que pueden estar desactivados
        ReloadAllTextMeshPro();
        UpdateAllLocalizedTexts();
        //Debug.Log("Cambio de idioma completado.");
        // Obtén el nombre o índice de la escena activa
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Recarga la escena
        SceneManager.LoadScene(currentSceneName);
    }

    void UpdateChildImage()
    {
        //Debug.Log("Actualizando imagen de la bandera...");
        if (isSpanish)
        {
            childImage.sprite = englishFlagSprite;
        }
        else
        {
            childImage.sprite = spanishFlagSprite;
        }
        //Debug.Log("Imagen actualizada.");
    }

    void LoadLanguageFromPrefs()
    {
        //Debug.Log("Cargando idioma de PlayerPrefs...");
        string savedLanguage = PlayerPrefs.GetString(LanguagePrefKey, "en");

        if (savedLanguage == "es-ES")
        {
           // Debug.Log("Idioma guardado es español.");
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(locale => locale.Identifier.Code == "es-ES");
            isSpanish = true;
        }
        else
        {
           // Debug.Log("Idioma guardado es inglés.");
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(locale => locale.Identifier.Code == "en");
            isSpanish = false;
        }

        UpdateChildImage();
    }

    public void ReloadAllTextMeshPro()
    {
        // Encuentra todos los objetos en la escena con un componente TextMeshProUGUI
        TextMeshProUGUI[] tmProUGUITexts = FindObjectsOfType<TextMeshProUGUI>();
        TextMeshPro[] tmProTexts = FindObjectsOfType<TextMeshPro>();

        // Recarga TextMeshProUGUI (usualmente en UI)
        foreach (TextMeshProUGUI text in tmProUGUITexts)
        {
            text.ForceMeshUpdate(); // Fuerza la actualización del texto
            Debug.Log($"TextMeshProUGUI actualizado: {text.text}");
        }

        // Recarga TextMeshPro (usualmente en objetos 3D)
        foreach (TextMeshPro text in tmProTexts)
        {
            text.ForceMeshUpdate(); // Fuerza la actualización del texto
            Debug.Log($"TextMeshPro actualizado: {text.text}");
        }
    }

    // Forzar actualización de textos localizados, incluidos los objetos que pueden estar desactivados
    void UpdateAllLocalizedTexts()
    {
        //Debug.Log("Forzando actualización de todos los textos localizados...");

        LocalizeStringEvent[] localizedTexts = Resources.FindObjectsOfTypeAll<LocalizeStringEvent>();

        foreach (var localizeStringEvent in localizedTexts)
        {
            if (localizeStringEvent == null) continue;

            // Comprobar si el objeto pertenece a la escena activa para evitar trabajar con prefabs
            if (localizeStringEvent.gameObject.scene.IsValid())
            {
                //Debug.Log($"Forzando actualización para {localizeStringEvent.gameObject.name}");

                // Si el objeto está desactivado, activarlo temporalmente para que se actualice
                bool wasActive = localizeStringEvent.gameObject.activeSelf;

                if (!wasActive)
                {
                    localizeStringEvent.gameObject.SetActive(true);
                }

                // Forzar la recarga del texto
                localizeStringEvent.RefreshString();

                // Restaurar el estado original
                if (!wasActive)
                {
                    localizeStringEvent.gameObject.SetActive(false);
                }
            }
        }

        //Debug.Log("Actualización de textos completada.");
    }
}
