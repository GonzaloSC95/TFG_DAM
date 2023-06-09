using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    /* Atributos */
    /* About Life and Points*/
    public int life;
    public int points;
    /* About Speed */
    public float speed;
    /* About Components */
    protected RaycastHit2D hit;
    protected Transform tr;
    protected Collider2D col;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected SpriteRenderer rend;
    /* About Layer */
    protected int groundLayer;
    /* About Player */
    protected PlayerController playerController;
    /* About hit */
    protected bool isHit;
    /* About max distance from the Player */
    protected float maxDistance;
    /* About Partycle System */
    [SerializeField] private ParticleSystem partycleSystemDead;
    [SerializeField] private ParticleSystem partycleSystemRun;
    /* About flying enemies */
    public bool canFly;
    /* About lifes for Player */
    public GameObject lifePrefab;

    /* Métodos Abstractos */
    /* Método OnHit */
    public abstract void OnHit();
    /* Métodos */
    /* Método Awake */
    private void Awake()
    {
        tr = this.transform;
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("Ground");
    }

    /* Método Start */
    public virtual void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        playerController = GameManager.Instance.PlayerController;
    }
    
    /* Método IEnumerator */
    public IEnumerator Die()
    {
        float oldSpeed = speed;
        yield return new WaitForSeconds(0.5f);
        Color color = new Color(1,1,1,1);

        anim.SetTrigger("hit");
        isHit = true;

        //Detenemos al enemigo
        col.enabled = false;
        rb.simulated = false;
        speed = 0;

        //Si la vida es menor o igual a 0 el enemigo muere
        if (life <= 0)
        {
            //Make it fade
            for (int i = 0; i < 5; i++)
            {
                for (float j = 0; j < 1; j += Time.deltaTime*10)
                {
                    color.a = Mathf.Lerp(0,1,j);
                    rend.color = color;
                    yield return GameManager.Instance.EndOfFrame;
                } 

                 for (float j = 0; j < 1; j += Time.deltaTime*10)
                {
                    color.a = Mathf.Lerp(1,0,j);
                    rend.color = color;
                    yield return GameManager.Instance.EndOfFrame;
                }
            }
            //Hacemos que el enemigo desparezaca de la escena
            GameManager.Instance.UnsubsCribeObject(gameObject);
            //Instanciamos su sistema de particulas
            Instantiate(partycleSystemDead, tr.position, Quaternion.identity);
            //Emitimos el sonido correspondiente
            GameManager.Instance.PlaySound("killenemy");
            GiveLifeToPlayer();
            // Le sumamos los puntos al jugador por matar al enemigo
            playerController.GetComponent<PlayerController>().AddPoints(points);

        }
        else
        {
            //Volvemos a activar al enemigo
            yield return new WaitForSeconds(0.5f);
            col.enabled = true;
            rb.simulated = true;
            speed = oldSpeed;
        }
    }

    /* Método RemoveLive */
    public void RemoveLife(int life) {

        this.life -= life;
    }

    /* Método IsPlayerNearEnemy */
    public bool IsPlayerNearEnemy()
    {
        if(!canFly)
        {
            //Si el player esta a una altura elevada el enemigo no le persigue 
            float distanceY = (Math.Abs(transform.position.y) - Math.Abs(playerController.transform.position.y));
            if (Math.Round(distanceY) != 0)
            {
                return false;
            }
        }
        //Si el player no esta a una altura elevada y esta cerca respecto al eje x, el enemigo si le persigue 
        float distanceX = (Math.Abs(transform.position.x) - Math.Abs(playerController.transform.position.x));
        if (Math.Abs(distanceX) <= maxDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /* Método StartParticleSystem */
    public void StartParticleSystem(bool oneShot = false)
    {
        if(partycleSystemRun)
        {
            // Obtener el MainModule del ParticleSystem
            ParticleSystem.MainModule main = partycleSystemRun.main;
            main.loop = oneShot;

            if (!partycleSystemRun.isPlaying)
            {
                partycleSystemRun.Play();
            }
        }
    }

    /* Método StopParticleSystem */
    public void StopParticleSystem()
    {
        if (partycleSystemRun)
        {
            if (partycleSystemRun.isPlaying)
            {
                partycleSystemRun.Stop();
            }
        }
    }

    /* Método GiveLifeToPlayer */
    public void GiveLifeToPlayer()
    {
        int randomNum, maxRange;
        if (speed >= 0.6)
        {
            maxRange = 2;
        }
        else
        { 
            maxRange = 11; 
        }
        randomNum = UnityEngine.Random.Range(0, maxRange);
        if((randomNum == 1) && (lifePrefab))
        {
            Instantiate(lifePrefab, transform.position, Quaternion.identity);
        }

    }
}
