using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
using Enemy;
public class ScizerIA : EnemyBase
{
    [SerializeField]BoxCollider2D boxCollider;
    [SerializeField]GameObject scizerBulltet;
    [SerializeField]Transform[] shootPoints;
    Throw pF;
    private GroundChecker efd;
    Transform player;
    bool attacking;
    new void Awake()
    {
        base.Awake();
        efd = GetComponent<GroundChecker>();
    }
    private void FixedUpdate() {
        if(!attacking)efd.SetOnGroundVelocity(speed);
        else rigid.SetVelocity(0f, 0f);
    }
    public void Attack()
    {
        if (!eh.freezed)
        {
            if (player.position.x < transform.position.x)
            {
                GameObject mb = Instantiate(scizerBulltet, shootPoints[1].position, Quaternion.identity) as GameObject;
                pF = mb.GetComponent<Throw>();
                pF.ThrowPrefab(shootPoints[1], player);
            }
            else if (player.position.x > transform.position.x)
            {
                GameObject mb = Instantiate(scizerBulltet, shootPoints[0].position, Quaternion.identity) as GameObject;
                pF = mb.GetComponent<Throw>();
                pF.ThrowPrefab(shootPoints[0], player);
            }
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player" && !eh.freezed)
        {
            player = col.transform;
            attacking=true;
            if (col.IsTouching(boxCollider))
            {
                anim.SetBool("Attacking", attacking);
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (!col.IsTouching(boxCollider))
            {
                attacking=false;
                player = null;
                anim.SetBool("Attacking", attacking);
            }
        }
    }
}
