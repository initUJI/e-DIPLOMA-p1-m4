using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using TMPro;
using UnityEngine.Localization.Tables;

public class LocalizedTextUpdater : MonoBehaviour
{
    private LocalizeStringEvent localizeStringEvent;
    private TextMeshProUGUI textComponent;
    private Locale currentLocale;
    private const string LanguagePrefKey = "SelectedLanguage";

    void Awake()
    {
        // Intentamos obtener los componentes solo una vez en Awake para mejorar la eficiencia
        localizeStringEvent = GetComponent<LocalizeStringEvent>();
        textComponent = GetComponent<TextMeshProUGUI>();

        // Validación de los componentes necesarios
        if (localizeStringEvent == null)
        {
            Debug.LogError($"[LocalizedTextUpdater] El componente LocalizeStringEvent no se encuentra en {gameObject.name}");
        }

        if (textComponent == null)
        {
            Debug.LogError($"[LocalizedTextUpdater] El componente TextMeshProUGUI no se encuentra en {gameObject.name}");
        }

        // Suscribirse a los cambios de idioma
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;

        // Almacenar el idioma actual para evitar actualizaciones innecesarias
        currentLocale = LocalizationSettings.SelectedLocale;
    }

    void OnDestroy()
    {
        // Desuscribirse al destruir el objeto para evitar memory leaks
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    // Método que se llama cuando se cambia el idioma
    private void OnLocaleChanged(Locale newLocale)
    {
        // Solo actualizamos si el idioma ha cambiado realmente
        if (currentLocale != newLocale)
        {
            currentLocale = newLocale;
            UpdateLocalizedText();
        }
    }

    // Método que se llama cuando el objeto se activa
    void OnEnable()
    {
        // Actualizamos solo si el idioma actual no coincide con el que tenía antes
       // if (currentLocale != LocalizationSettings.SelectedLocale)
        //{
            //currentLocale = LocalizationSettings.SelectedLocale;
            UpdateLocalizedText();
       // }
    }

    // Método que asegura que el texto esté actualizado
    private void UpdateLocalizedText()
    {
        if (localizeStringEvent == null || textComponent == null)
        {
            Debug.LogWarning($"[LocalizedTextUpdater] No se pudo actualizar el texto en {gameObject.name}. Asegúrate de que los componentes necesarios estén configurados correctamente.");
            return;
        }

        // Obtener el código de idioma desde PlayerPrefs
        string languageCode = PlayerPrefs.GetString(LanguagePrefKey, "en"); // Valor predeterminado: "en"
        Locale locale = GetLocaleByCode(languageCode);

        if (locale == null)
        {
            Debug.LogWarning($"Idioma '{languageCode}' no encontrado en los idiomas disponibles.");
            return;
        }

        // Configurar el Locale seleccionado en LocalizationSettings
        LocalizationSettings.SelectedLocale = locale;

        // Obtener tableReference y entryReference desde localizeStringEvent
        string tableReference = localizeStringEvent.StringReference.TableReference.TableCollectionName;
        long entryReference = localizeStringEvent.StringReference.TableEntryReference.KeyId;

        // Obtener la tabla de strings y esperar a que se cargue completamente
        StringTable stringTable = LocalizationSettings.StringDatabase.GetTable(tableReference) as StringTable;
        if (stringTable == null)
        {
            Debug.LogWarning($"No se encontró la tabla de strings '{tableReference}'.");
            return;
        }

        // Obtener la entrada de la tabla usando entryReference
        var entry = stringTable.GetEntry(entryReference);
        if (entry == null)
        {
            Debug.LogWarning($"No se encontró la entrada con ID '{entryReference}' en la tabla '{tableReference}'.");
            return;
        }

        // Obtener el texto localizado y actualizar el componente de texto
        string localizedText = entry.GetLocalizedString(locale);
        textComponent.text = localizedText;
        Debug.Log($"[LocalizedTextUpdater] Texto actualizado a '{localizedText}' en {gameObject.name} para el idioma {locale.LocaleName}.");
    }

    // Método auxiliar para obtener el Locale basado en el código de idioma
    private Locale GetLocaleByCode(string languageCode)
    {
        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            if (locale.Identifier.Code == languageCode)
            {
                return locale;
            }
        }

        return null; // Retorna null si el idioma no se encuentra
    }
}
