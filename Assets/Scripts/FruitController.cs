using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    /* Atributos */
    /* Puntos que aporta la fruta */
    private int points;
    /* Referencia al jugador */
    private PlayerController player;
    /* Animaciones */
    private Animator anim;
    private float timeBeforeDestroy = 0.5f;

    /* Métodos */
    /* Método Awake*/
    private void Awake()
    {
        // Obtenemos la referencia al jugador
        player = FindObjectOfType<PlayerController>();
        // Obtenemos el compoenente Animator
        anim = GetComponent<Animator>();
        // Asignamos los puntos de la fruta
        SetPoints();
    }

    /* Método OnTriggerEnter2D */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Ejecutamos la animación collected
            anim.SetTrigger("collected");
            // Emitimos el sonido de recolección
            player.PlaySound("fruit");
            // Añadimos los puntos al jugador
            player.AddPoints(points);
            // Destruimos la fruta
            Invoke("DestroyFruit", timeBeforeDestroy);
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

    /* Método DestroyFruit */
    private void DestroyFruit()
    {
        Destroy(gameObject);
    }
}

