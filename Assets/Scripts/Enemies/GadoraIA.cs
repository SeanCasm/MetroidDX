using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
using Enemy;
public class GadoraIA : EnemyBase
{
    [SerializeField]int damageCount;
    [SerializeField]BoxCollider2D hurtBox,playerDetector;
    [SerializeField]GameObject gadoraBlast,deadSound,deadPrefab;
    [SerializeField]Transform shootPoint;
    private Vector2 blastDirection;
    private bool playerDetected,damaged,blink;
    new void Awake()
    {
        base.Awake();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.IsTouching(playerDetector))
        {
            playerDetected = true;

        }else if (collision.IsTouching(hurtBox))
        {
            switch (collision.tag)
            {
                case "Missile":
                    damageCount--; damaged = true;
                    break;
                case "SuperMissile":
                    damageCount -= 3; damaged = true;
                    break;
                case "Player":
                    blink = true;
                    break;
            }
            if (damageCount <= 0)
            {
                Instantiate(deadSound);
                Instantiate(deadPrefab,transform.position,Quaternion.identity,null);
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.IsTouching(playerDetector) && collision.tag=="Player")
        {
            playerDetected = false;
        }
    }
  
    /// <summary>
    /// Set damaged animator state to false in the animation
    /// </summary>
    public void Damaged()
    {
        damaged = false;
    }
    /// <summary>
    /// Set blink animator state to false in the animation
    /// </summary>
    public void DontBlink()
    {
        blink = false;
    }
    /// <summary>
    /// Set a probability of go back to blink animation state
    /// </summary>
    public void BlinkAgain()
    {
        int i = Random.Range(1, 3);
        if (i == 1) blink = true;
        else blink = false;
    }
    public void Attack()
    {
        GameObject blast = Instantiate(gadoraBlast, shootPoint.position,transform.rotation);
        Weapon blastBullet = blast.GetComponent<Weapon>();
        blastBullet.Direction = transform.right;
    }
    void LateUpdate()
    {
        anim.SetBool("Blink", damaged || blink);
    }
}
