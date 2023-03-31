using System;
using UnityEngine;
using System.Collections;

public class PatrolEnemy : Enemy
{

    [SerializeField] float speed;
    [SerializeField]Collider2D col;
    [SerializeField]Rigidbody2D rb;
    [SerializeField] Animator anim;

    public override void OnHit()
    {
       StartCoroutine( Die());
    }

    

    private void Update()
    {
        tr.position += direction * (speed * Time.deltaTime);
    }
    public override void OnWallCollide()
    {
        ChangeDirection();
    }
    private IEnumerator Die(){
        col.enabled=false;
        rb.simulated=false;
        speed=0;
        SpriteRenderer rend=this.GetComponent<SpriteRenderer>();
        anim.SetTrigger("Hit");
        yield return new WaitForSeconds(0.5f);
        WaitForEndOfFrame endOfFrame= new WaitForEndOfFrame();
        Color color=new Color(1,1,1,1);
        //Make it fade
        for (int i = 0; i < 5; i++)
        {
          
            for (float j = 0; j < 1; j+=Time.deltaTime*10)
            {
                color.a=Mathf.Lerp(0,1,j);
                rend.color=color;
                yield return endOfFrame;
            } 
             for (float j = 0; j < 1; j+=Time.deltaTime*10)
            {
                color.a=Mathf.Lerp(1,0,j);
                rend.color=color;
                yield return endOfFrame;
            }
        }
        GameManager.instance.UnsuscribeEnemy(this);
        this.gameObject.SetActive(false);
    }
   

}