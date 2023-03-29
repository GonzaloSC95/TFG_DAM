using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform target;
    Vector3 offset;
    Transform tr;
    public float offsetDirection=1;

    private void Start()
    {
        tr=this.transform;
        offset=new Vector3(0,tr.position.y,tr.position.z);
    }
    
    private void Awake()
    {
        GameManager.instance.CameraControllerInstance=this;
    }
    // Update is called once per frame
    void Update()
    {
        offset.x=target.position.x+offsetDirection;
        tr.position=Vector3.Lerp(tr.position,offset,Time.deltaTime*5);
    }
}
