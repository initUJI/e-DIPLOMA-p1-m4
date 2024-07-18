using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string scene1 = "ImageTargetScene";
    public string scene2 = "VuforiaArWorking";

    public void SwitchScene()
    {
        // Obtiene el nombre de la escena actual
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Cambia a la otra escena
        if (currentSceneName == scene1)
        {
            SceneManager.LoadScene(scene2);
        }
        else if (currentSceneName == scene2)
        {
            SceneManager.LoadScene(scene1);
        }
    }
}
