using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public GameObject selectModeCanvas;
    public GameObject mainCanvas;
    public GameObject block;
    private ScreenOrientation originalOrientation;

    void OnEnable()
    {
        // Registra el m�todo para que se llame al cambiar de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
    }

    void OnDisable()
    {
        // Desregistra el m�todo al deshabilitar el script
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    // M�todo para verificar si el modo de componentes ha sido completado
    public bool IsComponentsModeCompleted()
    {
        return PlayerPrefs.GetInt("ComponentsModeCompleted", 0) == 1;
    }

    public void activeSelectMenu()
    {
        selectModeCanvas.SetActive(true);
        mainCanvas.SetActive(false);

        if (IsComponentsModeCompleted())
        {
            block.SetActive(false);
        }
    }

    public void activeMainMenu()
    {
        mainCanvas.SetActive(true);
        selectModeCanvas.SetActive(false);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Verifica si la escena cargada es la escena 0
        if (scene.buildIndex == 0)
        {
            // Guarda la orientaci�n original y establece la orientaci�n deseada
            originalOrientation = Screen.orientation;
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }

    void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        // Restaura la orientaci�n original cuando se sale de la escena 0
        if (previousScene.buildIndex == 0 && newScene.buildIndex != 0)
        {
            Screen.orientation = originalOrientation;
        }
    }

    // M�todo para alternar el estado activo de un GameObject en la jerarqu�a
    public void Toggle(GameObject obj)
    {
        if (obj != null)
        {
            // Alterna el estado activo del GameObject en la jerarqu�a
            bool isActiveInHierarchy = obj.activeInHierarchy;
            obj.SetActive(!isActiveInHierarchy);
        }
        else
        {
            Debug.LogWarning("El GameObject proporcionado es nulo.");
        }
    }

    // Funci�n p�blica que carga la escena con el �ndice 1
    public void LoadScene(int n)
    {
        SceneManager.LoadScene(n);
    }

    // Funci�n p�blica para cerrar la aplicaci�n
    public void QuitApplication()
    {
        // Verifica si estamos en el editor de Unity
#if UNITY_EDITOR
        // Si estamos en el editor, para la reproducci�n del juego
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Si estamos en una construcci�n de juego, cierra la aplicaci�n
        Application.Quit();
#endif
    }
}
