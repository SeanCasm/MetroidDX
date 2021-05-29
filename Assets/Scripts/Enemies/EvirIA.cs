using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
using Enemy.Weapon;
public class EvirIA : EnemyBase
{
    [SerializeField]Transform shootPoint;
    [SerializeField]BoxCollider2D detector,collision;
    [SerializeField]Pool bulletsPool;
    private Transform player;
    private float currentSpeed;
    private bool playerDetected;
    new void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        currentSpeed = speed;
    }
    void Update()
    {
        if (playerDetected)
        {
            if (player.position.x < transform.position.x && transform.localScale.x>0)
            {
                transform.localScale=new Vector2(-1f,1f);
            }else if(player.position.x > transform.position.x && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(1f, 1f);
            }
        }
    }
    void FixedUpdate()
    {
        if (playerDetected) rigid.velocity = new Vector2(0f, 0f)*Time.deltaTime;
        else rigid.velocity = new Vector2(0f, currentSpeed) * Time.deltaTime;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag=="Player" && col.IsTouching(detector))
        {
            playerDetected = true;
            anim.SetBool("Attack", true);
            player = col.transform;
        }else if(col.tag=="Suelo" && col.IsTouching(collision))
        {
            currentSpeed *= -1f;
        }
    }
    public void Shoot()
    {
        bulletsPool.ActiveNextPoolObject();
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player" && col.IsTouching(detector))
        {
            playerDetected = false;
            anim.SetBool("Attack", false);
            player = null;
            currentSpeed = speed;
        }
    }
}
