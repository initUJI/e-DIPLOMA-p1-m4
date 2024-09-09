using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;

public class UICollisionDetector : MonoBehaviour, IPointerClickHandler
{
    public Manager manager;  // Referencia al Manager para gestionar el objeto brillando actualmente
    private string correctText;  // Texto correcto con el que comparar
    public InfoProcessor infoProcessor;  // Referencia al InfoProcessor para actualizaciones de estado

    private EventLogger logger;

    [HideInInspector]
    public bool solved;

    private void Start()
    {
        logger = FindFirstObjectByType<EventLogger>();
        solved = false;
        correctText = GetComponent<TextMeshProUGUI>().text;
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
            if (!string.IsNullOrEmpty(targetTextComponent.text) && targetTextComponent.text != correctText)
            {
                // Buscar el objeto que tenía originalmente el texto
                ImageOutline originalOutline = manager.FindImageOutlineByText(targetTextComponent.text);
                if (originalOutline != null)
                {
                    // Si el objeto original está en estado translúcido, restaurarlo
                    originalOutline.RemoveGlow();
                }
            }

            // Si hay un objeto brillando, transferir su texto al objeto clicado (este)
            TransferTextAndFontSize(highlightedObject);

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
        TextMeshProUGUI targetTextComponent = GetComponent<TextMeshProUGUI>();  // Este objeto
        TextMeshProUGUI sourceTextComponent = highlightedObject.GetComponentInChildren<TextMeshProUGUI>();  // Objeto que brillaba

        if (logger == null)
        {
            logger = manager.gameObject.GetComponent<EventLogger>();
        }

        if (logger != null)
        {
            logger.LogEvent("Text transferred from " + sourceTextComponent.text + " to " + targetTextComponent.text);
        }

        // Transfiere el texto y el tamaño de la fuente
        targetTextComponent.text = sourceTextComponent.text;
        targetTextComponent.fontSize = sourceTextComponent.fontSize;

        // Actualizar el estado del puzzle
        CheckSolvedStatus();
        infoProcessor.UpdateSolvedCount();
        infoProcessor.UpdateStatusText();
        infoProcessor.CheckAndDeactivateInfoObjects();
    }

    private void CheckSolvedStatus()
    {
        // Verifica si el texto actual coincide con el texto correcto
        solved = ProcessString(GetComponent<TextMeshProUGUI>().text) == ProcessString(correctText);
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
}
