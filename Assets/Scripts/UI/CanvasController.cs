using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    /* Atributos */
    private string sceneName;

    /* M�todos */
    /* M�todo RestartScene */
    private void Start()
    {
        sceneName = GetCurrentSceneName();
    }
    /* M�todo RestartScene */
    public void RestartScene()
    {
        //M�todo para reiniciar la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /* M�todo ExitScene */
    public void ExitScene()
    {
        SceneManager.LoadScene("Menu");
    }

    /* M�todo LoadNextLevel */
    public void LoadNextScene()
    {
        switch(sceneName)
        {
            case "Nivel1":
                SceneManager.LoadScene("Nivel_2");
                break;
            case "Nivel2":
                SceneManager.LoadScene("Nivel_3");
                break;
            case "Nivel3":
                SceneManager.LoadScene("Menu");
                break;
            default:
                SceneManager.LoadScene("Nivel_1");
                break;
        }
    }
    public string GetCurrentSceneName()
    {
        string sceneName = "";
        // Obtener la escena activa
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene != null) sceneName = currentScene.name;
        // Obtener el nombre de la escena activa
        return sceneName;
    }
}
