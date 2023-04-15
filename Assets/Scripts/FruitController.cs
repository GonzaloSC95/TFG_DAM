using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    /* Atributos */
    /* Puntos que aporta la fruta */
    private int points;
    /* Referencia al jugador */
    private PlayerController playerScript;

    /* Métodos */
    /* Método Start */
    private void Start()
    {
        // Obtenemos la referencia al jugador
        playerScript = FindObjectOfType<PlayerController>();
        // Asignamos los puntos de la fruta
        SetPoints();
        //-------------------------------
        Debug.Log("-----------------Start Fruit------------- " + gameObject.tag + " --> POINTS = " + points);
    }

    /* Método OnTriggerEnter2D */
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("-----------------Trigger-------------");
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has collided with a fruit");
            // Añadimos los puntos al jugador
            playerScript.AddPoints(points);
            // Destruimos la fruta
            Destroy(gameObject);
        }
    }

    /* Método SetPoints */
    private void SetPoints()
    {
        switch (gameObject.tag)
        {
            case "Melon":
                points = 30;
                break;
            case "Kiwi":
                points = 10;
                break;
            default:
                points = 5;
                break;
        }
    }
}

