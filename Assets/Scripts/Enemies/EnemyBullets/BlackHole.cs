using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
public class BlackHole : Weapon
{
    [SerializeField] CircleCollider2D circleCollider;
    private Animator anim;
    private bool collided;
    new void Awake()
    {
        base.OnEnable();
        anim = GetComponent<Animator>();
        player = References.Player.transform;
        transform.SetParent(null);
        base.SetDirectionAndRotation();
    }
     
    new void FixedUpdate()
    {
        if(!collided)base.FixedUpdate();
        else rigid.velocity=Vector2.zero;
    }
    new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Suelo") && collision.IsTouching(circleCollider))
        {
            anim.SetBool("OnGround", true);
            collided=true;
        }
    }

    public void SearchForSomething()
    {
    }
}
