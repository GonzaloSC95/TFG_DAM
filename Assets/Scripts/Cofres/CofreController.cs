using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CofreController : MonoBehaviour
{
    /* Atributos */
    private int points;
    private Animator anim;

    /* Métodos */
    /* Método Start */
    private void Start()
    {
        points = 100;
        anim = GetComponent<Animator>();
    }
    /* Método OnTriggerEnter2D */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(other.GetComponent<PlayerController>().getPlayerHaskey())
            {
                other.GetComponent<PlayerController>().AddPoints(points);
                anim.SetTrigger("Open");
            }
        }
    }
}
