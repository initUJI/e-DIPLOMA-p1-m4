using UnityEngine;

public class AlwaysOnTopCanvas : MonoBehaviour
{
    public Camera mainCamera; // Asigna la cámara principal en el inspector

    private Canvas canvas;
    private RectTransform canvasRect;

    void Start()
    {
        canvas = GetComponent<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("Este objeto no tiene un componente Canvas.");
            return;
        }

        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }

        if (canvas.renderMode != RenderMode.WorldSpace)
        {
            Debug.LogWarning("El Canvas debe estar en modo World Space para funcionar correctamente.");
            canvas.renderMode = RenderMode.WorldSpace;
        }

        // Ajustar la escala del Canvas para adaptarse a la pantalla
        canvasRect = canvas.GetComponent<RectTransform>();
        AdjustCanvasSize();
        PositionCanvasInFrontOfCamera();
    }

    void Update()
    {
        // Asegurarse de que el Canvas permanezca frente a la cámara
        PositionCanvasInFrontOfCamera();
    }

    private void AdjustCanvasSize()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        canvasRect.sizeDelta = new Vector2(screenWidth, screenHeight);
        float scale = 1.0f / Mathf.Max(screenWidth, screenHeight);
        canvasRect.localScale = new Vector3(scale, scale, scale) * 1f;
    }

    private void PositionCanvasInFrontOfCamera()
    {
        if (mainCamera != null)
        {
            // Colocar el Canvas a una distancia fija frente a la cámara
            canvas.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 1f; // Ajustar la distancia según sea necesario
            canvas.transform.rotation = mainCamera.transform.rotation;
        }
    }
}
