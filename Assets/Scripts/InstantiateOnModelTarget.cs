using UnityEngine;
using Vuforia;

public class InstantiateOnModelTarget : MonoBehaviour
{
    public GameObject prefabToInstantiate;
    public GameObject manager;
    private GameObject instantiatedObject;
    private ModelTargetBehaviour modelTargetBehaviour;

    private void Start()
    {
        modelTargetBehaviour = GetComponent<ModelTargetBehaviour>();
    }

    private void OnTrackingFound()
    {
        if (prefabToInstantiate != null)
        {
            if (instantiatedObject != null)
            {
                Destroy(instantiatedObject);
            }

            instantiatedObject = Instantiate(prefabToInstantiate);
            instantiatedObject.transform.SetParent(modelTargetBehaviour.transform, false);

            // Aplicar la posición y rotación local del prefab
            instantiatedObject.transform.localPosition = prefabToInstantiate.transform.localPosition;
            instantiatedObject.transform.localRotation = prefabToInstantiate.transform.localRotation;

            // Desvincula el objeto instanciado del ModelTargetBehaviour para que mantenga su posición
            //instantiatedObject.transform.SetParent(null, true);
        }
    }

    public void OnTrackingLost()
    {
        if (instantiatedObject != null)
        {
            //instantiatedObject.transform.parent = manager.transform;
        }
    }
}
