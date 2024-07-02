
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class buttonTest : MonoBehaviour
{
    private GameObject component;
    public Button targetButton;
    private BoxCollider boxCollider;

    public Slider progressSlider; // Referencia al Slider
    public float requiredCollisionTime = 1f; // Tiempo requerido de colisión

    private bool isIndexColliding = false;
    private Coroutine collisionCoroutine;
    private Collider[] collidersBuffer = new Collider[10]; // Buffer para evitar la creación de arrays

    void Start()
    {
        // Cachear referencias
        component = transform.parent.parent.gameObject;
        targetButton = GetComponent<Button>();
        boxCollider = GetComponent<BoxCollider>();

        if (progressSlider != null)
        {
            progressSlider.gameObject.SetActive(false); // Ocultar el Slider al inicio
        }

    }

    void Update()
    {
        // Limitar la frecuencia de verificación de colisiones
        if (Time.frameCount % 10 == 0)
        {
            bool currentlyColliding = CheckIndexCollision();

            if (currentlyColliding && collisionCoroutine == null)
            {
                collisionCoroutine = StartCoroutine(CollisionCheckCoroutine());
            }
            else if (!currentlyColliding && collisionCoroutine != null)
            {
                StopCoroutine(collisionCoroutine);
                collisionCoroutine = null;
                progressSlider.gameObject.SetActive(false); // Ocultar el Slider si la colisión termina
            }

            isIndexColliding = currentlyColliding;
        }
    }

    private bool CheckIndexCollision()
    {
        int numColliders = Physics.OverlapBoxNonAlloc(boxCollider.bounds.center, boxCollider.bounds.extents, collidersBuffer, boxCollider.transform.rotation);

        for (int i = 0; i < numColliders; i++)
        {
            if (collidersBuffer[i].CompareTag("Index"))
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator CollisionCheckCoroutine()
    {
        progressSlider.gameObject.SetActive(true); // Mostrar el Slider
        float elapsedTime = 0f;

        while (elapsedTime < requiredCollisionTime)
        {
            if (!isIndexColliding)
            {
                progressSlider.gameObject.SetActive(false); // Ocultar el Slider si la colisión termina
                yield break;
            }

            elapsedTime += Time.deltaTime;
            progressSlider.value = elapsedTime / requiredCollisionTime; // Actualizar el valor del Slider

            yield return null;
        }


        progressSlider.gameObject.SetActive(false); // Ocultar el Slider después de la colisión
        collisionCoroutine = null;
    }
}

