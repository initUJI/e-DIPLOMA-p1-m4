using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class constraintMovement : MonoBehaviour
{
    public void constraintRotation(GameObject gameObject)
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void releaseRotation(GameObject gameObject)
    {
        gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }
}
