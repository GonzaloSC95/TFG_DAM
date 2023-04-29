using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    /* Atributos */
    private PlayerController playerController;
    private Vector3 direction;
    private float speed;
    private Transform tr;

    /* Método InicializarComponentes */
    public void InicializarComponentes()
    {
        playerController = GameManager.Instance.PlayerController;
        tr = GetComponent<Transform>();
        direction = -Vector3.right;
        speed = 0.6f;
    }
    /* Método Start */
    public void Start()
    {
        InicializarComponentes();
        SetDirecction();
        StartCoroutine(OnShoot());
    }
    /* Método OnShoot */
    private IEnumerator OnShoot()
    {
        while(gameObject.activeSelf)
        {
            tr.position += direction * (speed * Time.deltaTime);
            // Si el objeto se encuentra fuera de la camara principal, desparece
            GameManager.Instance.UnsubsCribeObjectIsoutOfMainCamera(gameObject);
            yield return GameManager.Instance.EndOfFrame;
        }     
    }
    private void SetDirecction()
    {
        Vector3 directionToPlayer = playerController.transform.position - transform.position;
        if (directionToPlayer.x < 0)
        {
            tr.localScale = new Vector3(1, 1, 1);
            direction = -Vector3.right;
        }
        else
        {
            tr.localScale = new Vector3(-1, 1, 1);
            direction = Vector3.right;
        }
    }
    /* Método OnCollisionEnter2D */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            ContactPoint2D point = new ContactPoint2D();
            other.GetComponent<Rigidbody2D>().AddForce(Vector2.one*1.15f,ForceMode2D.Impulse);
            other.GetComponent<PlayerController>().PlayerIsHit(point,1);
            GameManager.Instance.UnsubsCribeObject(gameObject);
            Destroy(gameObject);
        }
        if(other.gameObject.CompareTag("Ground"))
        {
            GameManager.Instance.UnsubsCribeObject(gameObject);
            Destroy(gameObject);
        }
       
    }
}
