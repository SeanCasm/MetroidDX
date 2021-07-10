using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
using Enemy;
using System;

public class PlayerKnockBack : MonoBehaviour
{
    #region Properties
    [SerializeField] BoxCollider2D box;
    [SerializeField] PlayerHealth health;
    [SerializeField] PlayerController player;
    [SerializeField] Animator animator;
    [SerializeField] SomePlayerFX playerFX;
    [SerializeField] float knockBackTime;
    private float currentTime;
    private EnemyHealth enemy;
    private Vector2 knockBackDir;
    public int damageReceived { get; set; }
    #endregion
    #region Unity Methods
    private void OnEnable()
    {
        box.enabled=true;
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
    void EnableCollider()
    {
        box.enabled = true;
    }
    /// <summary>
    /// Set the direction of the player knock back depending of player position in X axis and
    /// the collision position in X axis, at any animation state except balled.
    /// </summary>
    /// <param name="myXPosition">player X position</param>
    /// <param name="collisionX">collision X position</param>
    private void Hitted(float myXPosition, float collisionX)
    {
        if(!player.Balled)animator.SetTrigger("Hitted");
        if (collisionX >= myXPosition) { player.leftLook = false; player.OnLeft(false); knockBackDir=new Vector2(-.55f,.55f);}
        else { player.leftLook = true; knockBackDir=new Vector2(.55f,.55f);player.OnLeft(true); }
        CancelInvoke();
        StartCoroutine(KnockBack());
        box.enabled = false; 
        Invoke("EnableCollider", knockBackTime);
        health.AddDamage(damageReceived);
        Invoke("EnableMovement",knockBackTime);
    }
    void EnableMovement()
    {
        player.damaged =  false;
        player.movement = PlayerController.canInstantiate = true;
        player.rb.gravityScale = 1;
        StopCoroutine(KnockBack());
    }
    IEnumerator KnockBack()
    {
        while (player.damaged)
        {
            player.rb.velocity=knockBackDir;
            currentTime+=.015f;
            if(currentTime>=knockBackTime/2)knockBackDir = new Vector2(knockBackDir.x, -1.1f);
            yield return new WaitForSeconds(.015f);
        }
    }
    #endregion
}