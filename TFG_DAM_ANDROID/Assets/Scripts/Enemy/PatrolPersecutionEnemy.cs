using System.Collections;
using UnityEngine;

public class PatrolPersecutionEnemy : Enemy
{
    /* Atributos */
    private Vector3 direction;

    /* Métodos */
    /* Método InicializarComponentes */
    private void InicializarComponentes()
    {
        direction = -Vector3.right;
        maxDistance = 2f;
    }
    /* Método Start */
    public override void Start()
    {
        base.Start();
        InicializarComponentes();
        StartCoroutine(PatrolPersecutionLoop());
    }
    /* Método PatrolPersecutionLoop */
    private IEnumerator PatrolPersecutionLoop()
    {
        while (life > 0)
        {
            if (((playerController.Life > 0) && (playerController.gameObject.activeSelf)))
            {
                if (!IsPlayerNearEnemy())
                {
                    StopParticleSystem();
                    anim.SetTrigger("walk");
                    Patrol();
                }
                else
                {
                    anim.SetTrigger("run");
                    StartParticleSystem(true);
                    Persecution();
                }
            }
            else
            {
                StopParticleSystem();
                anim.SetTrigger("idle");
            }
            yield return GameManager.Instance.EndOfFrame;
        }
            
    }
    /* Método Patrol */
    private void Patrol()
    {
        tr.position += direction * (speed * Time.deltaTime);
        hit = Physics2D.Raycast((tr.position), direction, 2, groundLayer);
        Debug.DrawRay((tr.position), direction, Color.blue, 1);
        if (hit)
        {
            if (hit.distance <= 0.5f)
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
        }
    }
    /* Método Persecution */
    private void Persecution()
    {
        // Hacemos que el enemego vaya desde su posición hasta la del player y aumentamos su velocidad
        transform.position = Vector3.MoveTowards(transform.position, playerController.transform.position, (speed * 2f) * Time.deltaTime);
        // Ajustamos la dirección para que mire al jugador
        Vector3 directionToPlayer = playerController.transform.position - transform.position;
        if (directionToPlayer.x < 0)
        {
            tr.localScale = new Vector3(1, 1, 1);
            direction = -Vector3.right;
        }
        else
        {
            tr.localScale = new Vector3(-1, 1, 1);
            direction = Vector3.right;
        }
    }

    /* Método OnHit */
    public override void OnHit()
    {
        StartCoroutine(Die());
    }
}
