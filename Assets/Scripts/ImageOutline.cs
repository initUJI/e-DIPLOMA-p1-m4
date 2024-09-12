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

        // Marcar el objeto como iluminado
        isGlowing = true;
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

        // Marcar el objeto como no iluminado
        isGlowing = false;

        ApplyGlow(glowColorTranslucent);
        isTranslucentHighlighted = true;
    }

    // Funci�n para hacer las im�genes transl�cidas
    public void MakeTranslucent()
    {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            if (sr != null)
            {
                Debug.Log($"Making {sr.gameObject.name} translucent");

                // Cambiar el color del SpriteRenderer para hacer el sprite transl�cido
                Color currentColor = Color.white;
                currentColor.a = translucencyAlpha;  // Ajustar el alfa para hacer el sprite transl�cido
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

        // Desactivar el colisionador del cubo para que no interfiera con la f�sica
        Destroy(highlightCube.GetComponent<BoxCollider>());

        // Hacer que el cubo sea hijo del padre
        highlightCube.transform.SetParent(parent, false);

        // Ajustar el cubo al tama�o y posici�n del BoxCollider
        highlightCube.transform.localPosition = parentCollider.center;

        // Ajustar el tama�o del cubo para que sea ligeramente m�s grande que el BoxCollider
        Vector3 originalSize = parentCollider.size;
        highlightCube.transform.localScale = originalSize * (1 + scaleIncrease);

        // Aplicar un material azul transl�cido
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