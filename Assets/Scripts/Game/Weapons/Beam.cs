using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Weapon;
namespace Player.Weapon{
    public class Beam : Weapon,IRejectable
    {
        [SerializeField]GameObject reject,impactClip;
        [SerializeField] protected Vector3 direction;
        [SerializeField] protected float speed;
        [Tooltip("When beam collides generates this impact.")]
        [SerializeField] protected GameObject impactPrefab;
        [SerializeField] protected BoxCollider2D floorCol;
        private BoxCollider2D damageCol;
        public Vector3 Direction { get { return direction; } set { direction = value; } }


        #region Unity methods
        new private void Awake() {
            base.Awake();
        }
        // Start is called before the first frame update
        protected void Start()
        {
            damageCol=GetComponent<BoxCollider2D>();
            rigid = GetComponent<Rigidbody2D>();
            Destroy(gameObject, livingTime);
            GameEvents.overHeatAction.Invoke(hotPoints);
        }
        protected void FixedUpdate()
        {
            if(!rejected)rigid.velocity = direction.normalized * speed;
        }
        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy") && collision.IsTouching(damageCol))
            {
                IDamageable<float> health = collision.GetComponent<IDamageable<float>>();
                IInvulnerable iInvulnerable=collision.GetComponent<IInvulnerable>();
                if(health==null && iInvulnerable!=null){
                    //in this case, beams collides with a body shield, and inmediatly rejected
                    Reject();
                }
                if (health!=null&& iInvulnerable!= null)
                {
                    TryDoDamage(damage, health, beamType,iInvulnerable);
                    if (!rejected){ 
                        Instantiate(impactPrefab, transform.position, Quaternion.identity,null);
                        if(beamType !=WeaponType.Plasma){
                            Destroy(gameObject);
                        }
                    }
                    else Reject();
                }
            }else if ((collision.IsTouching(floorCol) && collision.tag=="Suelo"))FloorCollision();
            else if(collision.CompareTag("EnemyBeam")){
                IDrop iDrop = collision.GetComponent<IDrop>();
                if(iDrop!=null)FloorCollision();
            }
        }
        protected void FloorCollision(){
            if(impactClip)Instantiate(impactClip);
            Instantiate(impactPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        private void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
        #endregion
        public void Reject()
        {
            if(beamType == WeaponType.Missile || beamType == WeaponType.SuperMissile){
                Instantiate(reject);
                rigid.bodyType = RigidbodyType2D.Dynamic;
                rigid.constraints = RigidbodyConstraints2D.None;
                rigid.velocity = Vector2.zero;
                rigid.gravityScale = 1f;
                GetComponent<BoxCollider2D>().isTrigger = false;
            }else{
                Destroy(gameObject);
            }
        }
    }
}
 