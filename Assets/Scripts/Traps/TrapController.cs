using System;
using System.Collections;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    /* Atributos */
    private float maxDistance;
    private PlayerController playerController;
    private Animator anim;

    /* M�todos */
    public void InicializarComponentes()
    {
        playerController = GameManager.Instance.PlayerController;
        anim = GetComponent<Animator>();
        maxDistance = 2f;
    }
    /* M�todo Start */
    private void Start()
    {
        InicializarComponentes();
        StartCoroutine(Activedefense());
    }
    /* M�todo Update */
    private IEnumerator Activedefense()
    {
        while(true)
        {
            anim.SetBool("pinchos", IsPlayerNearTrap());
            yield return GameManager.Instance.EndOfFrame;
        }
    }
    /* M�todo IsPlayerNearEnemy */
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
    /* M�todo IsPlayerNearEnemy */
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().PlayerIsHit(other.contacts[0]);
        }
        
    }
}
