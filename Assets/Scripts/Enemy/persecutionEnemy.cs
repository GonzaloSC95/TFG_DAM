using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersecutionEnemy : Enemy
{
    /* Atributos */
    private float maxDistance;

    /* Métodos */
    /* Método InicializarComponentes */
    public void InicializarComponentes()
    {
        maxDistance = 2f;
    }
    /* Método Start */
    public override void Start() 
    { 
        base.Start();
        InicializarComponentes();
        StartCoroutine(PersecutionEnemyLoop()); 
    }
    /* Método ProsecutionEnemyLoop*/
    private IEnumerator PersecutionEnemyLoop()
    {
        while (life > 0)
        {
            if (!isHit)
            {
                switch ((IsPlayerNearEnemy()))
                {
                    case true:
                        anim.SetTrigger("walk");
                        PerseguirPlayer();
                        break;
                    case false:
                        anim.SetTrigger("idle");
                        yield return new WaitUntil(() => !IsPlayerNearEnemy());
                        break;
                }
            }
            else
            {
                yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
                anim.SetTrigger("idle");
                isHit = false;
            }
            yield return GameManager.Instance.EndOfFrame;
        }
    }
    /* Método PerseguirPlayer*/
    private void PerseguirPlayer()
    {
        // Hacemos que el enemego vaya desde su posición hasta la del player
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
