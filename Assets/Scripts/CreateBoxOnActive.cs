using UnityEngine;

public class CreateBoxOnActive : MonoBehaviour
{
    private float scaleIncrease = 0.1f;  // Porcentaje para aumentar el tamaño de la caja
    public float boxTransparency = 0.5f; // Nivel de transparencia de la caja azul
    private GameObject blueBox;  // Referencia al objeto de la caja azul
    private GameObject[] blueBoxes;  // Referencia a las cajas azules

    public Material blueMaterial;
    public Material grayMaterial;

    void OnEnable()
    {
        // Acceder al padre del objeto actual
        Transform parentTransform = transform.parent;

        if (parentTransform != null)
        {
            // Obtener todos los Colliders en el objeto padre
            BoxCollider[] parentColliders = parentTransform.GetComponents<BoxCollider>();

            if (parentColliders.Length > 0)
            {
                blueBoxes = new GameObject[parentColliders.Length];

                for (int i = 0; i < parentColliders.Length; i++)
                {
                    BoxCollider parentCollider = parentColliders[i];
                    // Crear una caja azul translúcida por cada Collider en el objeto padre
                    blueBoxes[i] = CreateBlueBox(parentTransform, parentCollider);
                }
            }
            else
            {
                Debug.LogWarning("No Colliders found on the parent object.");
            }
        }
        else
        {
            Debug.LogWarning("This object has no parent.");
        }
    }

    void OnDisable()
    {
        // Eliminar todas las cajas azules cuando el objeto se desactive
        if (blueBoxes != null)
        {
            foreach (GameObject box in blueBoxes)
            {
                if (box != null)
                {
                    Destroy(box);
                }
            }
            blueBoxes = null;
        }
    }

    private GameObject CreateBlueBox(Transform parent, BoxCollider parentCollider)
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

        renderer.material = blueMaterial;

        // Forzar que el cubo sea el último en renderizarse
        blueBox.transform.SetAsLastSibling();

        return blueBox;
    }

    public GameObject CreateGrayBox(Transform parent, BoxCollider parentCollider)
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

        renderer.material = grayMaterial;

        // Forzar que el cubo sea el último en renderizarse
        blueBox.transform.SetAsLastSibling();

        return blueBox;
    }
}
