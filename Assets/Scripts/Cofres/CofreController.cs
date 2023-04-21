using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CofreController : MonoBehaviour
{
    /* Atributos */
    private int points;
    private Animator anim;

    /* Métodos */
    /* Método InicializarComponentes */
    private void InicializarComponentes()
    {
        points = 100;
        anim = GetComponent<Animator>();
    }
    /* Método Start */
    private void Start()
    {
        // Inicializamos los componentes
        InicializarComponentes();
    }
    /* M�todo OnTriggerEnter2D */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(other.GetComponent<PlayerController>().PlayerHasKey)
            {
                other.GetComponent<PlayerController>().AddPoints(points);
                anim.SetTrigger("Open");
            }
        }
    }
}
