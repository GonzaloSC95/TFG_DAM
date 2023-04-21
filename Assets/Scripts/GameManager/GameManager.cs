using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    /* Atributos */
    private static GameManager instance;
    private static CameraController cameraControllerInstance;
    /* About Player */
    private static PlayerController playerController;
    /* About Text */
    public  Text LifeText;
    public  Text PointsText;
    public  Image KeyImage;
    private string lifePlayerText;
    private string pointsPlayerText;
    private GameObject keyImg;
    /* About End of Frames*/
    private WaitForEndOfFrame endOfFrame;

    /* Métodos */
    /* Método InicializarComponentes */
    private void InicializarComponentes()
    {
        //Instanciamos el Player
        playerController = FindObjectOfType<PlayerController>();
        //Instanciamos un WaitForEndOfFrame
        endOfFrame = new WaitForEndOfFrame();
    }
    /* Método Awake */
    private void Awake()
    {
        //Creamos la instancia del objeto GameManager
        if (instance != this && instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        //Inicializamos los componentes
        InicializarComponentes();
    }
    
    /* Método RestartScene */
    public void RestartScene()
    {
        //Método para reiniciar la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /* Getters y Setters*/
    public static GameManager Instance { get => instance; }
    public WaitForEndOfFrame EndOfFrame { get => endOfFrame; }
    public PlayerController PlayerController { get => playerController; }
    public CameraController CameraControllerInstance
    {
        get => cameraControllerInstance;

        set
        {
            if (cameraControllerInstance != value && cameraControllerInstance != null)
            {
                Destroy(value);
            }
            else cameraControllerInstance = value;
        }
    }
    public string LifePlayerText 
    {
        get => lifePlayerText;
        set => LifeText.text = value;
    }
    public string PointsPlayerText
    {
        get => pointsPlayerText;
        set => PointsText.text = value;
    }
    public GameObject KeyImg
    {
        get => KeyImage.gameObject;
    }
    

}