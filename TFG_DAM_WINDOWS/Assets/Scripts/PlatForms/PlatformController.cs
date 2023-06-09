using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour {

    /* Atributos */
    public float speed = 0.75f; // Velocidad de movimiento
    public float distance = 1f; // Distancia total de movimiento
    public float direction; // Dirección de movimiento
    private float startingPosition; // Posición inicial del objeto

    /* Métodos */
    /* Método  Awake*/
    private void Awake()
    {
        if (CompareTag("HoriontalPlatform"))
        {
            startingPosition = transform.position.x; // Guardar la posición inicial del objeto
        }
        else if (CompareTag("VerticalPlatform"))
        {
            startingPosition = transform.position.y; // Guardar la posición inicial del objeto
        }
    }
    /* Método FixedUpdate */
    private void FixedUpdate()
    {
        if (CompareTag("HoriontalPlatform"))
        {
            MoveHorizontal();
        }
        else if (CompareTag("VerticalPlatform"))
        {
            MoveVertical();
        }
    }
    /* Método Start */
    private void Start() 
    {

    }
    /* Método Update */
    void Update () 
    {

    }
    /* Método MoveHorizontal */
    void MoveHorizontal()
    {
        // Cambiar la dirección de movimiento si el objeto alcanza el límite de distancia
        if ((transform.position.x >= (startingPosition + distance/2f)) 
        || (transform.position.x <= (startingPosition - distance/2f))) {
            direction = -direction;
        }

        // Calcular la nueva posición del objeto
        transform.position += new Vector3((direction * speed * Time.deltaTime), 0f, 0f);
    }
    /* Método MoveVertical */
    void MoveVertical()
    {
        // Cambiar la dirección de movimiento si el objeto alcanza el límite de distancia
        if ((transform.position.y >= (startingPosition + distance/2f)) 
        || (transform.position.y <= (startingPosition - distance/2f))) {
            direction = -direction;
        }

        // Calcular la nueva posición del objeto
        transform.position += new Vector3(0f, (direction * speed * Time.deltaTime), 0f);
    }
    /* Método OnCollisionEnter2D */
    private void OnCollisionEnter2D(Collision2D other) 
    {
        // Si la plataforma colisiona con un objeto, este se convierte en hijo de la plataforma
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            other.transform.parent = transform;
        }
        
    }
    /* Método OnCollisionExit2D */
    private void OnCollisionExit2D(Collision2D other) 
    {
        // Si la plataforma deja de colisionar con un objeto, este deja de ser hijo de la plataforma
        if(other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Enemy"))
        {
            other.transform.parent = null;
        }
    }
}

