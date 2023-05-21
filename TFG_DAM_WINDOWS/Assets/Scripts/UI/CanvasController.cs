using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    /* Atributos */
    [SerializeField] private GameObject pausePanel;
    private string sceneName;
    private Usuario user;
    
    /* M�todos */
    /* M�todo RestartScene */
    private void Start()
    {
        try 
        {
            sceneName = GetCurrentSceneName();
            user = SessionManager.Instance.User;
        }
        catch(Exception e)
        {
            Debug.LogError("SessionManager.Instance.User = null -- " + e.Message);
        }
        StartCoroutine(PauseMenuOn());

    }
    /* M�todo RestartScene */
    public void RestartScene()
    {
        //M�todo para reiniciar la escena
        TogglePause(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /* M�todo ExitScene */
    public void ExitScene()
    {
        TogglePause(false);
        if (user != null)
        {
            UpdateLevel();
        }
        SceneManager.LoadScene("Menu");
    }

    /* M�todo LoadNextLevel */
    public void LoadNextScene()
    {
        if (user != null)
        {
            UpdateLevel();
        }
        switch (sceneName)
        {
            case "Nivel_1":
                SceneManager.LoadScene("Nivel_2");
                break;
            case "Nivel_2":
                SceneManager.LoadScene("Nivel_3");
                break;
            case "Nivel_3":
                SceneManager.LoadScene("Credits");
                break;
            default:
                SceneManager.LoadScene("Menu");
                break;
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

    /* M�todo PauseMenuOn */
    public IEnumerator PauseMenuOn()
    {
        while(GameManager.Instance.PlayerController.Life > 0)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                pausePanel.SetActive(true);
                TogglePause(true);
            }
            yield return new WaitUntil(() => (Input.anyKeyDown == true));
        }
    }

    /* M�todo PauseMenuOut */
    public void PauseMenuOut()
    {
        pausePanel.SetActive(false);
        TogglePause(false);
    }

    /* M�todo TogglePause */
    void TogglePause(bool isPaused)
    {
        Time.timeScale = isPaused ? 0f : 1f;
    }
}
