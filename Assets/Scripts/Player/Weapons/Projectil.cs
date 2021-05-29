using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player.Weapon
{
    public class Projectil : Weapon, IRejectable, IPooleable
    {
        [SerializeField] protected GameObject reject, impactClip;
        [SerializeField] protected float speed;
        [Tooltip("When beam collides generates this impact.")]
        [SerializeField] protected GameObject impactPrefab;
        [SerializeField] protected BoxCollider2D floorCol;
        [SerializeField] protected Rigidbody2D rigid;
        [SerializeField] BoxCollider2D damageCol;
        [SerializeField] bool pooleable;
        [SerializeField] bool isSpazer,spazerChild;
        protected IDamageable<float> health;
        public bool Pooleable => pooleable;
        protected IInvulnerable iInvulnerable;
        public Vector3 Direction { get { return direction; } set { direction = value; } }
        public bool IsSpazer=>isSpazer;

        public Transform parent { get; set; }

        public System.Action NoPlasmaOnTrigger;
        public System.Action OnParentDisabled,OnChildCollided;

        #region Unity methods
        new private void Awake()
        {
            base.Awake();
        }
        protected void OnEnable()
        {
            Invoke("BackToGun", livingTime);
            GameEvents.overHeatAction.Invoke(hotPoints);
            direction = transform.parent.right;

        }   
        protected void FixedUpdate()
        {
            if (!rejected) rigid.velocity = direction.normalized * speed;
        }
        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy") && collision.IsTouching(damageCol))
            {
                health = collision.GetComponent<IDamageable<float>>();
                iInvulnerable = collision.GetComponent<IInvulnerable>();
                if (health == null && iInvulnerable != null)
                {
                    //in this case, beams collides with a body shield, and inmediatly rejected
                    Reject();
                }
                if (health != null && iInvulnerable != null)
                {
                    TryDoDamage(damage, health, beamType, iInvulnerable);
                    if (!rejected)
                    {
                        Instantiate(impactPrefab, transform.position, Quaternion.identity, null);
                        NoPlasmaOnTrigger?.Invoke();
                    }
                    else Reject();
                }
            }
            else if ((collision.IsTouching(floorCol) && collision.tag == "Suelo")) FloorCollision();
            else if (collision.CompareTag("EnemyBeam"))
            {
                IDrop iDrop = collision.GetComponent<IDrop>();
                if (iDrop != null) FloorCollision();
            }
        }
        protected void BackToGun()
        {
            if (!pooleable) Destroy(gameObject);
            if (!spazerChild)
            {
                transform.SetParent(parent);
                transform.position = parent.position;
                if(isSpazer)OnParentDisabled?.Invoke();
                gameObject.SetActive(false);
            }else{
                OnChildCollided?.Invoke();
            }
        }
        protected void FloorCollision()
        {
            if (impactClip) Instantiate(impactClip);
            Instantiate(impactPrefab, transform.position, Quaternion.identity);
            BackToGun();
        }
        protected void OnBecameInvisible()
        {
            BackToGun();
        }
        #endregion
        public void Reject()
        {
            Instantiate(reject);
            BackToGun();
        }
    }
}
