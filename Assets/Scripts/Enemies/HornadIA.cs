using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
using Enemy;
public class HornadIA : EnemyBase
{
    public GameObject playerDetector;
    Vector2 direction;
    public float visionLenght=2f;
    bool facingRight;
    private bool attacking = true,doJump=false;
    public LayerMask wallLayer,playerLayer;
    private float currentSpeed;
    public GameObject bulletPrefab;
    public Transform _firePoint;
    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        if (transform.localScale.x > 0){facingRight = true;currentSpeed=speed;}
        else{currentSpeed=-speed; facingRight = false;}
    }

    // Update is called once per frame
    void Update()
    {
        if (!eh.freezed)
        {
            if (!facingRight) { direction = Vector2.left; }
            else { direction = Vector2.right; }

            if (Physics2D.Raycast(transform.position, direction, 0.2f, wallLayer))
            {
                Flip();
            }
            if (Physics2D.Raycast(transform.position, direction * -1f, 0.6f, playerLayer))
            {
                Flip();
            }

            if (Physics2D.Raycast(transform.position, direction, visionLenght, playerLayer)
            && Physics2D.Raycast(transform.position, Vector2.down, 0.2f, wallLayer))
            {
                attacking = true;
            }
            else
            {
                attacking = false;
            }
            if (!IsInvoking("RandomMovement"))Invoke("RandomMovement", 2f);
        }
        
    }
   void RandomMovement()
    {
        int i = Random.Range(1, 5);
        if(i==1 || i == 2)
        {
            anim.SetTrigger("Moving");
        }
    }
    Transform playerTransform;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !GetComponentInChildren<EnemyHealth>().freezed)
        {
            playerTransform = col.transform;
            attacking = true;
        }
    }
    void OntriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player") )
        {
            attacking = false;
        }
    }
    void LateUpdate()
    {
        anim.SetBool("DetectedClose", attacking);
    }
    private void FixedUpdate() {
        if(doJump){
            rigid.gravityScale=0;
            rigid.SetVelocity(speed* Time.deltaTime,jumpForce*Time.deltaTime);
        }else{
            rigid.gravityScale = 1;
        }
    }
    void Flip()
    {
        facingRight =! facingRight;
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1f;
        currentSpeed*=-1f;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    public float jumpForce=2f;
    public void Jump()
    {
        doJump=!doJump;
    }
    public void Shoot()
    {
        GameObject myBullet = Instantiate(bulletPrefab, _firePoint.position, Quaternion.identity) as GameObject;
        Throw bulletComponent = myBullet.GetComponent<Throw>();
        bulletComponent.ThrowPrefab(_firePoint, playerTransform);
    }
}
