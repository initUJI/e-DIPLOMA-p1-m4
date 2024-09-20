using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UICollisionDetector : MonoBehaviour, IPointerClickHandler
{
    public Manager manager;  // Referencia al Manager para gestionar el objeto brillando actualmente
    public InfoProcessor infoProcessor;  // Referencia al InfoProcessor para actualizaciones de estado
    public LocalizeStringEventManager localizeStringEventManager;

    private EventLogger logger;
    private GameObject[] grayBoxes;  // Referencia a las cajas azules

    [HideInInspector]
    public bool solved;

    private void Start()
    {
        logger = FindFirstObjectByType<EventLogger>();
        solved = false;   
        manager = FindFirstObjectByType<Manager>();
        Initialize();
    }
    private void Initialize()
    {
        EnsureBoxCollider();  // Asegura que el objeto tenga un BoxCollider para detectar clics
    }

    private void EnsureBoxCollider()
    {
        if (GetComponent<BoxCollider>() == null)
        {
            gameObject.AddComponent<BoxCollider>();  // Añade un BoxCollider si no lo tiene
        }
    }

    // Función para manejar el toque o clic en el objeto
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Entra en OnPointerClick");
        // Verificar si hay un objeto actualmente brillando en el manager
        GameObject highlightedObject = manager.GetCurrentlyHighlighted();

        if (highlightedObject != null)
        {
            Debug.Log("Objeto brillante encontrado");
            // Verificar si el receptor ya tiene un texto (no está en blanco)
            TextMeshProUGUI targetTextComponent = GetComponent<TextMeshProUGUI>();
            if (!string.IsNullOrEmpty(targetTextComponent.text))
            {
                // Buscar el objeto que tenía originalmente el texto
                ImageOutline originalOutline = manager.FindImageOutlineByText(targetTextComponent.text);
                if (originalOutline != null)
                {
                    // Si el objeto original está en estado translúcido, restaurarlo
                    originalOutline.RemoveGlow();

                    if (originalOutline.box != null)
                    {
                        Destroy(originalOutline.box);
                    }

                }
            }

            // Si hay un objeto brillando, transferir su texto al objeto clicado (este)
            TransferTextAndFontSize(highlightedObject);
            createGrayBox();

            // Después de transferir el texto, hacer el objeto actualmente brillante translúcido
            ImageOutline outline = highlightedObject.GetComponent<ImageOutline>();
            if (outline != null)
            {
                outline.MakeTranslucent();  // Hacer que las imágenes de los hijos se vuelvan translúcidas

                // Guardar la referencia al UICollisionDetector que recibió el texto
                outline.transferredTo = this;
            }

            // Desiluminar el objeto que brillaba usando UnhighlightAll
            manager.UnhighlightAll();  // Cambiado de UnhighlightCurrentlySelected a UnhighlightAll
        }
        else
        {
            Debug.Log("No hay objeto brillante seleccionado");
        }
    }

    private void TransferTextAndFontSize(GameObject highlightedObject)
    {
        // Obtener el componente de texto de este objeto
        TextMeshProUGUI targetTextComponent = GetComponent<TextMeshProUGUI>();  // Este objeto
        if (targetTextComponent != null)
        {
            Debug.Log("Target TextMeshProUGUI found on " + gameObject.name);
        }
        else
        {
            Debug.LogError("Target TextMeshProUGUI not found on " + gameObject.name);
        }

        // Obtener el componente de texto del objeto resaltado
        TextMeshProUGUI sourceTextComponent = highlightedObject.GetComponentInChildren<TextMeshProUGUI>();  // Objeto que brillaba
        if (sourceTextComponent != null)
        {
            Debug.Log("Source TextMeshProUGUI found on " + highlightedObject.name + " with text: " + sourceTextComponent.text);
        }
        else
        {
            Debug.LogError("Source TextMeshProUGUI not found on " + highlightedObject.name);
        }

        // Comprobar si el logger es nulo y obtenerlo si es necesario
        if (logger == null)
        {
            logger = manager.gameObject.GetComponent<EventLogger>();
            if (logger != null)
            {
                Debug.Log("EventLogger found on manager object.");
            }
            else
            {
                Debug.LogError("EventLogger not found on manager object.");
            }
        }

        // Registrar el evento si se encuentra el logger
        if (logger != null && sourceTextComponent != null && targetTextComponent != null)
        {
            logger.LogEvent("Text transferred from " + sourceTextComponent.text + " to " + targetTextComponent.text);
        }

        // Transfiere el texto y el tamaño de la fuente si ambos componentes existen
        if (targetTextComponent != null && sourceTextComponent != null)
        {
            Debug.Log("Transferring text: '" + sourceTextComponent.text + "' and font size: " + sourceTextComponent.fontSize);
            targetTextComponent.text = sourceTextComponent.text;
            targetTextComponent.fontSize = sourceTextComponent.fontSize;
        }
        else
        {
            Debug.LogError("Cannot transfer text or font size because one of the components is missing.");
        }

        // Actualizar el estado del puzzle
        Debug.Log("Updating puzzle state...");
        CheckSolvedStatus();
        infoProcessor.UpdateSolvedCount();
        infoProcessor.CheckAndDeactivateInfoObjects();
        Debug.Log("Puzzle state updated.");
    }


    private void CheckSolvedStatus()
    {
        Debug.Log("entra: CheckSolvedStatus()");
        // Verifica si el texto actual coincide con el texto correcto

        List<string> list = localizeStringEventManager.GetLocalizedStrings();
        Debug.Log("list: " + list.Count);

        foreach (string s in list) {
            Debug.Log("solved: " + ProcessString(GetComponent<TextMeshProUGUI>().text) + " " + ProcessString(s));
            if (ProcessString(GetComponent<TextMeshProUGUI>().text) == ProcessString(s))
            {
                solved = true; break;
            }
            else
            {
                solved = false;
            }
        }

        Debug.Log("solved: " + solved);
    }

    public static string ProcessString(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        // Convertir a minúsculas y eliminar espacios y puntuación
        string lowerCaseString = input.ToLower();
        return Regex.Replace(lowerCaseString, @"[\s\p{P}]", "");
    }

    private void createGrayBox()
    {
        Debug.Log("Entra: createGrayBox()");
        Debug.Log("parentTransform1: createGrayBox()" + transform.parent);
        Debug.Log("parentTransform2: createGrayBox()" + transform.parent.parent);
        // Acceder al padre del objeto actual
        Transform parentTransform = transform.parent.parent;

        if (parentTransform != null)
        {
            Debug.Log("Parent object found: " + parentTransform.name);

            // Comprobar si ya se han creado las grayBoxes
            if (grayBoxes == null || grayBoxes.Length <= 0)
            {
                // Obtener todos los Colliders en el objeto padre
                BoxCollider[] parentColliders = parentTransform.GetComponents<BoxCollider>();

                if (parentColliders.Length > 0)
                {
                    Debug.Log("Found " + parentColliders.Length + " BoxColliders on the parent object.");

                    grayBoxes = new GameObject[parentColliders.Length];

                    for (int i = 0; i < parentColliders.Length; i++)
                    {
                        BoxCollider parentCollider = parentColliders[i];
                        Debug.Log("Creating gray box for collider " + (i + 1) + " with bounds: " + parentCollider.bounds);

                        // Crear una caja gris translúcida por cada Collider en el objeto padre
                        grayBoxes[i] = transform.parent.GetComponent<CreateBoxOnActive>().CreateGrayBox(parentTransform, parentCollider);

                        if (grayBoxes[i] != null)
                        {
                            Debug.Log("Gray box created successfully for collider " + (i + 1));
                        }
                        else
                        {
                            Debug.LogError("Failed to create gray box for collider " + (i + 1));
                        }
                    }
                }
                else
                {
                    Debug.LogWarning("No BoxColliders found on the parent object.");
                }
            }
            else
            {
                Debug.Log("Gray boxes have already been created.");
            }
        }
        else
        {
            Debug.LogWarning("This object has no parent.");
        }
    }

    private void destroyGrayBoxes()
    {
        foreach (var collider in grayBoxes)
        {
            Destroy(collider.gameObject);
        }
    }
}
