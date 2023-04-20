using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    /* Atributos */
    [SerializeField] Transform target;

    public Vector3 offset;
    private Transform tr;
    public float offsetDirection=1;

    /* Métodos */
    /* Método Start */
    private void Start()
    {
        tr = this.transform;
        offset = new Vector3(0,tr.position.y,tr.position.z);
    }
    /* Método Awake */
    private void Awake()
    {
        GameManager.instance.CameraControllerInstance=this;
    }
    /* Método Update */
    void Update()
    {
        //Movimiento horizontal en base al player
        offset.x = target.position.x + offsetDirection;
        //Movimiento vertical en base al player
        offset.y = target.position.y + offsetDirection;
        //Se suaviza la transición de la cámara utilizando la función Lerp
        tr.position=Vector3.Lerp(tr.position,offset,Time.deltaTime*5);
    }
}
