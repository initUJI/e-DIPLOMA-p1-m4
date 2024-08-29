using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public GameObject optionPrefab;
    public float moveStep = 0.005f;
    public float checkDelay = 0.001f;
    private EventLogger logger;

    public TextMeshProUGUI textMeshProUGUIID;
    public TMP_InputField tMP_InputFieldID;
    private void Start()
    {
        logger = FindObjectOfType<EventLogger>();
        InitializeAllInfoProcessors();
    }

    void InitializeAllInfoProcessors()
    {
        // Encuentra todos los objetos con el tag "ModelTarget"
        GameObject[] modelTargets = GameObject.FindGameObjectsWithTag("ModelTarget");

        // Lista para almacenar todos los objetos hijos
        List<GameObject> allChildObjects = new List<GameObject>();

        // Recorre cada objeto ModelTarget y sus hijos para agregar a la lista
        foreach (GameObject modelTarget in modelTargets)
        {
            GetChildren(modelTarget, allChildObjects);
        }

        // Itera sobre cada objeto hijo y busca componentes InfoProcessor
        foreach (GameObject obj in allChildObjects)
        {
            InfoProcessor infoProcessor = obj.GetComponent<InfoProcessor>();
            if (infoProcessor != null)
            {
                // Llama a la funci�n InitializeInfoObjects pasando el primer hijo del GameObject
                if (infoProcessor.gameObject.transform.childCount > 0)
                {
                    Transform firstChild = infoProcessor.gameObject.transform.GetChild(0);
                    infoProcessor.InitializeInfoObjects(firstChild);
                }
            }
        }
    }

    void GetChildren(GameObject obj, List<GameObject> allObjects)
    {
        // A�ade el objeto actual a la lista
        allObjects.Add(obj);

        // Recorre cada hijo y llama recursivamente a esta funci�n
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            GetChildren(obj.transform.GetChild(i).gameObject, allObjects);
        }
    }

    public void CreateOptionName(TextMeshProUGUI text, Transform initialPos)
    {
        GameObject optionInstance = InstantiateOption(initialPos);
        SetOptionText(optionInstance, text);
        StartCoroutine(MoveUpUntilNoCollision(optionInstance));
    }

    private GameObject InstantiateOption(Transform initialPos)
    {
        // Instantiate the prefab at the initial position
        return Instantiate(optionPrefab, initialPos.position, initialPos.rotation);
    }

    private void SetOptionText(GameObject optionInstance, TextMeshProUGUI text)
    {
        // Set the text in the TextMeshProUGUI component
        optionInstance.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = text.text;
    }

    private IEnumerator MoveUpUntilNoCollision(GameObject optionInstance)
    {
        Collider optionCollider = GetOptionCollider(optionInstance);
        if (optionCollider == null)
        {
            yield break;
        }

        float initialYPosition = optionInstance.transform.position.y;
        float maxHeight = 0.5f;

        while (IsColliding(optionCollider, optionInstance.transform.rotation) && !HasReachedMaxHeight(optionInstance.transform.position.y, initialYPosition, maxHeight))
        {
            MoveOptionUp(optionInstance);
            yield return new WaitForSeconds(checkDelay);
        }
    }

    private Collider GetOptionCollider(GameObject optionInstance)
    {
        Collider collider = optionInstance.GetComponent<Collider>();
        if (collider == null)
        {
            Debug.LogError("The prefab does not have a Collider.");
        }
        return collider;
    }

    private bool IsColliding(Collider optionCollider, Quaternion rotation)
    {
        Vector3 currentCenter = optionCollider.bounds.center;
        Vector3 currentExtents = optionCollider.bounds.extents;

        Collider[] hitColliders = Physics.OverlapBox(currentCenter, currentExtents, rotation);

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider != optionCollider)
            {
                Debug.Log($"Collision detected with: {hitCollider.gameObject.name}");
                return true;
            }
        }
        return false;
    }

    private bool HasReachedMaxHeight(float currentY, float initialY, float maxHeight)
    {
        if (currentY - initialY >= maxHeight)
        {
            Debug.LogWarning("Maximum allowed height reached.");
            return true;
        }
        return false;
    }

    private void MoveOptionUp(GameObject optionInstance)
    {
        // Move the option instance considering its rotation
        optionInstance.transform.position += optionInstance.transform.up * moveStep;
    }

    public void ReloadCurrentScene()
    {
        logger.LogEvent("Reload scene button pressed");
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void infoPanel(GameObject infoPanel)
    {
        if (infoPanel.activeInHierarchy)
        {
            infoPanel.SetActive(false);
        }
        else
        {
            infoPanel.SetActive(true);
        }

        logger.LogEvent(infoPanel.name + " button pressed. Info panel active: " + infoPanel.activeInHierarchy);
    }

    public void equalTexts()
    {
        textMeshProUGUIID.text = tMP_InputFieldID.text;
    }

    private void Update()
    {
        equalTexts();
    }


}
