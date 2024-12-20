using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageOutline : MonoBehaviour, IPointerClickHandler
{
    public Color glowColorTranslucent = new Color(1f, 0f, 0f, 1f);   // Rojo intenso para el brillo del objeto
    public Color glowColorNormal = new Color(0f, 1f, 0f, 1f);        // Verde intenso para el brillo del objeto
    public float translucencyAlpha = 0.5f;           // Nivel de transparencia cuando el sprite es transl�cido
    public float boxTranslucencyAlpha = 0.6f;        // Transparencia ajustada para el cubo rojo
    public UICollisionDetector transferredTo;        // Referencia al UICollisionDetector que recibi� el texto
    public Manager manager;                          // Referencia al Manager
    public float scaleIncrease = 0.1f;  // Porcentaje para aumentar el tama�o de la caja
    public Material material;

    private Color originalColor;
    private bool isTranslucentHighlighted = false;    // Nueva bandera para controlar el estado de resaltado rojo
    private GameObject highlightCube;                 // Referencia al cubo de resaltado

    [HideInInspector] public GameObject box;

    private void Start()
    {
        // Guardar el color original del SpriteRenderer
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            originalColor = sr.color;
        }
    }
    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("AppFirstOpen");
        PlayerPrefs.Save();
    }
    public void RestoreOriginalState()
    {
        Debug.Log("Restoring to original state");

        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null)
            {
                Debug.Log($"Restoring {sr.gameObject.name} to original color");

                // Restaurar el color original del sprite
                sr.color = originalColor;
            }
        }

        // Marcar el objeto como no transl�cido
        isTranslucentHighlighted = false;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        manager.UnhighlightAll();
        Debug.Log("OnPointerClick - ImageOutline clicked");

        // Verificar si el manager est� asignado
        if (manager == null)
        {
            Debug.LogError("Manager is not assigned!");
            return;
        }

        // Verificar si las im�genes del objeto son transl�cidas
        bool isTranslucent = CheckIfTranslucent();

        // Si el objeto es transl�cido, debe iluminarse en rojo
        if (isTranslucent)
        {
            Debug.Log("Object is translucent");
            ApplyGlow(glowColorTranslucent);
            isTranslucentHighlighted = true;

            // Si tiene un UICollisionDetector asociado, iluminar el abuelo en rojo
            if (transferredTo != null)
            {
                HighlightGrandparentColliders(transferredTo.transform.parent.parent, glowColorTranslucent);
            }
            return;
        }

        // Si no es transl�cido, aplicar brillo verde y notificar al Manager
        Debug.Log("Object is not translucent");
        if (!isTranslucentHighlighted)
        {
            ApplyGlow(glowColorNormal);
            manager.SetCurrentlyHighlighted(gameObject);  // Notificar al Manager para guardar este objeto como resaltado
        }
    }


    // Funci�n para verificar si las im�genes en los hijos son transl�cidas
    public bool CheckIfTranslucent()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null && sr.color.a < 1f)
            {
                Debug.Log($"Image {sr.gameObject.name} is translucent (alpha < 1)");
                return true;  // El objeto es transl�cido si el alfa es menor a 1
            }
        }
        return false;  // Si ninguna imagen es transl�cida, retornar false
    }

    // Funci�n para aplicar brillo a los SpriteRenderer en los hijos
    public void ApplyGlow(Color glowColor)
    {
        Debug.Log("Applying glow to children: " + glowColor.ToString());

        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null)
            {
                Debug.Log($"Applying glow to {sr.gameObject.name}");

                // Cambiar el color del SpriteRenderer directamente
                sr.color = glowColor;
            }
        }
    }

    public void toOriginalColor()
    {
        Debug.Log("toOriginalColor(): "+ transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);

        var spriteRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        Debug.Log($"toOriginalColor: {originalColor}");
        Debug.Log($"SpriteRenderer: {spriteRenderer.color}");

        spriteRenderer.color= Color.white;

        Debug.Log($"toOriginalColor: {originalColor}");
        Debug.Log($"SpriteRenderer: {spriteRenderer.color}");

        // Forzar la actualizaci�n del SpriteRenderer
        spriteRenderer.enabled = false;
        spriteRenderer.enabled = true;

        // Desactivar y reactivar el objeto completo
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);

        // Forzar la actualizaci�n del objeto
        Canvas.ForceUpdateCanvases();
        ForceRenderUpdate();
        Camera.main.Render(); // Forzar el renderizado de la c�mara principal
        Debug.Log($"toOriginalColor: {originalColor}");
        Debug.Log($"SpriteRenderer: {spriteRenderer.color}");
    }

    void ForceRenderUpdate()
    {
        // Forzar un nuevo frame de renderizado
        Application.targetFrameRate = 60; // Asegura un framerate para forzar los gr�ficos
        QualitySettings.vSyncCount = 0;  // Asegura que no haya l�mites de vSync
        Debug.Log("Forzando actualizaci�n de gr�ficos en el pr�ximo frame.");
    }


    // Funci�n para quitar el brillo de los SpriteRenderer en los hijos
    public void RemoveGlow()
    {
        Debug.Log("Removing glow from children");

        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null)
            {
                Debug.Log($"Removing glow from {sr.gameObject.name}");

                // Si el color actual es rojo (glowColorTranslucent), volver al estado transl�cido
                if (sr.color == glowColorTranslucent)
                {
                    Debug.Log($"Returning {sr.gameObject.name} to translucent state");
                    Color translucentColor = sr.color;
                    translucentColor.a = translucencyAlpha;  // Ajustar el alfa para hacer el sprite transl�cido
                    sr.color = translucentColor;  // Aplicar el color transl�cido
                }
                else
                {
                    // Restaurar el color original del sprite
                    Debug.Log($"Color: {originalColor}");
                    sr.color = originalColor;  // Volver al color original
                }
            }
        }

        // Eliminar el cubo de resaltado si existe
        if (highlightCube != null)
        {
            Destroy(highlightCube);
            highlightCube = null;
        }
    }

    public bool CheckIfHighlightedRed()
    {
        // Obt�n todos los SpriteRenderers hijos del objeto actual
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null)
            {
                // Verificar si el color actual es el color de brillo rojo
                if (sr.color == glowColorTranslucent)
                {
                    return true;  // Est� resaltado en rojo
                }
            }
        }
        return false;  // No est� resaltado en rojo
    }

    public void RemoveRedGlow()
    {
        Debug.Log("Removing red glow from children");

        // Obt�n todos los SpriteRenderers hijos del objeto actual
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null)
            {
                // Si el color actual es el color de brillo rojo
                if (sr.color == glowColorTranslucent)
                {
                    Debug.Log($"Removing red glow from {sr.gameObject.name}");

                    // Restaurar el color original del sprite
                    sr.color = originalColor;  // O hacer el sprite transl�cido seg�n tu l�gica

                    // Si el objeto estaba iluminado en rojo, aseg�rate de marcarlo como no transl�cido
                    isTranslucentHighlighted = false;
                }
            }
        }

        // Eliminar el cubo de resaltado si existe
        if (highlightCube != null)
        {
            Destroy(highlightCube);
            highlightCube = null;
        }

        ApplyGlow(glowColorTranslucent);
        isTranslucentHighlighted = true;
    }

    // Funci�n para hacer las im�genes transl�cidas
    public void MakeTranslucent()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        Debug.Log("MakeTranslucent():" + spriteRenderers.Length);
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null)
            {
                Debug.Log($"Making {sr.gameObject.name} translucent");

                Color c = Color.white;
                c.a = c.a / 2;
                sr.color =c;  // Aplicar el color con el nuevo alfa
            }
        }
    }

    public void HighlightGrandparentColliders(Transform grandparent, Color color)
    {
        //Debug.Log($"Highlighting grandparent colliders for {grandparent.gameObject.name}");

        // Obtener todos los BoxColliders del abuelo
        BoxCollider[] boxColliders = grandparent.GetComponents<BoxCollider>();

        if (boxColliders.Length > 0)
        {
            foreach (BoxCollider boxCollider in boxColliders)
            {
                //Debug.Log($"BoxCollider found for {grandparent.gameObject.name}, applying highlight");

                // Verifica si ya existe un cubo de resaltado para este BoxCollider
                string highlightName = $"HighlightCube_{boxCollider.GetInstanceID()}";
                Transform highlight = grandparent.Find(highlightName);

                if (highlight == null)
                {
                    // Crear un cubo que coincida con el BoxCollider
                    CreateBox(grandparent, boxCollider);
                }
                else
                {
                    // Si el cubo ya existe, solo cambiar el color
                    MeshRenderer renderer = highlight.GetComponent<MeshRenderer>();
                    renderer.material.color = color;
                }
            }
        }
        else
        {
           // Debug.LogWarning("No BoxColliders found on grandparent");
        }
    }
    private void CreateBox(Transform parent, BoxCollider parentCollider)
    {
        // Crear un cubo que coincida con el BoxCollider del padre
        highlightCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        highlightCube.name = "RedHighlightBox";

        // Desactivar el colisionador del cubo para que no interfiera con la f�sica
        Destroy(highlightCube.GetComponent<BoxCollider>());

        // Hacer que el cubo sea hijo del padre
        highlightCube.transform.SetParent(parent, false);

        // Ajustar el cubo al tama�o y posici�n del BoxCollider
        highlightCube.transform.localPosition = parentCollider.center;
        highlightCube.transform.localScale = parentCollider.size * (1 + scaleIncrease);

        // Aplicar un material azul transl�cido
        MeshRenderer renderer = highlightCube.GetComponent<MeshRenderer>();
        renderer.material = material;

        // Forzar que el cubo sea el �ltimo en renderizarse
        highlightCube.transform.SetAsLastSibling();

        // Guardar la referencia
        box = highlightCube;
    }
}