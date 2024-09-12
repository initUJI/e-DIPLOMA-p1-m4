using UnityEngine;
using UnityEngine.EventSystems;

public class ImageOutline : MonoBehaviour, IPointerClickHandler
{
    public Color glowColorTranslucent = new Color(1f, 0f, 0f, 1f);   // Rojo intenso para el brillo del objeto
    public Color glowColorNormal = new Color(0f, 1f, 0f, 1f);        // Verde intenso para el brillo del objeto
    public float translucencyAlpha = 0.5f;           // Nivel de transparencia cuando el sprite es translúcido
    public float boxTranslucencyAlpha = 0.6f;        // Transparencia ajustada para el cubo rojo
    public UICollisionDetector transferredTo;        // Referencia al UICollisionDetector que recibió el texto
    public Manager manager;                          // Referencia al Manager
    public float scaleIncrease = 0.1f;  // Porcentaje para aumentar el tamaño de la caja
    public Material material;

    private bool isGlowing = false;
    private Color originalColor;
    private bool isTranslucentHighlighted = false;    // Nueva bandera para controlar el estado de resaltado rojo
    private GameObject highlightCube;                 // Referencia al cubo de resaltado

    [HideInInspector] public GameObject box;

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
        manager.UnhighlightAll();
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
                HighlightGrandparentColliders(transferredTo.transform.parent.parent, glowColorTranslucent);
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

    public bool CheckIfHighlightedRed()
    {
        // Obtén todos los SpriteRenderers hijos del objeto actual
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null)
            {
                // Verificar si el color actual es el color de brillo rojo
                if (sr.color == glowColorTranslucent)
                {
                    return true;  // Está resaltado en rojo
                }
            }
        }
        return false;  // No está resaltado en rojo
    }

    public void RemoveRedGlow()
    {
        Debug.Log("Removing red glow from children");

        // Obtén todos los SpriteRenderers hijos del objeto actual
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
                    sr.color = originalColor;  // O hacer el sprite translúcido según tu lógica

                    // Si el objeto estaba iluminado en rojo, asegúrate de marcarlo como no translúcido
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

        // Marcar el objeto como no iluminado
        isGlowing = false;

        ApplyGlow(glowColorTranslucent);
        isTranslucentHighlighted = true;
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

    public void HighlightGrandparentColliders(Transform grandparent, Color color)
    {
        Debug.Log($"Highlighting grandparent colliders for {grandparent.gameObject.name}");

        // Obtener todos los BoxColliders del abuelo
        BoxCollider[] boxColliders = grandparent.GetComponents<BoxCollider>();

        if (boxColliders.Length > 0)
        {
            foreach (BoxCollider boxCollider in boxColliders)
            {
                Debug.Log($"BoxCollider found for {grandparent.gameObject.name}, applying highlight");

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
            Debug.LogWarning("No BoxColliders found on grandparent");
        }
    }
    private void CreateBox(Transform parent, BoxCollider parentCollider)
    {
        // Crear un cubo que coincida con el BoxCollider del padre
        highlightCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        highlightCube.name = "RedHighlightBox";

        // Desactivar el colisionador del cubo para que no interfiera con la física
        Destroy(highlightCube.GetComponent<BoxCollider>());

        // Hacer que el cubo sea hijo del padre
        highlightCube.transform.SetParent(parent, false);

        // Ajustar el cubo al tamaño y posición del BoxCollider
        highlightCube.transform.localPosition = parentCollider.center;

        // Ajustar el tamaño del cubo para que sea ligeramente más grande que el BoxCollider
        Vector3 originalSize = parentCollider.size;
        highlightCube.transform.localScale = originalSize * (1 + scaleIncrease);

        // Aplicar un material azul translúcido
        MeshRenderer renderer = highlightCube.GetComponent<MeshRenderer>();
        /*Material translucentMaterial = new Material(Shader.Find("Standard"));

        // Configurar el material para soporte de transparencia
        translucentMaterial.SetFloat("_Mode", 3); // Modo de transparencia
        translucentMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        translucentMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        translucentMaterial.SetInt("_ZWrite", 0);
        translucentMaterial.DisableKeyword("_ALPHATEST_ON");
        translucentMaterial.EnableKeyword("_ALPHABLEND_ON");
        translucentMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        translucentMaterial.renderQueue = 3000;

        // Asignar el color azul con la transparencia ajustada
        translucentMaterial.color = new Color(1f, 0f, 1f, boxTranslucencyAlpha);  // Azul con transparencia

        renderer.material = translucentMaterial;*/
        renderer.material = material;
        box = highlightCube;
    }

}