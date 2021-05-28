﻿using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Enemy;
public class PirateIA : EnemyBase
{
    #region Properties
    [Header("Pirate settings")]
    [SerializeField] private Collider2D playerDetector;
    [SerializeField] Enemy.BulletPool.Pool bulletsPool;
    Transform playerTransform;
    private float currentSpeed, horizontalVelocity;
    private GroundChecker efd;
    private bool idleShooting;
    private const float minAltitude = 0.5f;
    public LayerMask playerLayer;
    #endregion
    #region Unity Methods
    new void Awake()
    {
        base.Awake();
        currentSpeed = speed;
        efd = GetComponent<GroundChecker>();
    }
    void Update()
    {
        if (pDetect.detected && playerTransform!=null)
        {
            if (playerTransform.position.x < transform.position.x)
            {
                if (efd.FacingRight) { efd.Flip(); }
            }
            else if (playerTransform.position.x > transform.position.x)
            {
                if (!efd.FacingRight) { efd.Flip(); }
            }
            if (!idleShooting)
            {
                if (playerTransform.position.y >= transform.position.y + minAltitude)
                {
                    anim.SetTrigger("Shoot D up");
                }
                else if (playerTransform.position.y <= transform.position.y - minAltitude)
                {
                    anim.SetTrigger("Shoot D down");
                }
                else anim.SetTrigger("Shoot");

                idleShooting = true;
                Invoke("StartCheck", 2f);
            }
        }
        else horizontalVelocity = speed;
    }
    private void LateUpdate() {
        anim.SetBool("Idle", idleShooting);
    }
    private void FixedUpdate()
    {
        if (idleShooting){rigid.SetVelocity(0f, 0f);}
        else
        {
            if (pDetect.detected) efd.SetOnGroundVelocity(horizontalVelocity * 1.2f);
            else efd.SetOnGroundVelocity(speed);
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (col.IsTouching(playerDetector))playerTransform = col.transform;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.IsTouching(playerDetector) && col.tag == "Player")
        {
            playerTransform = null;
            idleShooting=false;
        }
    }
    #endregion
     
    void StartCheck(){
        idleShooting =false;
    }
    public void ShootEvent()
    {
        foreach(Transform element in bulletsPool.ShootPoint){
            
            bulletsPool.ActiveNextPoolObject();
        }
    }
}
