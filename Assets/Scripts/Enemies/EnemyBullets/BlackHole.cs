using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
public class BlackHole : Weapon
{
    [SerializeField] CircleCollider2D circleCollider;
    private Animator anim;
    new void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
    }
    new void Start()
    {
        base.Start();
        base.SetDirection(transform);
    }
    new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    new void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Suelo") && collision.IsTouching(circleCollider))
        {
            anim.SetBool("OnGround", true);
        }
    }

    public void SearchForSomething()
    {
    }
}
