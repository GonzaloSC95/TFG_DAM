using UnityEngine;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private CameraController cameraControllerInstance;
    [SerializeField]private List<Enemy> enemies;


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

    public void SubscribeEnemy(Enemy enemy){
        enemies.Add(enemy);
    }
    public void UnsuscribeEnemy(Enemy enemy){
        enemies.Remove(enemy);
    }

}