using UnityEngine;

public class MultiAxisLocker : MonoBehaviour
{
    [System.Flags]
    public enum Axis
    {
        None = 0,
        X = 1 << 0,
        Y = 1 << 1,
        Z = 1 << 2
    }

    public Axis lockAxes = Axis.None; // Ejes a bloquear especificados en el inspector

    private Vector3 initialLocalPosition;

    void Start()
    {
        initialLocalPosition = transform.localPosition; // Guardar la posición local inicial del objeto
    }

    void Update()
    {
        Vector3 newLocalPosition = transform.localPosition;

        if ((lockAxes & Axis.X) == Axis.X)
        {
            newLocalPosition.x = initialLocalPosition.x;
        }

        if ((lockAxes & Axis.Y) == Axis.Y)
        {
            newLocalPosition.y = initialLocalPosition.y;
        }

        if ((lockAxes & Axis.Z) == Axis.Z)
        {
            newLocalPosition.z = initialLocalPosition.z;
        }

        transform.localPosition = newLocalPosition;
    }
}
