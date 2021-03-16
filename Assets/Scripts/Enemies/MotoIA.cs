using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
public class MotoIA : EnemyBase
{
    #region Properties
    [SerializeField]BoxCollider2D playerDetector;
    private GroundChecker efd;
    private float currentSpeed;
    private PlayerDetect pD;
    bool detected, prepared;
    #endregion
    #region Unity Methods
    new void Awake()
    {
        base.Awake();
        currentSpeed=speed;
        pD = GetComponentInChildren<PlayerDetect>();
        efd = GetComponentInChildren<GroundChecker>();
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
            efd.SetOnGroundVelocity(currentSpeed);
        }
        else if (detected && !prepared)
        {
            rigid.velocity = new Vector2(0f, rigid.velocity.y);
        }
        else if (prepared) efd.SetOnGroundVelocity(currentSpeed *1.5f);
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

    public void SetOnGroundVelocity(float amount)
    {
        if (!efd.OnSlope)
        {
            if (efd.FacingRight) rigid.velocity = new Vector2(amount, rigid.velocity.y);
            else rigid.velocity = new Vector2(-amount, rigid.velocity.y);
        }
        else
        {
            if (efd.FacingRight) rigid.velocity = new Vector2(amount * efd.SlopePerp.x * -1, amount *efd.SlopePerp.y*-1);
            else rigid.velocity = new Vector2(-amount * efd.SlopePerp.x * -1, amount * efd.SlopePerp.y);
        }
    }
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
