using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public int life;
    public int points;
    public RaycastHit2D hit;
    protected Transform tr;
    int groundLayer ;
    protected Vector3 direction = -Vector3.right;



    public abstract void OnHit();
    public abstract void OnWallCollide();
    private void Awake()
    {
        tr = this.transform;
        groundLayer= LayerMask.GetMask("Ground");
    }
    private void Start()
    {
        GameManager.instance.SubscribeEnemy(this);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
         
    }

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
