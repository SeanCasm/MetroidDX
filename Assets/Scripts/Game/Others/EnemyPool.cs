using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float upSpeed;
    public float respawnTiming;
    bool moveTowards;
    public bool MoveT { get { return moveTowards; } set { moveTowards = value; } }
    Transform target;
    public Transform Target { get { return target; } }
    bool invoked = false, detected;
    void ins()
    {
        invoked = false;
    }
    void func()
    {
        if (invoked) Invoke("ins", respawnTiming);
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("PlayerDetect") )
        {
            detected = true;
            target = col.GetComponent<Transform>();
            if (col.transform.position.x > transform.position.x && !invoked)
            {
                Invoke("instantiateEnemy", 1.5f);
                invoked = true;
            }
            else
             if (col.transform.position.x < transform.position.x && !invoked)
            {
                Invoke("instantiateEnemy2", 1.5f);
                invoked = true;
            }
            if (!IsInvoking()) func();
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("PlayerDetect")) {
            CancelInvoke("instantiateEnemy");
            CancelInvoke("instantiateEnemy2");
            detected = false;
        }
    }
    void instantiateEnemy()
    {
        GameObject avispa = Instantiate(enemyPrefab, transform.position, Quaternion.identity, transform) as GameObject;
        avispa.transform.localScale = new Vector2(1f, transform.localScale.y);
        moveTowards = true;
    }
   
    void instantiateEnemy2()
    {
        GameObject avispa = Instantiate(enemyPrefab, transform.position, Quaternion.identity, transform) as GameObject;
        avispa.transform.localScale=new Vector2(-1f,transform.localScale.y);
        moveTowards = true;
    }

}
