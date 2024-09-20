using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class LocalizeStringEventManager : MonoBehaviour
{
    private LocalizeStringEvent localizeStringEvent;
    private List<string> localizedStrings = new List<string>();
    private bool isLoaded = false;

    void Start()
    {
        Debug.Log("Start: Iniciando carga de LocalizeStringEvent.");

        // Obtener el componente LocalizeStringEvent
        localizeStringEvent = GetComponent<LocalizeStringEvent>();

        if (localizeStringEvent == null)
        {
            Debug.LogError("LocalizeStringEvent no encontrado en el GameObject.");
        }
        else
        {
            Debug.Log("LocalizeStringEvent encontrado. Iniciando carga de strings localizados.");
            // Iniciar la corrutina para cargar los strings localizados
            StartCoroutine(LoadLocalizedStringsCoroutine());
        }
    }

    private IEnumerator LoadLocalizedStringsCoroutine()
    {
        Debug.Log("Esperando a que la localización esté inicializada...");
        // Esperar a que la localización esté inicializada
        yield return LocalizationSettings.InitializationOperation;

        Debug.Log("Localización inicializada. Cargando tabla de strings...");
        // Obtener la referencia de la tabla y la entrada
        var tableReference = localizeStringEvent.StringReference.TableReference;
        var entryReference = localizeStringEvent.StringReference.TableEntryReference;

        // Cargar la tabla de strings de manera asíncrona
        var stringTableOperation = LocalizationSettings.StringDatabase.GetTableAsync(tableReference);
        yield return stringTableOperation;

        if (stringTableOperation.Result != null)
        {
            Debug.Log("Tabla de strings cargada. Procesando idiomas...");
            // Recorrer todos los idiomas disponibles
            List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;
            foreach (Locale locale in locales)
            {
                // Cambiar temporalmente al idioma actual
                LocalizationSettings.SelectedLocale = locale;

                // Obtener el valor localizado para cada idioma
                string localizedValue = localizeStringEvent.StringReference.GetLocalizedString();
                localizedStrings.Add(localizedValue);

                Debug.Log($"Localized string for {locale.LocaleName}: {localizedValue}");
            }
        }
        else
        {
            Debug.LogError("Error: Tabla de strings no encontrada.");
        }

        // Marcar que los strings localizados han sido cargados
        isLoaded = true;
        Debug.Log("Carga de strings localizados completa.");
    }

    public List<string> GetLocalizedStrings()
    {
        Debug.Log("GetLocalizedStrings: Verificando si los strings están cargados...");

        // Si ya está cargado, devolver la lista directamente
        if (isLoaded)
        {
            Debug.Log("Los strings ya están cargados. Devolviendo la lista.");
            return localizedStrings;
        }
        else
        {
            // Si no está cargado, forzar la carga sincronizando la corrutina
            Debug.LogWarning("Los strings aún no están cargados. Forzando la carga...");
            LoadLocalizedStringsSync();
            return localizedStrings;
        }
    }

    private void LoadLocalizedStringsSync()
    {
        Debug.Log("LoadLocalizedStringsSync: Ejecutando la corrutina de carga de strings...");
        // Ejecutar la corrutina de carga y esperar que termine
        StartCoroutine(LoadLocalizedStringsAndWait());
    }

    private IEnumerator LoadLocalizedStringsAndWait()
    {
        Debug.Log("LoadLocalizedStringsAndWait: Iniciando la espera y carga de strings.");
        // Iniciar la corrutina para cargar los strings y esperar hasta que termine
        yield return LoadLocalizedStringsCoroutine();
    }
}
