using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterEnemy : Enemy
{
    /* Atributos */
    [SerializeField] private Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject bullet;
    private float shootInterval = 1f;
    private float nextShootTime = 0f;

    /* M�todos */
    /* M�todo InicializarComponentes */
    public void InicializarComponentes()
    {
        maxDistance = 2f; 
    }

    /* M�todo Start */
    public override void Start()
    {
        base.Start();
        InicializarComponentes();
        StartCoroutine(ShooterLoop());
    }

    /* M�todo PatrolPersecutionLoop */
    private IEnumerator ShooterLoop()
    {
        while (life > 0)
        {
            LookAtPlayer();
            if ((!isHit) && ((playerController.Life > 0) && (playerController.gameObject.activeSelf)))
            {
                if (IsPlayerNearEnemy())
                {
                    anim.SetTrigger("attack");
                    Shoot();
                }
                else
                {
                    anim.SetTrigger("idle");
                }
            }
            else
            {
                anim.SetTrigger("idle");
                isHit = false;
                yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            }
            yield return GameManager.Instance.EndOfFrame;
        }

    }

    /* M�todo Shoot */
    private void Shoot()
    {

        // Si el jugador est� dentro del rango de disparo y ha pasado el tiempo de espera para el siguiente disparo
        if (IsPlayerNearEnemy() && (Time.time >= nextShootTime))
        {
            // Crear una instancia del prefab de bala en la posici�n del firePoint
            bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            SetBulletDirecction();
            // Establecer el tiempo de espera para el siguiente disparo
            nextShootTime = Time.time + shootInterval;
        }

    }

    /* M�todo LookAtEnemy */
    private void LookAtPlayer()
    {
        // Ajustamos la direcci�n para que mire al jugador
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

    /* M�todo SetBulletDirecction */
    private void SetBulletDirecction()
    {
        Vector3 directionToPlayer = playerController.transform.position - transform.position;
        if (directionToPlayer.x < 0)
        {
            if (bullet)
            {
                bullet.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
                bullet.GetComponent<BulletController>().Direction = -Vector3.right;
            }
        }
        else
        {
            if (bullet)
            {
                bullet.GetComponent<Transform>().localScale = new Vector3(-1, 1, 1);
                bullet.GetComponent<BulletController>().Direction = Vector3.right;
            }
        }
    }


    /* M�todo OnHit */
    public override void OnHit()
    {
        StartCoroutine(Die());
    }

}
