using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    /* Atributos */
    [SerializeField] private ParticleSystem particleSystemExplosion;
    private PlayerController playerController;
    private Vector3 direction;
    private float speed;
    private Transform tr;

    /* Método InicializarComponentes */
    public void InicializarComponentes()
    {
        playerController = GameManager.Instance.PlayerController;
        tr = GetComponent<Transform>();
        speed = 0.6f;
    }
    /* Método Start */
    public void Start()
    {
        InicializarComponentes();
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
  
    /* Método OnCollisionEnter2D */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            ContactPoint2D point = new ContactPoint2D();
            other.GetComponent<Rigidbody2D>().AddForce(Vector2.one*1.15f,ForceMode2D.Impulse);
            other.GetComponent<PlayerController>().PlayerIsHit(point,1);
            GameManager.Instance.UnsubsCribeObject(gameObject);
            Instantiate(particleSystemExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if(other.gameObject.CompareTag("Ground"))
        {
            GameManager.Instance.UnsubsCribeObject(gameObject);
            Instantiate(particleSystemExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
       
    }

    /* Getters y Setters */
    public Vector3 Direction { get => direction; set => direction = value; }
}
