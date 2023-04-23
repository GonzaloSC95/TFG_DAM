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
    /* M�todos */
    /* M�todo InicializarComponentes */
    private void InicializarComponentes()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        InitialOrderInlayer = 5;
        backOrderInlayer = InitialOrderInlayer - 1;
        IsEnd = gameObject.CompareTag("End");
    }
    /* M�todo Start */
    private void Start()
    {
        InicializarComponentes();
    }
    /* M�todo OnTriggerEnter2D */
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
    /* M�todo OnTriggerEnter2D */
    private void OnTriggerExit2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("Player")) && (IsEnd))
        {
            SpriteRenderer.sortingOrder = InitialOrderInlayer;
        }
    }
}
