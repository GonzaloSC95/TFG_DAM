using System;
using System.Collections;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    /* Atributos */
    private float maxDistance;
    private PlayerController playerController;
    private Animator anim;

    /* Métodos */
    public void InicializarComponentes()
    {
        playerController = GameManager.Instance.PlayerController;
        anim = GetComponent<Animator>();
        maxDistance = 2f;
    }
    /* Método Start */
    private void Start()
    {
        InicializarComponentes();
        StartCoroutine(Activedefense());
    }
    /* Método Update */
    private IEnumerator Activedefense()
    {
        while(true)
        {
            anim.SetBool("pinchos", IsPlayerNearTrap());
            yield return GameManager.Instance.EndOfFrame;
        }
    }
    /* Método IsPlayerNearEnemy */
    private bool IsPlayerNearTrap()
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
    /* Método IsPlayerNearEnemy */
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().PlayerIsHit(other.contacts[0]);
        }
        
    }
}
