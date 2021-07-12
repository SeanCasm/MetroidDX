using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Enemy;
using System;

public class PirateIA : EnemyBase
{
    #region Properties
    [Header("Pirate settings")]
    [SerializeField] private Collider2D playerDetector;
    [SerializeField] Enemy.Weapon.Pool bulletsPool;
    [Header("Platform detector settings")]
    [SerializeField]Transform platformDetector;
    [SerializeField]Transform fallDetector;
    [SerializeField]float floorDistance,jumpForce,horizontalSpeed,jumpTime;
    [SerializeField] float minAltitude;
    Transform playerTransform;
    private float currentSpeed, horizontalVelocity,ascend;
    private GroundSlopeChecker efd;
    private bool idleShooting,jumping,jumpEnabled;
    #endregion
    #region Unity Methods
    new void Awake()
    {
        base.Awake();
        currentSpeed = speed;
        efd = GetComponent<GroundSlopeChecker>();
    }
    void Update()
    {
        if (pDetect.detected && playerTransform!=null && efd.OnGround)
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
                else if (playerTransform.position.y < transform.position.y - minAltitude)
                {
                    anim.SetTrigger("Shoot D down");
                }
                else anim.SetTrigger("Shoot");

                idleShooting = true;
                Invoke("StartCheck", 2f);
            }
        }
        else if(!pDetect.detected)horizontalVelocity = speed;
        if(!jumping && jumpEnabled){
            OnJump();
        }else if(efd.OnGround && !jumping && !jumpEnabled){
            CheckPlatform();
        }else if(jumping){
            if(!IsInvoking("CheckLanding"))Invoke("CheckLanding",.3f);
        }
    }
    private void LateUpdate() {
        anim.SetBool("Idle", idleShooting);
    }
    private void FixedUpdate()
    {
       if(!jumping){
            if (idleShooting) { rigid.SetVelocity(0f, 0f); }
            else
            {
                if (pDetect.detected) efd.SetOnGroundVelocity(horizontalVelocity * 2f);
                else efd.SetOnGroundVelocity(speed);
            }
       }else{
           rigid.AddForce(new Vector2(horizontalSpeed*efd.dir,jumpForce*ascend));
       }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player" && !jumping)
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
    private void OnJump(){
        ascend=1;
        rigid.velocity = Vector2.zero;
        jumping = true;
        Invoke("StopAscend", jumpTime);
        anim.SetTrigger("Jump");
        playerTransform = null;
        jumpEnabled = efd.enabled = false;
    }
    private void StopAscend(){
        ascend=0;
    }
    private void CheckPlatform(){
        RaycastHit2D hit2D=Physics2D.Raycast(platformDetector.position,-Vector2.up,floorDistance,efd.GroundLayer);
        RaycastHit2D hit2D1=Physics2D.Raycast(fallDetector.position,-Vector2.up,floorDistance,efd.GroundLayer);
        if(hit2D && !hit2D1)jumpEnabled=true;
        else jumpEnabled=false;
    }
    private void CheckLanding(){
        if(Physics2D.Raycast(transform.position,-Vector2.up,floorDistance,efd.GroundLayer)){
            jumping=false;
            efd.enabled = true;
            rigid.velocity=Vector2.zero;
            anim.SetTrigger("Grounded");
        }
    }
     
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