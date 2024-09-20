using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using TMPro;

public class LocalizedTextUpdater : MonoBehaviour
{
    private LocalizeStringEvent localizeStringEvent;
    private TextMeshProUGUI textComponent;
    private Locale currentLocale;

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
        if (currentLocale != LocalizationSettings.SelectedLocale)
        {
            currentLocale = LocalizationSettings.SelectedLocale;
            UpdateLocalizedText();
        }
    }

    // Método que asegura que el texto esté actualizado
    private void UpdateLocalizedText()
    {
        if (localizeStringEvent != null && textComponent != null)
        {
            // Actualizar el texto cuando cambie el idioma o se active el objeto
            localizeStringEvent.RefreshString();
            Debug.Log($"[LocalizedTextUpdater] Texto actualizado en {gameObject.name} al cambiar de idioma o activarse.");
        }
        else
        {
            Debug.LogWarning($"[LocalizedTextUpdater] No se pudo actualizar el texto en {gameObject.name}. Asegúrate de que los componentes necesarios estén configurados correctamente.");
        }
    }
}
