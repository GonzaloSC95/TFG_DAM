using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour
{
    /* Atributos */
    private SpriteRenderer SpriteRenderer;
    private int InitialOrderInlayer;
    private int backOrderInlayer;
    private bool IsEnd;
    /* Métodos */
    /* Método InicializarComponentes */
    private void InicializarComponentes()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InitialOrderInlayer = 5;
        backOrderInlayer = InitialOrderInlayer - 1;
        IsEnd = gameObject.CompareTag("End");
    }
    /* Método Start */
    private void Start()
    {
        InicializarComponentes();
    }
    /* Método OnTriggerEnter2D */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Player")) && (IsEnd))
        {
            //Aqui se controla la vitoria del Player
            //other.gameObject.GetComponent<PlayerController>().PlaySound("Win"); TODO
            other.gameObject.GetComponent<PlayerController>().PlayerWin();
            SpriteRenderer.sortingOrder = backOrderInlayer;
        }
    }
    /* Método OnTriggerEnter2D */
    private void OnTriggerExit2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Player")) && (IsEnd))
        {
            SpriteRenderer.sortingOrder = InitialOrderInlayer;
        }
    }
}
