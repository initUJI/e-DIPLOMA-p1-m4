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
            //Debug.Log("Colisión detectada con la punta del dedo índice");
            PerformActionEnter(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable")/* && !manager.IsObjectManipulated()*/)
        {
            //Debug.Log("Colisión detectada con la punta del dedo índice");
            PerformActionExit(other.gameObject);
        }
    }

    private void PerformActionEnter(GameObject colliding)
    {
        // Acción personalizada al detectar la colisión con la punta del dedo índice
        //Debug.Log("Acción personalizada ejecutada");
        /*if (manager.GetComponent<showInformation>() != null)
        {
            manager.GetComponent<showInformation>().startManipulation(colliding);
        }*/
    }

    private void PerformActionExit(GameObject colliding)
    {
        // Acción personalizada al detectar la colisión con la punta del dedo índice
        //Debug.Log("Acción personalizada ejecutada");
        /*if (manager.GetComponent<showInformation>() != null)
        {
            manager.GetComponent<showInformation>().exitManipulation(colliding);
        }*/
    }
}