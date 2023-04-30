using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    /* Atributos */
    public Dictionary<PlayerStatesEnum, PlayerStates> states ;
    private PlayerStates currentState;
    private PlayerStatesEnum currentStateEnum;
    [SerializeField] private Animator animationController;
    [SerializeField] private ParticleSystem particleSystemFeet;
    [SerializeField] private ParticleSystem particleSystemDead;
    [SerializeField] private Transform playerFeet;
    private SpriteRenderer rend;
    private Transform tr;
    private Rigidbody2D rb;
    private bool directionRight;
    //About life
    private static bool invulnerable;
    private static float invulTime = 2;
    private int life = 2;
    //About Points
    private int points = 0;
    // About Cofre key
    private bool playerHasKey = false;
    //About Sounds
    private AudioSource audioSourcePlayer;
    [SerializeField] private AudioClip collectSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip lifeSound;
    [SerializeField] private AudioClip keySound;
    [SerializeField] private AudioClip onHitSound;
    [SerializeField] private AudioClip onHitEnemy;

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
        //Obtenemos el componente AudioSource del player
        audioSourcePlayer = GetComponent<AudioSource>();  
    }

    /* Método Start*/
    private void Start()
    {
        ChangeState(PlayerStatesEnum.PlayerGrounded);
        //Pintamos la vida actual del player
        GameManager.Instance.LifePlayerText = life.ToString();
    }

    /* Método Update */
    public void Update()
    {
        //Si el estado actual del player es distinto de null, ejecutamos el método Tick
        currentState?.Tick();
        //Aqui se controla la muerte del Player
        if(life <= 0) GameManager.Instance.LifePlayerText = "0";
    }

    /* Método FixedUpdate */
    public void FixedUpdate(){
        //Si el estado actual del player es distinto de null, ejecutamos el método FixedTick
        currentState?.FixedTick();
        //De esta forma evitamos que el player atraviese el collider del grid
        Rb.velocity = Vector2.ClampMagnitude(rb.velocity, 7);
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
        // Obtener el MainModule del ParticleSystem
        ParticleSystem.MainModule main = particleSystemFeet.main;
        main.loop = oneShot;

        if(!particleSystemFeet.isPlaying) 
        {
            particleSystemFeet.Play();
        }
    }

    /* Método StopParticleSystem */
    public void StopParticleSystem(){

        if(particleSystemFeet.isPlaying) 
        {
            particleSystemFeet.Stop();
        }
    }

    /* Método SwitchPlayerDirection */
    public void SwitchPlayerDirection(bool right)
    {
        GameManager.Instance.CameraControllerInstance.offsetDirection = right ? 0.2f : -0.2f; 
        tr.localScale = new Vector3((right ? 1 : -1), 1, 1);
        directionRight = right;
    }

    /* Método OnCollisionEnter2D */
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            bool removeEnemyLife = false;
            foreach (ContactPoint2D contact in other.contacts)
            {
                if (Mathf.Abs(contact.normal.y) > 0.5f)
                {
                    //El player no pierde vida al saltar encima del enemigo
                    ProcessEnemyHit(contact);
                    removeEnemyLife = true;
                }
                else if (Mathf.Abs(contact.normal.x) > 0.5f)
                {
                    //El player solo pierde vida si el enemigo le golpea por los lados
                    StartCoroutine(PlayerGotHit(contact));
                    removeEnemyLife = false;
                }
            }
            if(removeEnemyLife)
            {
                other.collider.GetComponent<Enemy>().RemoveLife(1);
            }
            else
            {
                RemoveLife(1);
                StartCoroutine(IsPlayerDie());
            }
        }
        if (other.gameObject.CompareTag("Trap"))
        {
            bool removePlayerLife = false;
            //El player solo pierde vida si cae encima de la trampa
            foreach (ContactPoint2D contact in other.contacts)
            {
                if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
                {
                    StartCoroutine(PlayerGotHit(contact));
                    removePlayerLife = true;
                }
            }
            if (removePlayerLife)
            {
                RemoveLife(1);
                StartCoroutine(IsPlayerDie());
            }

        }
        if (other.gameObject.CompareTag("TurtleTrap"))
        {
            bool removePlayerLife = false;
            //El player pierde vida si choca con la trampa
            foreach (ContactPoint2D contact in other.contacts)
            {
                StartCoroutine(PlayerGotHit(other.contacts[0]));
                removePlayerLife = true;
            }
            if (removePlayerLife)
            {
                RemoveLife(1);
                StartCoroutine(IsPlayerDie());
            }
        }
    }

    /* Método OnTriggerEnter2D */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Key"))
        {
            PlaySound("key");
            PlayerHasKey = true;
            Destroy(other.gameObject);
            GameManager.Instance.KeyImg.SetActive(true);
        }
        if (other.gameObject.CompareTag("Life"))
        {
            PlaySound("life");
            AddLife(1);
            Destroy(other.gameObject);
        }
    }

    /* Método ProcessEnemyHit */
    private void ProcessEnemyHit(ContactPoint2D point)
    {
        RaycastHit2D hit = Physics2D.Raycast((playerFeet.position), -Tr.up, 1);

        if (hit.collider.CompareTag("Enemy"))
        {
            Rb.AddForce(((point.normal) + (Vector2.up))*1.25f, ForceMode2D.Impulse);
            hit.collider.GetComponent<Enemy>().OnHit();
            PlaySound("hitenemy");
        }
    }

    /* Método PlayerGotHit */
    private IEnumerator PlayerGotHit(ContactPoint2D point)
    {
        Rb.AddForce(((point.normal)+(Vector2.up))*1.05f, ForceMode2D.Impulse);
        Color color = new Color(1,1,1,1);
        invulnerable = true;

        if (life > 0)
        {
            animationController.SetTrigger("Hit");
            PlaySound("hit");
            for (float i = 0; i < invulTime;)
            {

                for (float j = 0; j < 1; j += Time.deltaTime * 10)
                {
                    color.a = Mathf.Lerp(0, 1, j);
                    rend.color = color;
                    i += Time.deltaTime;
                    yield return GameManager.Instance.EndOfFrame;
                }

                for (float j = 0; j < 1; j += Time.deltaTime * 10)
                {
                    color.a = Mathf.Lerp(1, 0, j);
                    rend.color = color;
                    i += Time.deltaTime;
                    yield return GameManager.Instance.EndOfFrame;
                }

            }
            rend.color = Color.white;
            invulnerable = false;
        }
    }

    /* Método IsPlayerDie */
    private IEnumerator IsPlayerDie()
    {
        if(life <= 0)
        {
            //TODO: Sacar menu de reinicio
            animationController.SetTrigger("Hit");
            StopParticleSystem();
            //Animación cuando el player muere
            animationController.SetTrigger("Die");
            yield return new WaitForSeconds(animationController.GetCurrentAnimatorStateInfo(0).length * 1.75f);
            //Sistema de particulas cuando el player muere
            Instantiate(particleSystemDead,tr.position,Quaternion.identity);
            //El player desaparece de la escena
            GameManager.Instance.UnsubsCribeObject(gameObject);
            GameManager.Instance.PlaySound("gameover");
            //Si la vida llega a 0 salta el panel de Game Over
            GameManager.Instance.Invoke("ActiveGameoverPanel", 2f);
        }
    }

    /* Método AddPoints */
    public void AddPoints(int pointsToAdd)
    {
        points += pointsToAdd;
        GameManager.Instance.PointsPlayerText = "POINTS: " + points.ToString("D3");
    }

    /* Método AddLife */
    public void AddLife(int lifeNum)
    {
        if (life == 3) return;
        life += lifeNum;
        GameManager.Instance.LifePlayerText = life.ToString();
    }

    /* Método RemoveLife */
    public void RemoveLife(int life)
    {
        this.life -= life;
        GameManager.Instance.LifePlayerText = this.life.ToString();
    }

    /* Método PlayerWin */
    public void PlayerWin()
    {
        //TODO: definir condiciones para la victoria del player y gestionar el cambio de escena
        if((life>0) && (PlayerHasKey))
        {
            //Si la vida llega a 0 salta el panel de Game Win
            GameManager.Instance.Invoke("ActiveGameWinPanel", 2f);
        }
        Debug.Log("Necesitas estar en posesión de la llave y " +
            "abrir el cofre para poder pasar al siguiente nivel :(");
    }

    /* Método PlaySounds */
    public void PlaySound(String sound)
    {
        switch(sound)
        {
            case "fruit":
                audioSourcePlayer.PlayOneShot(collectSound);
            break;
            case "jump":
                audioSourcePlayer.PlayOneShot(jumpSound);
                break;
            case "key":
                audioSourcePlayer.PlayOneShot(keySound); 
                break;
            case "life":
                audioSourcePlayer.PlayOneShot(lifeSound);
                break;
            case "hit":
                audioSourcePlayer.PlayOneShot(onHitSound);
                break;
            case "hitenemy":
                audioSourcePlayer.PlayOneShot(onHitEnemy);
                break;
        }
    }

    /* Método PlaySounds */
    public void PlayerIsHit(ContactPoint2D point, int life = 1)
    {
        RemoveLife(life);
        StartCoroutine(PlayerGotHit(point));
        StartCoroutine(IsPlayerDie());
    }

    /* Getters y Setters*/
    public PlayerStatesEnum CurrentStateEnum { get => currentStateEnum; }
    public Transform Tr { get => tr; }
    public Animator AnimationController { get => animationController; }
    public Rigidbody2D Rb { get => rb; }
    public Transform PlayerFeet { get => playerFeet; }

    public bool PlayerHasKey 
    { 
        get => playerHasKey;
        set => playerHasKey = value;
    }
    public int Life { get => life; }
}
