using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    /* Atributos */
    public Dictionary<PlayerStatesEnum, PlayerStates> states ;
    private PlayerStates currentState;
    private PlayerStatesEnum currentStateEnum;
    [SerializeField]private Animator animationController;
    [SerializeField]private ParticleSystem particleSystem;
    [SerializeField] Transform playerFeet;
    private SpriteRenderer rend;
    private Transform tr;
    private Rigidbody2D rb;
    private bool directionRight;
    //About life
    private bool invulnerable;
    private float invulTime = 2;
    private int life = 3;
    //About Points
    private static int points = 0;
    public Text pointsText;

    /* Propertys */
    public PlayerStatesEnum CurrentStateEnum { get => currentStateEnum; }
    public Transform Tr { get => tr; }
    public Animator AnimationController { get => animationController; }
    public Rigidbody2D Rb { get => rb; }
    public Transform PlayerFeet { get => playerFeet; }

    /* Métodos */
    /* Método Awake*/
    private void Awake()
    {
        //Inicializamos el diccionario de estados
        states = new Dictionary<PlayerStatesEnum, PlayerStates>() 
        { 
            { PlayerStatesEnum.PlayerGrounded, new PlayerGrounded(this) }, 
            { PlayerStatesEnum.PlayerJumping, new PlayerJumping(this) } 
        };
        //Obtenemos el componente Transform del player
        tr = transform;
        //Obtenemos el componente Rigidbody2D del player
        rb = this.GetComponent<Rigidbody2D>();
        //Obtenemos el componente SpriteRenderer del player
        rend = this.GetComponent<SpriteRenderer>();
        
    }

    /* Método Start*/
    private void Start()
    {
        ChangeState(PlayerStatesEnum.PlayerGrounded);
    }

    /* Método Update */
    void Update()
    {
        //Si el estado actual del player es distinto de null, ejecutamos el método Tick
        currentState?.Tick();
    }

    /* Método FixedUpdate */
    void FixedUpdate(){
        //Si el estado actual del player es distinto de null, ejecutamos el método FixedTick
        currentState?.FixedTick();
    }

    /* Método ChangeState */
    public void ChangeState(PlayerStatesEnum stateToGo)
    {
        currentState?.OnEnd();
        currentStateEnum = stateToGo;
        currentState = states[stateToGo];
        currentState.OnBegin();
    }

    /* Método StartParticleSystem */
    public void StartParticleSystem(bool oneShot = false)
    {
        particleSystem.loop = oneShot;
        if(!particleSystem.isPlaying) 
        {
            particleSystem.Play();
        }
    }

    /* Método StopParticleSystem */
    public void StopParticleSystem(){

        if(particleSystem.isPlaying) 
        {
            particleSystem.Stop();
        }
    }

    /* Método SwitchPlayerDirection */
    public void SwitchPlayerDirection(bool right)
    {
        GameManager.instance.CameraControllerInstance.offsetDirection = right?0.2f:-0.2f; 
        tr.localScale = new Vector3((right?1:-1), 1, 1);
        directionRight = right;
    }

    /* Método OnCollisionEnter2D */
    private void OnCollisionEnter2D(Collision2D other)
    {

        if(other.gameObject.CompareTag("Enemy")){
            ProcessEnemyHit(other.contacts[0]);
        }
    }

    /* Método ProcessEnemyHit */
    private void ProcessEnemyHit(ContactPoint2D point)
    {
         RaycastHit2D hit = Physics2D.Raycast((playerFeet.position), -Tr.up,1);
        
         if(hit.collider.CompareTag("Enemy"))
         {
            Rb.AddForce(Vector2.up * (3), ForceMode2D.Impulse);
            hit.collider.GetComponent<Enemy>().OnHit();
         }
         else
         {
            if(invulnerable) return;
            StartCoroutine(PlayerGotHit(point));
         }
    }

    /* Método PlayerGotHit */
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

    /* Método AddPoints */
    public void AddPoints(int pointsToAdd)
    {
        points += pointsToAdd;
        pointsText.text = "POINTS: " + points.ToString("D3");
    }
}
