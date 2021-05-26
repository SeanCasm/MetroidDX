using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player{
    public class GroundChecker : MonoBehaviour
    {
        private PlayerController player;
        private Rigidbody2D rb2d;
        private SomePlayerFX playerFX;
        private EnemyHealth enemyHealth;
        void Awake()
        {
            player = GetComponentInParent<PlayerController>();
            rb2d = GetComponentInParent<Rigidbody2D>();
            playerFX=GetComponentInParent<SomePlayerFX>();
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            switch (col.tag)
            {
                case "Plattform":
                    rb2d.velocity = new Vector3(0f, 0f, 0f);
                    player.transform.parent = col.transform;
                    player.IsGrounded = true;
                    break;
                case "Suelo":
                    player.IsGrounded = true;
                    player.FalseAnyAnimStateAtGrounding();
                    playerFX.StopLoopClips();
                break;
            }
        }
        void OnTriggerStay2D(Collider2D col)
        {
            switch (col.tag)
            {
                case "Enemy":
                    enemyHealth = col.GetComponent<EnemyHealth>();
                    if (enemyHealth.freezed && enemyHealth != null) player.IsGrounded = true;
                    break;
                case "Suelo":
                    player.IsGrounded = true;
                    break;
            }
        }
        void OnTriggerExit2D(Collider2D col)
        {
            switch (col.tag)
            {
                case "Suelo":
                    player.IsGrounded = false;
                    player.FalseAnyAnimStateAtAir();
                break;
                case "Enemy":
                    enemyHealth = col.GetComponent<EnemyHealth>();
                    if (enemyHealth.freezed && enemyHealth != null){
                        player.FalseAnyAnimStateAtAir();
                        player.IsGrounded = false;
                    }
                break;
                case "Platform":
                    player.transform.parent = null; player.IsGrounded = false;
                break;
            }
        }
    }
}