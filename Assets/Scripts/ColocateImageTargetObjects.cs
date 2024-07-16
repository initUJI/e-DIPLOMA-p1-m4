using UnityEngine;

public class ColocateImageTargetObjects : MonoBehaviour
{
    public float offsetX = 0.0f;
    public float offsetY = 0.0f;
    public float offsetZ = 0.0f;

    void Start()
    {
        //ApplyOffsetsToChildren();
    }

    public  void ApplyOffsetsToChildren()
    {
        foreach (Transform child in transform)
        {
            ApplyOffset(child);
        }
    }

    void ApplyOffset(Transform child)
    {
        Vector3 newPosition = child.position;
        newPosition.x += offsetX;
        newPosition.y += offsetY;
        newPosition.z += offsetZ;
        child.position = newPosition;
    }
}
