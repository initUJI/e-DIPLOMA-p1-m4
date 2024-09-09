using UnityEngine;

public class CreateBlueBoxOnActive : MonoBehaviour
{
    public float scaleIncrease = 0.1f;  // Porcentaje para aumentar el tamaño de la caja
    public float boxTransparency = 0.5f; // Nivel de transparencia de la caja azul
    private GameObject blueBox;  // Referencia al objeto de la caja azul

    void OnEnable()
    {
        // Acceder al padre del objeto actual
        Transform parentTransform = transform.parent;

        if (parentTransform != null)
        {
            BoxCollider parentCollider = parentTransform.GetComponent<BoxCollider>();

            if (parentCollider != null)
            {
                // Crear una caja azul translúcida que sea ligeramente más grande que el BoxCollider del padre
                CreateBlueBox(parentTransform, parentCollider);
            }
            else
            {
                Debug.LogWarning("No BoxCollider found on the parent object.");
            }
        }
        else
        {
            Debug.LogWarning("This object has no parent.");
        }
    }

    void OnDisable()
    {
        // Eliminar la caja azul cuando el objeto se desactive
        if (blueBox != null)
        {
            Destroy(blueBox);
            blueBox = null;
        }
    }

    private void CreateBlueBox(Transform parent, BoxCollider parentCollider)
    {
        // Crear un cubo que coincida con el BoxCollider del padre
        blueBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        blueBox.name = "BlueHighlightBox";

        // Desactivar el colisionador del cubo para que no interfiera con la física
        Destroy(blueBox.GetComponent<BoxCollider>());

        // Hacer que el cubo sea hijo del padre
        blueBox.transform.SetParent(parent, false);

        // Ajustar el cubo al tamaño y posición del BoxCollider
        blueBox.transform.localPosition = parentCollider.center;

        // Ajustar el tamaño del cubo para que sea ligeramente más grande que el BoxCollider
        Vector3 originalSize = parentCollider.size;
        blueBox.transform.localScale = originalSize * (1 + scaleIncrease);

        // Aplicar un material azul translúcido
        MeshRenderer renderer = blueBox.GetComponent<MeshRenderer>();
        Material translucentMaterial = new Material(Shader.Find("Standard"));

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
        translucentMaterial.color = new Color(0f, 0f, 1f, boxTransparency);  // Azul con transparencia

        renderer.material = translucentMaterial;
    }
}
