using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Weapon;
public class BouncingBomb : Bomb,IPooleable
{
    [SerializeField] float speed;
    [SerializeField] bool pooleable;
    private Collider2D collider2D;
    private Rigidbody2D rigid;
    private bool exploding;
    public bool Pooleable => pooleable;
    public Transform parent { get; set; }
    bool IPooleable.pooleable { get => this.pooleable; set => this.pooleable=value; }

    Vector2 direction,lastVelocity;
    private float currentSpeed;
    new void Awake() {
        base.Awake();
        collider2D=GetComponent<Collider2D>();
        rigid=GetComponent<Rigidbody2D>();
    }
    new void OnEnable(){
        base.OnEnable();
        currentSpeed = speed;
        Invoke("Explode",timeToExplode);
        Invoke("BackToGun", livingTime);
        direction = parent.right;
        transform.eulerAngles = parent.eulerAngles;
    }
    void FixedUpdate()
    {
        if(!exploding){
            lastVelocity = rigid.velocity;
            rigid.velocity = direction.normalized * currentSpeed;
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Suelo")
        {
            var speed = lastVelocity.magnitude;
            direction = Vector2.Reflect(lastVelocity.normalized, col.contacts[0].normal);
            rigid.SetVelocity(direction * Mathf.Max(currentSpeed, 0f));
        }
    }
    new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
    void Explode(){
        animator.SetTrigger("Explode");
        exploding=true;
        collider2D.isTrigger=true;
        rigid.velocity=Vector2.zero;
    }
    new void PlayExplosion(){
        base.PlayExplosion();
    }
    private void BackToGun()
    {
        if (!pooleable)
        {
            Destroy(gameObject);
        }else{
            transform.SetParent(parent);
            transform.position = parent.position;
            gameObject.SetActive(false);
        }
    }

}
