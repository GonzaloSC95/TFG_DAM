using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private CameraController cameraControllerInstance;



    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(this);
        }
        else instance = this;
    }

   

    public CameraController CameraControllerInstance
    {
        get => cameraControllerInstance; 
        set {
            if (cameraControllerInstance != value && cameraControllerInstance != null)
            {
                Destroy(value);
            }
            else cameraControllerInstance = value;
        }
    }
}