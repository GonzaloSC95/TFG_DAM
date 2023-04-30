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
    public Text LifeText;
    public Text PointsText;
    public Image KeyImage;
    /* About End of Frames*/
    private WaitForEndOfFrame endOfFrame;
    /* About Main Camera */
    private Camera mainCamera;
    /* About Sounds */
    private AudioSource audioSource;
    [SerializeField] private AudioClip gameOverAudio;
    [SerializeField] private AudioClip killEnemyAudio;
    [SerializeField] private AudioClip boomAudio;
    [SerializeField] private AudioClip winGameAudio;

    /* M�todos */
    /* M�todo InicializarComponentes */
    private void InicializarComponentes()
    {
        //Instanciamos el Player
        playerController = FindObjectOfType<PlayerController>();
        //Instanciamos un WaitForEndOfFrame
        endOfFrame = new WaitForEndOfFrame();
        //Instanciamos la camara principal
        mainCamera = Camera.main;
        //Instanciamos el componente Audio Source
        audioSource = GetComponent<AudioSource>();
    }
    /* M�todo Awake */
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
        Debug.Log("GameManager is Operative--->" + instance);
        //Inicializamos los componentes
        InicializarComponentes();
    }

    /* M�todo Update */
    private void Update()
    {
        UnsubscribePartyCleSystems();
    }

    /* M�todo RestartScene */
    public void RestartScene()
    {
        //M�todo para reiniciar la escena
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /* M�todo UnsubsCribeObject */
    public void UnsubsCribeObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    /* Public UnsubsCribeObjectIsoutOfMainCamera */
    public void UnsubsCribeObjectIsoutOfMainCamera(GameObject obj)
    {
        // Obtener las coordenadas normalizadas del objeto en la c�mara
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(obj.transform.position);

        // Verificar si las coordenadas normalizadas est�n fuera del rango (0,0) a (1,1)
        if (viewportPos.x < 0 || viewportPos.x > 1 || viewportPos.y < 0 || viewportPos.y > 1)
        {
            UnsubsCribeObject(obj);
            Destroy(obj);
        }
    }

    /* Public UnsubsCribeParticleSystems */
    public void UnsubscribePartyCleSystems()
    {
        GameObject[] partycleSystems = GameObject.FindGameObjectsWithTag("partycleSys");
        if(partycleSystems.Length > 0)
        {
            foreach (GameObject obj in partycleSystems)
            {
                if (!obj.GetComponent<ParticleSystem>().isEmitting)
                {
                    Destroy(obj);
                }
            }
        }
    }

    /* M�todo PlaySound */
    public void PlaySound(string sound)
    { 
        switch(sound)
        {
            case "gameover":
                audioSource.PlayOneShot(gameOverAudio);
                break;
            case "killenemy":
                audioSource.PlayOneShot(killEnemyAudio);
                break;
            case "boom":
                audioSource.PlayOneShot(boomAudio);
                break;
            case "win":
                audioSource.PlayOneShot(winGameAudio);
                break;
        }
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
        get => LifeText.text;
        set => LifeText.text = value;
    }
    public string PointsPlayerText
    {
        get => PointsText.text;
        set => PointsText.text = value;
    }
    public GameObject KeyImg
    {
        get => KeyImage.gameObject;
    }
    

}