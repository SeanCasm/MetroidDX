using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Weapon;
namespace Player.Weapon{
public class Bomb : Weapon
    {
        [SerializeField]AudioClip bombExplosion;
        private AudioSource audioPlayer;
        private EnemyHealth eHealth;
        #region Unity methods
        void Start()
        {
            if (startSound != null) Instantiate(startSound);
            //damageType = new HitDamage();
            GameEvents.overHeatAction.Invoke(hotPoints);
            audioPlayer=GetComponent<AudioSource>();
        }
        void OnTriggerEnter2D(Collider2D col)
        {
            IDamageable<float> health = col.GetComponent<IDamageable<float>>();
            IInvulnerable iInvulnerable = col.GetComponent<IInvulnerable>();
            if(health!=null && iInvulnerable!=null){
                TryDoDamage(damage, health, beamType,iInvulnerable);

            }
        }
        void OnTriggerStay2D(Collider2D col)
        {
            IDamageable<float> health = col.GetComponent<IDamageable<float>>();
            IInvulnerable iInvulnerable = col.GetComponent<IInvulnerable>();
            if (health!=null && iInvulnerable!=null && beamType == WeaponType.SuperBomb)
            {
                TryDoDamage(damage, eHealth, beamType,iInvulnerable);
            }
        }
        #endregion
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
 
