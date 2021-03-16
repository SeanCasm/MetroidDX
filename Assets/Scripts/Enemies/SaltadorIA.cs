using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
public class SaltadorIA : EnemyBase
{
    private bool detected=false;
    private bool grounded = true;
    private Transform player;
    public LayerMask groundLayer;

    new void Awake()
    {
        base.Awake();
    }
    void LateUpdate()
    {
        anim.SetBool("Jump", detected);
        anim.SetBool("Grounded", grounded);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !GetComponentInChildren<EnemyHealth>().freezed)
        {
            detected = true;
            player = col.transform;
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !eh.freezed)
        {
            groundDetector();
            detected = true;
            if (player.transform.position.x < transform.position.x && _facingRight)
            {
                Flip();
            }
            else if (player.transform.position.x > transform.position.x && _facingRight==false)
            {
                Flip();
            }

        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            detected = false;
        }
    }
    void groundDetector()
    {
        if(Physics2D.Raycast(transform.position, Vector2.down, 0.4f, groundLayer))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
    public float jumpForce = 2f;
    public void AddForceToY()
    {
        rigid.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    private bool _facingRight=true;
    private void Flip()
    {
        _facingRight = !_facingRight;
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    
}
