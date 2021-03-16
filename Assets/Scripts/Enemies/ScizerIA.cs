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
    Weapon pF;
    Transform player;
    new void Awake()
    {
        base.Awake();
    }
    public void Attack()
    {
        if (!eh.freezed)
        {
            if (player.position.x < transform.position.x)
            {
                GameObject mb = Instantiate(scizerBulltet, shootPoints[1].position, Quaternion.identity) as GameObject;
                pF = mb.GetComponent<Weapon>();
                pF.Throw(shootPoints[1], player);
            }
            else if (player.position.x > transform.position.x)
            {
                GameObject mb = Instantiate(scizerBulltet, shootPoints[0].position, Quaternion.identity) as GameObject;
                pF = mb.GetComponent<Weapon>();
                pF.Throw(shootPoints[0], player);
            }
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Player" && !eh.freezed)
        {
            player = col.transform;
            if (col.IsTouching(boxCollider))
            {
                anim.SetBool("Attacking", true);
            }
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            if (!col.IsTouching(boxCollider))
            {
                player = null;
                anim.SetBool("Attacking", false);
            }
        }
    }
}
