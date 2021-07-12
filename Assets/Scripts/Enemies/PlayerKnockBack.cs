using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
using Enemy;
using System;

public class PlayerKnockBack : MonoBehaviour
{
    #region Properties
    [SerializeField] PlayerHealth health;
    [SerializeField] PlayerController player;
    [SerializeField] Animator animator;
    [SerializeField] SomePlayerFX playerFX;
    [SerializeField] float knockBackTime;
    [SerializeField] float knockBackPowUp,knockBackPowHor;
    private float currentTime,dir;
    private EnemyHealth enemy;
    public int damageReceived { get; set; }
    #endregion
    #region Unity Methods
    private void OnEnable()
    {
        GameEvents.damagePlayer += HandleHit;
    }
    private void OnDisable()
    {
        GameEvents.damagePlayer -= HandleHit;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            enemy = col.GetComponent<EnemyHealth>();
            if (player.inSBVelo || (player.screwSelected && player.OnRoll) || player.HyperJumping)
            {
                enemy.AddDamage(999);
            }
        }
        else
        if (col.CompareTag("Suelo") && player.HyperJumping)
        {
            player.HyperJumping = player.inSBVelo = player.hyperJumpCharged = false;
            playerFX.StopLoopClips();
            health.AddDamage(20);
            player.rb.gravityScale=1;
        }
    }
    #endregion
    #region Private Methods
    private void HandleHit(int damage, float xPosition)
    {
        if (!PlayerHealth.invulnerability)
        {
            currentTime=0;
            player.damaged= true;
            player.ResetState();
            damageReceived = damage;
            Hitted(transform.position.x, xPosition);
        }
    }
    /// <summary>
    /// Set the direction of the player knock back depending of player position in X axis and
    /// the collision position in X axis, at any animation state except balled.
    /// </summary>
    /// <param name="myXPosition">player X position</param>
    /// <param name="collisionX">collision X position</param>
    private void Hitted(float myXPosition, float collisionX)
    {
        health.AddDamage(damageReceived);
        if(!PlayerHealth.isDead){
            if (!player.Balled){ animator.SetTrigger("Hitted");}
            if (collisionX >= myXPosition) { dir=-1;player.leftLook = false; player.OnLeft(false); }
            else { player.leftLook = true; player.OnLeft(true);dir = 1; }
            StartCoroutine("KnockBack");
        }
    }
    private IEnumerator KnockBack(){
        float time=0;
        player.rb.gravityScale=0;
        while(time<knockBackTime){
            player.rb.AddForce(new Vector2(dir*knockBackPowHor,knockBackPowUp));
            time+=Time.deltaTime;
            yield return null;
        }
        player.rb.gravityScale = 1;
        player.RestoreValuesAfterHit();
    }
 
  
    #endregion
}