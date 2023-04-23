using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CofreController : MonoBehaviour
{
    /* Atributos */
    private int points;
    private Animator anim;
    private SpriteRenderer SpriteRenderer;
    private int InitialOrderInlayer;
    private int backOrderInlayer;
    private bool isOpened;

    /* Métodos */
    /* Método InicializarComponentes */
    private void InicializarComponentes()
    {
        points = 100;
        anim = GetComponent<Animator>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InitialOrderInlayer = 5;
        backOrderInlayer = InitialOrderInlayer - 1;
        isOpened = false;
    }
    /* Método Start */
    private void Start()
    {
        // Inicializamos los componentes
        InicializarComponentes();
    }
    /* Método OnTriggerEnter2D */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if((other.GetComponent<PlayerController>().PlayerHasKey) && (!isOpened))
            {
                other.GetComponent<PlayerController>().AddPoints(points);
                anim.SetTrigger("Open");
                isOpened = true;
            }
            SpriteRenderer.sortingOrder = backOrderInlayer;
        }
    }
    /* Método OnTriggerEnter2D */
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SpriteRenderer.sortingOrder = InitialOrderInlayer;
        }
    }
}
