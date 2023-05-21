using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersecutionEnemy : Enemy
{
    /* Métodos */
    /* Método InicializarComponentes */
    public void InicializarComponentes()
    {
        //Distancia a la que el enemigo detecta al Player
        maxDistance = 2f;
    }
    /* Método Start */
    public override void Start() 
    { 
        base.Start();
        InicializarComponentes();
        StartCoroutine(PersecutionLoop()); 
    }
    /* Método ProsecutionEnemyLoop*/
    private IEnumerator PersecutionLoop()
    {
        while (life > 0)
        {
            if ((!isHit) && (playerController.Life > 0))
            {
                switch ((IsPlayerNearEnemy()))
                {
                    case true:
                        StartParticleSystem(true);
                        anim.SetTrigger("walk");
                        PerseguirPlayer();
                        break;
                    case false:
                        StopParticleSystem();
                        anim.SetTrigger("idle");
                        yield return new WaitUntil(() => !IsPlayerNearEnemy());
                        break;
                }
            }
            else
            {
                StopParticleSystem();
                anim.SetTrigger("idle");
                isHit = false;
                yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
                
            }
            yield return GameManager.Instance.EndOfFrame;
        }
    }
    /* Método PerseguirPlayer*/
    public void PerseguirPlayer()
    {
        // Hacemos que el enemego vaya desde su posición hasta la del player y aumentamos su velocidad
        transform.position = Vector3.MoveTowards(transform.position, playerController.transform.position, speed * Time.deltaTime);
        // Ajustamos la dirección para que mire al jugador
        Vector3 directionToPlayer = playerController.transform.position - transform.position;
        if (directionToPlayer.x < 0)
        {
            tr.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            tr.localScale = new Vector3(-1, 1, 1);
        }
    }
 
    /* Método OnHit */
    public override void OnHit()
    {
        StartCoroutine(Die());
    }
}
