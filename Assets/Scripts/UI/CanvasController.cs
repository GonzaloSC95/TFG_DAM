using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    /* Atributos */
    [SerializeField] private string[] Scenes;

    /* M�todos */
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
}
