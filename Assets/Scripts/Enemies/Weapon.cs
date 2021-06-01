using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy.Weapons
{
    public class Weapon : WeaponBase<int>,IPooleable
    {
        [SerializeField]float speed;
        [SerializeField]protected Rigidbody2D rigid;
        [SerializeField]bool pooleable;
        public Transform player{get;set;}
        public Transform parent{get;set;}
        private Vector3 target;

        public float Damage { get { return damage; } }
        public Vector3 Direction{get{return direction;}set{direction=value;}}
      
        new void Awake()
        {
            base.Awake();
        }
        protected void OnEnable() {
            Invoke("BackToShootPoint", livingTime);
        }
        protected void OnBecameInvisible()
        {
            BackToShootPoint();
        }
        protected void BackToShootPoint(){
            if(!pooleable){
                Destroy(gameObject);
            }else{
                transform.position = parent.position;
                gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// Looks to the player.
        /// </summary>
        public void SetDirectionAndRotation()
        {
            SetDirection();
            Vector2 direction = player.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 22);
        }
        public void SetDirectionAndRotation(float zDegrees)
        {
            switch (zDegrees)
            {
                case 0:
                    direction = Vector2.right;
                    break;
                case 45:
                    transform.Rotate(new Vector3(0f, 0f, 45f));
                    direction = Vector2.right + Vector2.up;
                    break;
                case 90:
                    transform.Rotate(new Vector3(0f, 0f, 90f));
                    direction = Vector2.up;
                    break;
                case 135:
                    transform.Rotate(new Vector3(0f, 0f, 135f));
                    direction = Vector2.left + Vector2.up;
                    break;
                case 180:
                    transform.Rotate(new Vector3(0, 0, 180));
                    direction = Vector2.left;
                    break;
                case 225:
                    transform.Rotate(new Vector3(0f, 0f, 225));
                    direction = Vector2.down + Vector2.left;
                    break;
                case 270:
                    transform.Rotate(new Vector3(0f, 0f, 2700f));
                    direction = Vector2.down;
                    break;
                case 315:
                    transform.Rotate(new Vector3(0f, 0f, 315));
                    direction = Vector2.down + Vector2.right;
                    break;

            }
        }
        protected void OnTriggerEnter2D(Collider2D collision)
        {
            switch(collision.tag){
                case "Player":
                    GameEvents.damagePlayer.Invoke(damage, transform.position.x);
                    BackToShootPoint();
                break;
                case "Suelo":BackToShootPoint();
                break;
            }
        }
        protected void FixedUpdate()
        {
            rigid.MovePosition(transform.position + direction * Time.deltaTime*speed);
        }
        /// <summary>
        /// Sets the weapon direction toward the player.
        /// </summary>
        /// <param name="myTransform"></param>
        protected void SetDirection()
        {
            if (player != null)
            {
                target = player.position;
                direction = (target - transform.position).normalized;
            }
        }
         
        protected void DoDrop()
        {
            GameEvents.drop.Invoke(transform.position);
            BackToShootPoint();
        }
    }
}