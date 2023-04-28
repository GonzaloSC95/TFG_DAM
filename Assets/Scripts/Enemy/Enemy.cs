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
        
            gameObject.SetActive(false);
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
        //Si el player esta a una altura elevada el enemigo no le persigue 
        float distanceY = (Math.Abs(transform.position.y) - Math.Abs(playerController.transform.position.y));
        if (Math.Round(distanceY) != 0)
        {
            return false;
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
}
