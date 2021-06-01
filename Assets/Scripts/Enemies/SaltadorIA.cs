using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
public class SaltadorIA : EnemyBase
{
    [SerializeField]LayerMask groundLayer;
    [SerializeField]Collider2D playerDetector;
    private bool detected=false,doJump=false,attacking;
    public float jumpForce = 2f;

    private Jumper jumper;
    private bool grounded = true;
    private Transform player;
    private void OnEnable() {
        jumper.OnJump+=OnJump;
        jumper.OutJump += OutJump;
    }
    private void OnDisable() {
        jumper.OnJump -= OnJump;
        jumper.OutJump -= OutJump;
    }

    new void Awake()
    {
        base.Awake();
        jumper = GetComponent<Jumper>();
    }
    void Start()
    {
        if (transform.localScale.x > 0) { jumper.facingRight = true; }
        else { jumper.facingRight = false; }
    }
    private void Update() {
        if (!eh.freezed)
        {

            if (Physics2D.Raycast(transform.position, jumper.direction, 0.2f, groundLayer))
            {
                jumper.Flip();
                speed*=-1;
            }
            if (!IsInvoking("RandomMovement") && !detected && grounded) Invoke("RandomMovement", 2f);
            //if(!jumper.doJump)groundDetector();
        }
    }
    void RandomMovement()
    {
        int i = Random.Range(1, 3);
        if (i == 1 || i == 3)
        {
            anim.SetTrigger("Jump");
        }
    }
    private void OnJump(){
        rigid.gravityScale = 0;
    }
    private void OutJump(){
        rigid.gravityScale = 1;
    }
    /*void LateUpdate()
    {
        anim.SetBool("Grounded", grounded);
    }*/
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !eh.freezed)
        {
            detected = true;
            player = col.transform;
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !eh.freezed)
        {
            detected = true;
            player = col.transform;
            if(grounded)anim.SetTrigger("Jump");
            if (player.transform.position.x < transform.position.x && jumper.facingRight)
            {
                jumper.Flip();
                speed *= -1;
            }
            else if (player.transform.position.x > transform.position.x && !jumper.facingRight)
            {
                jumper.Flip();
                speed *= -1;
            }

        }
    }
    private void FixedUpdate()
    {
        if (jumper.doJump)
        {
            rigid.SetVelocity(speed * Time.deltaTime, jumpForce * Time.deltaTime);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            detected = false;
            player=null;
        }
    }
    void groundDetector()
    {
        if(Physics2D.Raycast(transform.position, Vector2.down, 0.4f, groundLayer))grounded = true;
        else grounded = false;
    }
}