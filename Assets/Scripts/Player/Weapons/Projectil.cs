using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player.Weapon
{
    public class Projectil : Weapon, IRejectable, IPooleable,IPlayerWeapon
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
        private bool poolRemoved;
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
        private void OnDisable() {
            Pool.OnPoolChanged -= PoolChanged;
        }
        protected void OnEnable()
        {
            Invoke("BackToGun", livingTime);
            GameEvents.overHeatAction.Invoke(hotPoints);
            direction = parent.right;
            transform.eulerAngles=parent.eulerAngles;
            Pool.OnPoolChanged += PoolChanged;

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
                if (health == null && iInvulnerable != null)Reject();
                if (health != null && iInvulnerable != null)
                {
                    TryDoDamage(damage, health, beamType, iInvulnerable);
                    Instantiate(impactPrefab, transform.position, Quaternion.identity, null);
                    if (!rejected)
                    {
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
        private void PoolChanged(){
            poolRemoved=true;
        }
        protected void BackToGun()
        {
            if(poolRemoved)Destroy(gameObject);
            if (!pooleable){
                Destroy(gameObject);
            }else if (!spazerChild)
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
            Instantiate(impactPrefab, transform.position, Quaternion.identity,null);
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
