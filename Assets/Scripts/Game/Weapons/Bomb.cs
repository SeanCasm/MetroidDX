using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Weapon;
namespace Player.Weapon
{
    public class Bomb : Weapon
    {
        [SerializeField] protected AudioClip bombExplosion;
        [SerializeField] protected float timeToExplode;
        protected Animator animator;
        protected AudioSource audioPlayer;
        protected IDamageable<float> health;
        protected IInvulnerable iInvulnerable;
        #region Unity methods
        new void Awake() {
            audioPlayer = GetComponent<AudioSource>();
            animator=GetComponent<Animator>();
            Invoke("Explode",timeToExplode);
        }
        protected void Start()
        {
            if (startSound != null) Instantiate(startSound);
            GameEvents.overHeatAction.Invoke(hotPoints);
        }
        protected void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Enemy"))
            {
                health = col.GetComponent<IDamageable<float>>();
                iInvulnerable = col.GetComponent<IInvulnerable>();
                if (health != null && iInvulnerable != null)
                {
                    TryDoDamage(damage, health, beamType, iInvulnerable);
                }
            }
        }
        #endregion
        private void Explode(){
            animator.SetTrigger("Explode");
        }
        #region Public methods
        public void PlayExplosion()
        {
            audioPlayer.clip = bombExplosion;
            audioPlayer.Play();
        }
        public void Destroy()
        {
            Destroy(gameObject);
        }

        #endregion 
    }
}

