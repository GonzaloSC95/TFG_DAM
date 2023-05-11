using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    /* Atributos */
    private string sceneName;
    private Usuario user;

    /* M�todos */
    /* M�todo RestartScene */
    private void Start()
    {
        sceneName = GetCurrentSceneName();
        user = SessionManager.Instance.User;
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
        if(user != null)
        {
            UpdateLevel();
            SceneManager.LoadScene("Menu");
        }
    }

    /* M�todo LoadNextLevel */
    public void LoadNextScene()
    {
        if (user != null)
        {
            UpdateLevel();
        
            switch (sceneName)
            {
                case "Nivel_1":
                    SceneManager.LoadScene("Nivel_2");
                    break;
                case "Nivel_2":
                    SceneManager.LoadScene("Nivel_3");
                    break;
                case "Nivel_3":
                    SceneManager.LoadScene("Menu");
                    break;
                default:
                    SceneManager.LoadScene("Menu");
                    break;
            }
        }
    }

    /* M�todo GetCurrentSceneName */
    public string GetCurrentSceneName()
    {
        string sceneName = "";
        // Obtener la escena activa
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene != null) sceneName = currentScene.name;
        // Obtener el nombre de la escena activa
        return sceneName;
    }

    /* M�todo UpdateLevel */
    public void UpdateLevel()
    {
        Partida level = SessionManager.Instance.Db.GetLastPartida(user.Id);
        level.Level = sceneName;
        level.Points = GameManager.Instance.PlayerController.Points;
        level.Life = GameManager.Instance.PlayerController.Life;
        level.Date = System.DateTime.Now;
        SessionManager.Instance.Db.UpdatePartida(level);
    }
}
