using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
using Enemy;
using System;

public class PlayerKnockBack : MonoBehaviour
{
    #region Properties
    [SerializeField] BoxCollider2D box, floor;
    [SerializeField] PlayerHealth health;
    [SerializeField] PlayerController player;
    [SerializeField] Animator animator;
    private EnemyHealth enemy;
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
            if (player.speedJump || player.Screwing || player.HyperJumping)
            {
                enemy.AddDamage(999);
            }
        }
        else
        if (col.CompareTag("Suelo") && player.HyperJumping)
        {
            player.HyperJumping = player.speedJump = player.hyperJumpCharged = false;
            health.AddDamage(20);
        }
    }
    #endregion
    #region Private Methods
    private void HandleHit(int damage, float xPosition)
    {
        if (!player.speedJump && !player.Screwing && !player.HyperJumping && !health.invulnerability)
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
        if (collisionX >= myXPosition) { animator.SetTrigger("Hitted"); player.leftLook = false; }
        else { animator.SetTrigger("HittedLeft"); player.leftLook = true; }
        StartCoroutine(KnockBack());
        floor.enabled = box.enabled = false; Invoke("EnableCollider", 0.95f);
        health.AddDamage(damageReceived);
        Invoke("EnableMovement", 0.25f);
    }
    void EnableMovement()
    {
        player.Damaged = player.hittedLeft = player.hitted = false;
        floor.enabled = player.movement = player.canInstantiate = true;
    }
    IEnumerator KnockBack()
    {
        while (!player.movement)
        {
            player.rb.SetVelocity(0, 1.1f);
            yield return new WaitForSeconds(0.0001f);
        }
    }
    #endregion
}