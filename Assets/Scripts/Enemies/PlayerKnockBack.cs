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
            if (player.inSBVelo || player.Screwing || player.HyperJumping)
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
        if (!player.inSBVelo && !player.Screwing && !player.HyperJumping && !health.invulnerability)
        {
            player.Damaged = true;
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
        if (collisionX >= myXPosition) { animator.SetTrigger("Hitted"); player.leftLook = false; knockBackDir=new Vector2(-1.1f,1.1f);}
        else { animator.SetTrigger("HittedLeft"); player.leftLook = true; knockBackDir=new Vector2(1.1f,1.1f);}
        StartCoroutine(KnockBack());
        box.enabled = false; Invoke("EnableCollider", 0.95f);
        health.AddDamage(damageReceived);
        Invoke("EnableMovement", 0.25f);
    }
    void EnableMovement()
    {
        player.Damaged =  false;
        player.movement = PlayerController.canInstantiate = true;
        player.rb.gravityScale = 1;
    }
    IEnumerator KnockBack()
    {
        player.rb.gravityScale=0;
        while (!player.movement)
        {
            player.rb.velocity=knockBackDir;
            yield return null;
        }
    }
    #endregion
}