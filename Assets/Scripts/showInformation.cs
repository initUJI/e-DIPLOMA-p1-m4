using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class showInformation : MonoBehaviour
{
    public Camera cameraToLookAt;

    private float timeManipulating;
    private float timeRemaining;
    private bool counting;
    public List<GameObject> list;
    private Vector3 pos;
    private GameObject[] infos;

    // Start is called before the first frame update
    void Start()
    {
        timeManipulating = 0.5f;
        timeRemaining = timeManipulating;
        counting = false;
        list = new List<GameObject>();
        pos = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        if (timeRemaining > 0 && counting)
        {
            timeRemaining -= Time.deltaTime;
        }

        /*foreach (GameObject obj in list)
        {
            if (obj.activeInHierarchy)
            {
                Vector3 v = cameraToLookAt.transform.position - obj.transform.position;

                v.x = v.z = 0f;
                obj.transform.LookAt(cameraToLookAt.transform.position - v);
                obj.transform.Rotate(0, 180, 0);
            }
        }*/
    }

    public void startManipulation(GameObject gameObject)
    {
        counting = true;
        pos = gameObject.transform.position;
    }

    public void exitManipulation(GameObject gameObject)
    {
        infos = GameObject.FindGameObjectsWithTag("Info");
        desactiveInfos();

        counting = false;

        if (timeRemaining > 0)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                if (gameObject.transform.GetChild(i).gameObject.name == "Info")
                {
                    if (gameObject.transform.GetChild(i).gameObject.activeInHierarchy)
                    {
                        gameObject.transform.GetChild(i).gameObject.SetActive(false);

                        if (list.Contains(gameObject.transform.GetChild(i).gameObject))
                        {
                            list.Remove(gameObject.transform.GetChild(i).gameObject);
                        }
                    }
                    else
                    {
                        gameObject.transform.GetChild(i).gameObject.SetActive(true);
                        list.Add(gameObject.transform.GetChild(i).gameObject);
                    }
                }
            }
            gameObject.transform.position = pos;
        }

        timeRemaining = timeManipulating;
    }

    public void desactiveInfos()
    {
        foreach (GameObject info in infos)
        {
            info.SetActive(false);
        }
    }
}
