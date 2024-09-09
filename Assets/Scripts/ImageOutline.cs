using UnityEngine;
using UnityEngine.EventSystems;

public class ImageOutline : MonoBehaviour, IPointerClickHandler
{
    public Color glowColorTranslucent = new Color(1f, 0f, 0f, 1f);   // Rojo intenso para el brillo del objeto
    public Color glowColorNormal = new Color(0f, 1f, 0f, 1f);        // Verde intenso para el brillo del objeto
    public float translucencyAlpha = 0.5f;           // Nivel de transparencia cuando el sprite es translúcido
    public float boxTranslucencyAlpha = 0.3f;        // Transparencia ajustada para el cubo rojo
    public UICollisionDetector transferredTo;        // Referencia al UICollisionDetector que recibió el texto
    public Manager manager;                          // Referencia al Manager

    private bool isGlowing = false;
    private Color originalColor;
    private bool isTranslucentHighlighted = false;    // Nueva bandera para controlar el estado de resaltado rojo
    private GameObject highlightCube;                 // Referencia al cubo de resaltado

    private void Start()
    {
        manager = FindObjectOfType<Manager>();  // Encuentra el Manager en la escena
        // Guardar el color original del SpriteRenderer
        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
        {
            originalColor = sr.color;
        }
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

        // Marcar el objeto como no translúcido
        isTranslucentHighlighted = false;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("OnPointerClick - ImageOutline clicked");

        // Verificar si el manager está asignado
        if (manager == null)
        {
            Debug.LogError("Manager is not assigned!");
            return;
        }

        // Verificar si las imágenes del objeto son translúcidas
        bool isTranslucent = CheckIfTranslucent();

        // Si el objeto es translúcido, debe iluminarse en rojo
        if (isTranslucent)
        {
            Debug.Log("Object is translucent");
            ApplyGlow(glowColorTranslucent);
            isTranslucentHighlighted = true;

            // Si tiene un UICollisionDetector asociado, iluminar el abuelo en rojo
            if (transferredTo != null)
            {
                HighlightGrandparentCollider(transferredTo.transform.parent.parent, glowColorTranslucent);
            }
            return;
        }

        // Si no es translúcido, aplicar brillo verde y notificar al Manager
        Debug.Log("Object is not translucent");
        if (!isTranslucentHighlighted)
        {
            ApplyGlow(glowColorNormal);
            manager.SetCurrentlyHighlighted(gameObject);  // Notificar al Manager para guardar este objeto como resaltado
        }
    }


    // Función para verificar si las imágenes en los hijos son translúcidas
    public bool CheckIfTranslucent()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null && sr.color.a < 1f)
            {
                Debug.Log($"Image {sr.gameObject.name} is translucent (alpha < 1)");
                return true;  // El objeto es translúcido si el alfa es menor a 1
            }
        }
        return false;  // Si ninguna imagen es translúcida, retornar false
    }

    // Función para aplicar brillo a los SpriteRenderer en los hijos
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

        // Marcar el objeto como iluminado
        isGlowing = true;
    }

    // Función para quitar el brillo de los SpriteRenderer en los hijos
    public void RemoveGlow()
    {
        Debug.Log("Removing glow from children");

        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null)
            {
                Debug.Log($"Removing glow from {sr.gameObject.name}");

                // Si el color actual es rojo (glowColorTranslucent), volver al estado translúcido
                if (sr.color == glowColorTranslucent)
                {
                    Debug.Log($"Returning {sr.gameObject.name} to translucent state");
                    Color translucentColor = sr.color;
                    translucentColor.a = translucencyAlpha;  // Ajustar el alfa para hacer el sprite translúcido
                    sr.color = translucentColor;  // Aplicar el color translúcido
                }
                else
                {
                    // Restaurar el color original del sprite
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

        // Marcar el objeto como no iluminado
        isGlowing = false;
    }


    // Función para hacer las imágenes translúcidas
    public void MakeTranslucent()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null)
            {
                Debug.Log($"Making {sr.gameObject.name} translucent");

                // Cambiar el color del SpriteRenderer para hacer el sprite translúcido
                Color currentColor = Color.white;
                currentColor.a = translucencyAlpha;  // Ajustar el alfa para hacer el sprite translúcido
                sr.color = currentColor;  // Aplicar el color con el nuevo alfa
            }
        }
    }

    public void HighlightGrandparentCollider(Transform grandparent, Color color)
    {
        Debug.Log($"Highlighting grandparent collider for {grandparent.gameObject.name}");

        BoxCollider boxCollider = grandparent.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            Debug.Log($"BoxCollider found for {grandparent.gameObject.name}, applying highlight");

            // Verifica si ya existe un cubo de resaltado como hijo
            Transform highlight = grandparent.Find("HighlightCube");
            if (highlight == null)
            {
                // Crear un cubo que coincida con el BoxCollider
                highlightCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                highlightCube.name = "HighlightCube";

                // Desactiva el colisionador del cubo para que no interfiera con la física
                Destroy(highlightCube.GetComponent<BoxCollider>());

                // Haz que el cubo sea hijo del abuelo del UICollisionDetector
                highlightCube.transform.SetParent(grandparent, false);

                // Ajustar el cubo al tamaño y posición del BoxCollider
                highlightCube.transform.localPosition = boxCollider.center;
                highlightCube.transform.localScale = boxCollider.size;

                // Aplicar un material translúcido al cubo
                MeshRenderer renderer = highlightCube.GetComponent<MeshRenderer>();
                Material translucentMaterial = new Material(Shader.Find("Standard"));

                // Configurar el modo de renderizado a "Fade" para permitir transparencias
                translucentMaterial.SetFloat("_Mode", 2);  // Modo "Fade"
                translucentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                translucentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                translucentMaterial.SetInt("_ZWrite", 0);
                translucentMaterial.DisableKeyword("_ALPHATEST_ON");
                translucentMaterial.EnableKeyword("_ALPHABLEND_ON");
                translucentMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                translucentMaterial.renderQueue = 3000;  // Asegurar la transparencia en la cola de renderizado

                // Asignar el color con la transparencia ajustada
                translucentMaterial.color = new Color(color.r, color.g, color.b, boxTranslucencyAlpha);  // Aplicar transparencia reducida
                renderer.material = translucentMaterial;
            }
            else
            {
                // Si el cubo ya existe, solo cambiar el color
                MeshRenderer renderer = highlight.GetComponent<MeshRenderer>();
                renderer.material.color = new Color(color.r, color.g, color.b, boxTranslucencyAlpha); // Aplicar transparencia reducida
            }
        }
        else
        {
            Debug.LogWarning("BoxCollider not found on grandparent");
        }
    }
}