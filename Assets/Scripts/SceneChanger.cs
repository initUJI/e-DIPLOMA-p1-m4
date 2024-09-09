using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private ScreenOrientation originalOrientation;

    void OnEnable()
    {
        // Registra el método para que se llame al cambiar de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void OnDisable()
    {
        // Desregistra el método al deshabilitar el script
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Verifica si la escena cargada es la escena 0
        if (scene.buildIndex == 0)
        {
            // Guarda la orientación original y establece la orientación deseada
            originalOrientation = Screen.orientation;
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }

    void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        // Restaura la orientación original cuando se sale de la escena 0
        if (previousScene.buildIndex == 0 && newScene.buildIndex != 0)
        {
            Screen.orientation = originalOrientation;
        }
    }

    // Método para alternar el estado activo de un GameObject en la jerarquía
    public void Toggle(GameObject obj)
    {
        if (obj != null)
        {
            // Alterna el estado activo del GameObject en la jerarquía
            bool isActiveInHierarchy = obj.activeInHierarchy;
            obj.SetActive(!isActiveInHierarchy);
        }
        else
        {
            Debug.LogWarning("El GameObject proporcionado es nulo.");
        }
    }

    // Función pública que carga la escena con el índice 1
    public void LoadScene(int n)
    {
        SceneManager.LoadScene(n);
    }

    // Función pública para cerrar la aplicación
    public void QuitApplication()
    {
        // Verifica si estamos en el editor de Unity
#if UNITY_EDITOR
        // Si estamos en el editor, para la reproducción del juego
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si estamos en una construcción de juego, cierra la aplicación
        Application.Quit();
#endif
    }
}
