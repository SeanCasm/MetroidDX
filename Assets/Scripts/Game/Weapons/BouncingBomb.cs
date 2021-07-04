using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Weapon;
public class BouncingBomb : Bomb
{
    [SerializeField] float speed;
    [SerializeField] bool pooleable;
    private Rigidbody2D rigid;
    public bool Pooleable => pooleable;
    public Transform parent { get; set; }
    Vector2 direction,lastVelocity;
    private float currentSpeed;
    new void Start(){
        base.Start();
        currentSpeed=speed;
    }
    private void OnEnable() {
        Invoke("Explode",timeToExplode);
        Invoke("BackToGun", livingTime);
        GameEvents.overHeatAction.Invoke(hotPoints);
        direction = parent.right;
        transform.eulerAngles = parent.eulerAngles;
    }
    void FixedUpdate()
    {
        lastVelocity = rigid.velocity;
        rigid.velocity = direction.normalized * currentSpeed;
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Suelo")
        {
            var speed = lastVelocity.magnitude;
            direction = Vector2.Reflect(lastVelocity.normalized, col.contacts[0].normal);
            rigid.SetVelocity(direction * Mathf.Max(currentSpeed, 0f));
            if (direction.x < 0)transform.eulerAngles = new Vector3(0, 180, 0);
            else transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
    new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
    void Explode(){
        animator.SetTrigger("Explode");
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
