using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Dictionary<PlayerStatesEnum, PlayerStates> states ;
    private PlayerStates currentState;
    private PlayerStatesEnum currentStateEnum;
    [SerializeField]private Animator animationController;
    [SerializeField]private ParticleSystem particleSystem;
    [SerializeField] Transform playerFeet;
    private Transform tr;
    private Rigidbody2D rb;




    private void Awake()
    {
        
        states=new Dictionary<PlayerStatesEnum, PlayerStates>() { { PlayerStatesEnum.PlayerGrounded, new PlayerGrounded(this) }, { PlayerStatesEnum.PlayerJumping, new PlayerJumping(this) } };
        tr=transform;
        rb=this.GetComponent<Rigidbody2D>();
        
    }
    private void Start()
    {
        ChangeState(PlayerStatesEnum.PlayerGrounded);
    }

    void Update()
    {
        currentState?.Tick();
    }
    void FixedUpdate(){
        currentState?.FixedTick();
    }

    public void ChangeState(PlayerStatesEnum stateToGo)
    {
        currentState?.OnEnd();
        currentStateEnum=stateToGo;
        currentState = states[stateToGo];
        currentState.OnBegin();
    }
    public void StartParticleSystem(bool oneShot=false){
        particleSystem.loop=oneShot;
        if(!particleSystem.isPlaying) 
            particleSystem.Play();
    }
    public void StopParticleSystem(){
        if(particleSystem.isPlaying) 
            particleSystem.Stop();
    }

    public void SwitchPlayerDirection(bool right){
        GameManager.instance.CameraControllerInstance.offsetDirection=right?0.5f:-0.5f;
        tr.localScale = new Vector3(right?1:-1, 1, 1);
    }


    ///Propertys
    public PlayerStatesEnum CurrentStateEnum { get => currentStateEnum; }
    public Transform Tr { get => tr;  }
    public Animator AnimationController { get => animationController; }
    public Rigidbody2D Rb { get => rb; }
    public Transform PlayerFeet { get => playerFeet;  }
}
