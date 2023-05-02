using System.Collections;
using UnityEngine;

public class PatrolPersecutionEnemy : Enemy
{
    /* Atributos */
    private Vector3 direction;

    /* M�todos */
    /* M�todo InicializarComponentes */
    private void InicializarComponentes()
    {
        direction = -Vector3.right;
        maxDistance = 2f;
    }
    /* M�todo Start */
    public override void Start()
    {
        base.Start();
        InicializarComponentes();
        StartCoroutine(PatrolPersecutionLoop());
    }
    /* M�todo PatrolPersecutionLoop */
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
    /* M�todo Patrol */
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
    /* M�todo Persecution */
    private void Persecution()
    {
        // Hacemos que el enemego vaya desde su posici�n hasta la del player y aumentamos su velocidad
        transform.position = Vector3.MoveTowards(transform.position, playerController.transform.position, (speed * 2f) * Time.deltaTime);
        // Ajustamos la direcci�n para que mire al jugador
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

    /* M�todo OnHit */
    public override void OnHit()
    {
        StartCoroutine(Die());
    }
}
