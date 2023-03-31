using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    /* Atributos */
    public Dictionary<PlayerStatesEnum, PlayerStates> states ;
    private PlayerStates currentState;
    private PlayerStatesEnum currentStateEnum;
    [SerializeField]private Animator animationController;
    [SerializeField]private ParticleSystem particleSystem;
    [SerializeField] Transform playerFeet;
    SpriteRenderer rend;
    private Transform tr;
    private Rigidbody2D rb;
    bool directionRight;
    bool invulnerable;
    //invulnerable time
    float invulTime=2;
    int life=3;

    /* Métodos */
    /* 1º Metodo que se ejecuta */
    private void Awake()
    {
        
        states=new Dictionary<PlayerStatesEnum, PlayerStates>() { { PlayerStatesEnum.PlayerGrounded, new PlayerGrounded(this) }, { PlayerStatesEnum.PlayerJumping, new PlayerJumping(this) } };
        tr=transform;
        rb=this.GetComponent<Rigidbody2D>();
        rend=this.GetComponent<SpriteRenderer>();
        
    }
    /* 2º Metodo que se ejecuta */
    private void Start()
    {
        ChangeState(PlayerStatesEnum.PlayerGrounded);
    }
    /* Metodo de actualización */
    void Update()
    {
        currentState?.Tick();
    }
    /* Metodo de actualización */
    void FixedUpdate(){
        currentState?.FixedTick();
    }
    /* Metodo de cambio de estado */
    public void ChangeState(PlayerStatesEnum stateToGo)
    {
        currentState?.OnEnd();
        currentStateEnum=stateToGo;
        currentState = states[stateToGo];
        currentState.OnBegin();
    }
    /* Metodo para iniciar el sistema de particulas */
    public void StartParticleSystem(bool oneShot=false){
        particleSystem.loop=oneShot;
        if(!particleSystem.isPlaying) 
            particleSystem.Play();
    }
    /* Metodo para parar el sistema de particulas */
    public void StopParticleSystem(){
        if(particleSystem.isPlaying) 
            particleSystem.Stop();
    }
    /* Metodo para parar el sistema de particulas */
    public void SwitchPlayerDirection(bool right){
        GameManager.instance.CameraControllerInstance.offsetDirection=right?0.5f:-0.5f;
        tr.localScale = new Vector3(right?1:-1, 1, 1);
        directionRight=right;
    }

   
    private void OnCollisionEnter2D(Collision2D other)
    {

        if(other.gameObject.CompareTag("Enemy")){
            ProcessEnemyHit(other.contacts[0]);
        }
    }

    private void ProcessEnemyHit(ContactPoint2D point)
    {
         RaycastHit2D hit = Physics2D.Raycast((playerFeet.position), -Tr.up,1);
        
         if(hit.collider.CompareTag("Enemy")){
             Rb.AddForce(Vector2.up * (3), ForceMode2D.Impulse);
             hit.collider.GetComponent<Enemy>().OnHit();
         }
         else{
            if(invulnerable)return;
            StartCoroutine(PlayerGotHit(point));
         }
    }

    private IEnumerator PlayerGotHit(ContactPoint2D point)
    {
        
        rb.AddForce(((point.normal)+(Vector2.up))*1.5f, ForceMode2D.Impulse);
        WaitForEndOfFrame endOfFrame= new WaitForEndOfFrame();
        Color color=new Color(1,1,1,1);
        invulnerable=true;
        animationController.SetTrigger("Hit");
        life--;
        for (float i = 0; i < invulTime;)
        {
          
            for (float j = 0; j < 1; j+=Time.deltaTime*10)
            {
                color.a=Mathf.Lerp(0,1,j);
                rend.color=color;
                i+=Time.deltaTime;
                yield return endOfFrame;
            } 
            
             for (float j = 0; j < 1; j+=Time.deltaTime*10)
            {
                color.a=Mathf.Lerp(1,0,j);
                rend.color=color;
                 i+=Time.deltaTime;
                yield return endOfFrame;
            }

        }
        rend.color=Color.white;
        invulnerable=false;
        

        
    }


    /* Propertys */
    public PlayerStatesEnum CurrentStateEnum { get => currentStateEnum; }
    public Transform Tr { get => tr;  }
    public Animator AnimationController { get => animationController; }
    public Rigidbody2D Rb { get => rb; }
    public Transform PlayerFeet { get => playerFeet;  }
}
