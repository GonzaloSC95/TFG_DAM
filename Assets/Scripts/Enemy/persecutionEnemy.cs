using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class persecutionEnemy : Enemy
{
    /* Atributos */
    private float maxDistance = 2f;

    /* Métodos */
    /* Método FixedUpdate */
    void FixedUpdate()
    {
        // Si el player esta cerca le perseguimos
        if (IsPlayerNearEnemy())
        {
            PerseguirPlayer();
        }
        else
        {
            anim.SetTrigger("Idle");
        }
    }

    /* Método PerseguirPlayer*/
    private void PerseguirPlayer()
    {
        // Hacemos que el enemego vaya desde su posición hasta la del player
        anim.SetTrigger("Walk");
        transform.position = Vector3.MoveTowards(transform.position, playerController.transform.position, speed * Time.deltaTime);
        // Hacemos que la orientación del enemigo respecto a la del player, sea la adecuada
        if(transform.position.x < playerController.transform.position.x)
        {
            rend.flipX = true;
        }
        else
        {
            rend.flipX = false;
        }
    }

    /* Método IsPlayerNearEnemy */
    private bool IsPlayerNearEnemy()
    {
        float distance = (Math.Abs(transform.position.x) - Math.Abs(playerController.transform.position.x));
        if (Math.Abs(distance) <= maxDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /* Método OnHit */
    public override void OnHit()
    {
        StartCoroutine(Die());
    }
}
