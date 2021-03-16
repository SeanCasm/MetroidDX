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
    private bool attacking = true;
    public LayerMask wallLayer,playerLayer;
    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        if (transform.localScale.x > 0) facingRight = true;
        else facingRight = false;
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
            if (!IsInvoking("MovingRandom"))Invoke("MovingRandom", 2f);
        }
        
    }
    
    bool moving;
   void MovingRandom()
    {
        int i = Random.Range(1, 5);
        if(i==1 || i == 2)
        {
            moving = true;
        }
        else
        {
            moving = false;
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
        anim.SetBool("Moving", moving);
        anim.SetBool("DetectedClose", attacking);
    }
    void Flip()
    {
        facingRight =! facingRight;
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1f;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    public float jumpForce=2f;
    public void AddForceToY()
    {
        rigid.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
    }
    public GameObject bulletPrefab;
    public Transform _firePoint;
    public void Shoot()
    {
        GameObject myBullet = Instantiate(bulletPrefab, _firePoint.position, Quaternion.identity) as GameObject;
        Weapon bulletComponent = myBullet.GetComponent<Weapon>();
        bulletComponent.Throw(_firePoint, playerTransform);
    }
}
