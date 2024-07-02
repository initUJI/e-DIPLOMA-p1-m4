using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    //private Manager manager;

    private void Start()
    {
        //manager = FindObjectOfType<Manager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable") /*&& !manager.IsObjectManipulated()*/)
        {
            //Debug.Log("Colisi�n detectada con la punta del dedo �ndice");
            PerformActionEnter(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable")/* && !manager.IsObjectManipulated()*/)
        {
            //Debug.Log("Colisi�n detectada con la punta del dedo �ndice");
            PerformActionExit(other.gameObject);
        }
    }

    private void PerformActionEnter(GameObject colliding)
    {
        // Acci�n personalizada al detectar la colisi�n con la punta del dedo �ndice
        //Debug.Log("Acci�n personalizada ejecutada");
        /*if (manager.GetComponent<showInformation>() != null)
        {
            manager.GetComponent<showInformation>().startManipulation(colliding);
        }*/
    }

    private void PerformActionExit(GameObject colliding)
    {
        // Acci�n personalizada al detectar la colisi�n con la punta del dedo �ndice
        //Debug.Log("Acci�n personalizada ejecutada");
        /*if (manager.GetComponent<showInformation>() != null)
        {
            manager.GetComponent<showInformation>().exitManipulation(colliding);
        }*/
    }
}