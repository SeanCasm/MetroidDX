using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
using Enemy;
public class HornadIA : EnemyBase
{
    [SerializeField]Collider2D closeDetector,playerDetector;
    private Jumper jumper;
    public float jumpForce = 2f;

    private bool attacking = true,detectedClose;
    public LayerMask wallLayer,playerLayer;
    Transform playerTransform;

    public GameObject bulletPrefab;
    public Transform _firePoint;
    private void OnEnable()
    {
        jumper.OnJump += OnJump;
        jumper.OutJump += OutJump;
    }
    private void OnDisable()
    {
        jumper.OnJump -= OnJump;
        jumper.OutJump -= OutJump;
    }
    new void Awake()
    {
        base.Awake();
        jumper=GetComponent<Jumper>();
    }
    void Start()
    {
        if (transform.localScale.x > 0){jumper.facingRight = true;}
        else{jumper.facingRight = false;}
    }
    void Update()
    {
        if (!eh.freezed)
        {
            if (Physics2D.Raycast(transform.position, jumper.direction, 0.2f, wallLayer))SwapDirection();
            if (Physics2D.Raycast(transform.position, jumper.direction * -1f, 0.6f, playerLayer))SwapDirection();
            if(detectedClose){
                if (playerTransform.position.x < transform.position.x && jumper.facingRight)SwapDirection();
                else if (playerTransform.position.x > transform.position.x && !jumper.facingRight)SwapDirection();
            }
             
            if (!IsInvoking("RandomMovement") && !detectedClose && !attacking)Invoke("RandomMovement", 2f);
            if(attacking)anim.SetTrigger("Moving");
        }
        
    }
    private void SwapDirection(){
        jumper.Flip();
        speed *= -1;
    }
    private void OnJump()
    {
        rigid.gravityScale = 0;
    }
    private void OutJump()
    {
        rigid.gravityScale = 1;
    }
   void RandomMovement()
    {
        int i = Random.Range(1, 5);
        if(i==1 || i == 2)
        {
            anim.SetTrigger("Moving");
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !eh.freezed)
        {
            if(col.IsTouching(playerDetector)){
                attacking = true;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player")&&other.IsTouching(closeDetector)){
            attacking = false;
            playerTransform = other.transform;
            detectedClose = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            if(col.IsTouching(playerDetector)){
                detectedClose=attacking = false;
            }
        }
    }
    void LateUpdate()
    {
        anim.SetBool("DetectedClose", detectedClose);
    }
    private void FixedUpdate() {
        if(jumper.doJump){
            rigid.SetVelocity(speed* Time.deltaTime,jumpForce*Time.deltaTime);
        }
    }
  
    public void Shoot()
    {
        GameObject myBullet = Instantiate(bulletPrefab, _firePoint.position, Quaternion.identity) as GameObject;
        Throw bulletComponent = myBullet.GetComponent<Throw>();
        print(_firePoint);
        bulletComponent.ThrowPrefab(_firePoint, playerTransform);
    }
}
