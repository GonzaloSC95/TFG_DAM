using System;
using UnityEngine;
using System.Collections;

public class PatrolEnemy : Enemy
{
    /* Atributos */
    protected Vector3 direction = -Vector3.right;

    /* Métodos */
    /* Método Update */
    private void Update()
    {
        tr.position += direction * (speed * Time.deltaTime);
    }

    /* Método FixedUpdate */
    void FixedUpdate()
    {
        hit = Physics2D.Raycast((tr.position), direction, 2, groundLayer);
        Debug.DrawRay((tr.position), direction, Color.blue, 1);
        if (hit)
        {
            if (hit.distance <= 0.5f)
            {
                ChangeDirection();
            }
        }
    }

    /* Método ChangeDirection */
    protected void ChangeDirection()
    {
        if (direction == Vector3.right)
        {
            direction = -Vector3.right;
            tr.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            direction = Vector3.right;
            tr.localScale = new Vector3(-1, 1, 1);
        }
    }

    /* Método OnHit */
    public override void OnHit()
    {
        StartCoroutine(Die());
    }
}