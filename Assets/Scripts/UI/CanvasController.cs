using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{
    /* Atributos */
    [SerializeField] private string[] Scenes;

    /* Métodos */
    /* Método RestartScene */
    public void RestartScene()
    {
        //Método para reiniciar la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /* Método ExitScene */
    public void ExitScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
