using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
public class AvispaIA : EnemyBase
{
    float direction;
    private bool canMove;
    private EnemyPool enemyPool;
    private Vector2 movePos;
    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        enemyPool = GetComponentInParent<EnemyPool>();
        canMove = false;
        direction= Mathf.Sign(enemyPool.Target.position.y-transform.position.y);
    }
    // Update is called once per frame
    void Update()
    {
        if (enemyPool.MoveT)
        {
            movePos = new Vector2(transform.position.x,transform.position.y+direction* enemyPool.upSpeed);
            transform.position = movePos;
        }
        if(transform.position.y>= enemyPool.Target.position.y)
        {
            canMove = true;
            enemyPool.MoveT = false;
        }
    }
    void FixedUpdate()
    {
        if (canMove)
        {
            if (transform.localScale.x == -1f)
            {
                rigid.MovePosition(rigid.position + new Vector2(-speed, 0f) * Time.fixedDeltaTime);
            }
            else
            {
                rigid.MovePosition(rigid.position + new Vector2(speed, 0f) * Time.fixedDeltaTime);
            }
        }
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
