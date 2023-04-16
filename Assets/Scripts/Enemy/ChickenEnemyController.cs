using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenEnemyController : MonoBehaviour
{
    /* Atributos */
    private Transform playerPosition;
    private float speed = 0.46f;
    private float maxDistance = 2f;
    private SpriteRenderer spriterendererEnemy;

    /* Métodos */
    /* Método Start */
    void Start()
    {
        //Inicializamos los componentes
        playerPosition = GameObject.Find("Player").transform;
        spriterendererEnemy = GetComponent<SpriteRenderer>();
    }

    /* Método FixedUpdate */
    void FixedUpdate()
    {
        // Si el player esta cerca le perseguimos
        if(IsPlayerNearEnemy())
        {
            PerseguirPlayer();
        }
    }

    /* Método PerseguirPlayer*/
    private void PerseguirPlayer()
    {
        // Hacemos que el enemego vaya desde su posición hasta la del player
        transform.position = Vector3.MoveTowards(transform.position, playerPosition.position, speed * Time.deltaTime);
        // Hacemos que la orientación del enemigo respecto a la del player, sea la adecuada
        if(transform.position.x < playerPosition.position.x)
        {
            spriterendererEnemy.flipX = true;
        }
        else
        {
            spriterendererEnemy.flipX = false;
        }
    }

    /* Método IsPlayerNearEnemy */
    private bool IsPlayerNearEnemy()
    {
        float distance = (Math.Abs(transform.position.x) - Math.Abs(playerPosition.position.x));
        if (Math.Abs(distance) <= maxDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
