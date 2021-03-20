using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
using Enemy;
public class EspinaIA : EnemyBase
{
    private float currentSpeed;
    private GroundChecker efd;
    public GameObject bulletPrefab;
    private bool _isAttacking;
    GameObject[] bulletArray = new GameObject[5];
    Weapon[] bulletComponent = new Weapon[5];
    new void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        currentSpeed = speed;
        efd = GetComponent<GroundChecker>();
    }
    void Update()
    {
        if (_isAttacking) currentSpeed = 0f;
        else currentSpeed = speed;
    }
    void FixedUpdate()
    {
        efd.SetOnGroundVelocity(currentSpeed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))anim.SetBool("Attack",_isAttacking=true);
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))anim.SetBool("Attack", _isAttacking=false);
    } 
    public void Shoot()
    {
        for(int i = 0; i < 5; i++)
        {
            bulletArray[i]= Instantiate(bulletPrefab, transform.position, Quaternion.identity) as GameObject;
            bulletComponent[i] = bulletArray[i].GetComponent<Weapon>();
        }
        int angle=0;
        for(int i=0;i<bulletComponent.Length;i++){
            bulletComponent[i].SetDirectionAndRotation(angle);
            angle+=45;
        }
    }
}
