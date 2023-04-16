using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    /* Atributos */
    public int life;
    public int points;
    public RaycastHit2D hit;
    protected Transform tr;
    int groundLayer ;
    protected Vector3 direction = -Vector3.right;


    /* Métodos */
    public abstract void OnHit();
    public abstract void OnWallCollide();
    /* Método Awake */
    private void Awake()
    {
        tr = this.transform;
        groundLayer= LayerMask.GetMask("Ground");
    }
    /* Método Start */
    private void Start()
    {
        GameManager.instance.SubscribeEnemy(this);
    }
    /* Método FixedUpdate */
    public virtual void FixedUpdate()
    {
        hit = Physics2D.Raycast((tr.position), direction, 2, groundLayer);
        Debug.DrawRay((tr.position), direction, Color.blue, 1);
        if (hit)
        {
            if (hit.distance <= 0.5f)
            {
                OnWallCollide();
            }
        }
    }
    /* Método OnCollisionEnter2D */
    private void OnCollisionEnter2D(Collision2D other)
    {
         
    }
    /* Método ChangeDirection */
    protected void ChangeDirection()
    {
        if (direction == Vector3.right)
        {
            direction = -Vector3.right;
            tr.localScale=new Vector3(1,1,1);
        }
        else
        {
            direction = Vector3.right;
            tr.localScale=new Vector3(-1,1,1);
        }
    }
    



}
