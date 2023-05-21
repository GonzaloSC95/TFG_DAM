using System;
using System.Collections;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    /* Atributos */
    private float maxDistance;
    private PlayerController playerController;
    private Animator anim;
    private AudioSource audio;

    /* M�todos */
    public void InicializarComponentes()
    {
        playerController = GameManager.Instance.PlayerController;
        audio = GetComponent<AudioSource>();
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
    /* M�todo Update */
    private void PlaySound()
    {
        audio.Play();
    }
    /* M�todo IsPlayerNearEnemy */
    private bool IsPlayerNearTrap()
    {
        if (playerController.Life <= 0 || playerController.IsPlayerWinner == true) return false;
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
}
