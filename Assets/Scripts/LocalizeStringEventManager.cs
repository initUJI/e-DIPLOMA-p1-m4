using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class LocalizeStringEventManager : MonoBehaviour
{
    private LocalizeStringEvent localizeStringEvent;
    public bool isLoaded = false;

    void Start()
    {
        localizeStringEvent = GetComponent<LocalizeStringEvent>();

        if (localizeStringEvent == null)
        {
            Debug.LogError("LocalizeStringEvent no encontrado en el GameObject.");
            return;
        }

        Debug.Log("LocalizeStringEvent encontrado. Iniciando carga de strings localizados.");

        // Llama a la funci�n de carga sincr�nica y marca como cargado si es exitoso
        var loadedStrings = LoadLocalizedStrings();
        if (loadedStrings.Count > 0)
        {
            isLoaded = true;
            Debug.Log("Carga de strings localizados completa.");
        }
        else
        {
            Debug.LogError("Error en la carga de strings localizados.");
        }
    }

    private List<string> LoadLocalizedStrings()
    {
        List<string> localizedStrings = new List<string>();

        // Esperar hasta que la localizaci�n est� inicializada
        if (!LocalizationSettings.InitializationOperation.IsDone)
        {
            LocalizationSettings.InitializationOperation.WaitForCompletion();
        }

        Debug.Log("Localizaci�n inicializada. Cargando tabla de strings...");

        var tableReference = localizeStringEvent.StringReference.TableReference.TableCollectionName;
        var entryReference = localizeStringEvent.StringReference.TableEntryReference.KeyId;

        // Mostrar los valores de tableReference y entryReference para depuraci�n
        Debug.Log($"tableReference: {tableReference}");
        Debug.Log($"entryReference: {entryReference}");

        // Obtener la tabla de strings de forma s�ncrona
        StringTable stringTable = LocalizationSettings.StringDatabase.GetTable(tableReference) as StringTable;

        if (stringTable != null)
        {
            Debug.Log("Tabla de strings cargada. Procesando idiomas...");
            List<Locale> locales = LocalizationSettings.AvailableLocales.Locales;

            foreach (Locale locale in locales)
            {
                // Cambiar el idioma temporalmente para cada Locale
                LocalizationSettings.SelectedLocale = locale;

                // Obtener la entrada de la tabla en el idioma actual
                var entry = stringTable.GetEntry(entryReference);

                if (entry != null)
                {
                    string localizedValue = entry.GetLocalizedString();
                    localizedStrings.Add(localizedValue);

                    Debug.Log($"Localized string for {locale.LocaleName}: {localizedValue}");
                }
                else
                {
                    Debug.LogWarning($"No entry found for {locale.LocaleName}");
                }
            }
        }
        else
        {
            Debug.LogError("Error: Tabla de strings no encontrada.");
        }

        return localizedStrings;
    }


    public List<string> GetLocalizedStrings()
    {
        /*if (!isLoaded)
        {
            Debug.LogWarning("Los strings a�n no est�n cargados.");
            return new List<string>();
        }*/

        // Llama a LoadLocalizedStrings para obtener los strings cargados como par�metro local
        List<string> localizedStrings = LoadLocalizedStrings();
        Debug.Log("GetLocalizedStrings: Total strings en la lista: " + localizedStrings.Count);
        return localizedStrings;
    }
}
