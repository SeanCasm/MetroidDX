using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
public class MotoIA : EnemyBase
{
    #region Properties
    [SerializeField]BoxCollider2D playerDetector;
    private GroundSlopeChecker efd;
    private float currentSpeed;
    private PlayerDetector pD;
    bool detected, prepared;
    #endregion
    #region Unity Methods
    new void Awake()
    {
        base.Awake();
        currentSpeed=speed;
        pD = GetComponentInChildren<PlayerDetector>();
        efd = GetComponentInChildren<GroundSlopeChecker>();
    }
    void Update()
    {
        if (pD.PlayerPosX == transform.position.x && prepared)
        {
            detected = prepared = false;
        }
    }
    private void FixedUpdate()
    {
        if (!detected && !prepared)
        {
            rigid.gravityScale = 1;
            efd.SetOnGroundVelocity(currentSpeed);
        }
        else if (detected && !prepared)
        {
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
            rigid.gravityScale = 0;
        }
        else if (prepared){ efd.SetOnGroundVelocity(currentSpeed *1.85f);rigid.gravityScale = 1; }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player" && col.IsTouching(playerDetector))
        {
            detected = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            prepared = detected = false;
        }
    }
    private void LateUpdate()
    {
        anim.SetBool("detected", detected);
        anim.SetBool("prepared", prepared);
    }
    #endregion
    public void OnRotation()
    {
        currentSpeed *= transform.right.x;
    }
    public void Charged()
    {
        prepared = true;
    }
    public void Chargent()
    {
        prepared = false;
    }

     
}
