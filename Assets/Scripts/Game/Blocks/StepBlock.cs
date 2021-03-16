using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepBlock : DestroyableBlock
{
    private BoxCollider2D box;
    private Rigidbody2D rigid;
    new void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }
    new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Utilities.CompareYVectors(collision.transform.position,transform.position))
        {
            anim.SetBool("Hide", true);
            box.isTrigger = true;
            rigid.simulated = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Invoke("ResetState", 2f);
        }
    }

    private void ResetState()
    {
        anim.SetBool("Hide", false);
        box.isTrigger = false;
        rigid.simulated = true;
    }
}