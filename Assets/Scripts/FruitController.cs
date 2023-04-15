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
    /* Método Awake*/
    private void Awake()
    {
        // Obtenemos la referencia al jugador
        playerScript = FindObjectOfType<PlayerController>();
        // Asignamos los puntos de la fruta
        SetPoints();
    }

    /* Método Start */
    private void Start()
    {
    }

    /* Método OnTriggerEnter2D */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
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

