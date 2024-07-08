using UnityEngine;
using Vuforia;

public class InstantiateOnModelTarget : MonoBehaviour
{
    public GameObject prefabToInstantiate;
    private GameObject instantiatedObject;
    private ModelTargetBehaviour modelTargetBehaviour;

    private void Start()
    {
        modelTargetBehaviour = GetComponent<ModelTargetBehaviour>();
    }

    public void OnTrackingFound()
    {
        if (prefabToInstantiate != null && instantiatedObject == null)
        {
            instantiatedObject = Instantiate(prefabToInstantiate, modelTargetBehaviour.transform.position, prefabToInstantiate.transform.rotation);

            instantiatedObject.transform.position = modelTargetBehaviour.transform.position;
            instantiatedObject.transform.position = new Vector3(instantiatedObject.transform.position.y + prefabToInstantiate.transform.position.x,
                instantiatedObject.transform.position.y, instantiatedObject.transform.position.z);
            instantiatedObject.transform.parent = modelTargetBehaviour.transform;
        }
    }

    public void OnTrackingLost()
    {
        if (instantiatedObject != null)
        {
            Destroy(instantiatedObject);
        }
    }
}
