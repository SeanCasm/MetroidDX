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
        private float rotationZ;
        public Vector3 Direction{get{return direction;}set{direction=value;}}

        bool IPooleable.pooleable { get => this.pooleable; set => this.pooleable=value; }

       
        protected new void OnEnable() {
            base.OnEnable();
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
                transform.SetParent(parent);
                gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// Looks to the player.
        /// </summary>
        public void SetDirectionAndRotation()
        {
            SetDirection();
            rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);        
        }
        /// <summary>
        /// Looks toward the player, limitating the rotation and direction.
        /// </summary>
        /// <param name="zLimit">z right angle limit</param>
        /// <param name="zLimitLeft">< left angle limit</param>
        public void SetDirectionAndRotationLimit(float zLimit,float zLimitLeft)
        {
            SetDirection();
            rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            if(rotationZ<=90 && rotationZ>=-90)rotationZ=Mathf.Clamp(rotationZ,-zLimit,zLimit);
            else if(rotationZ>90 && rotationZ>-90) rotationZ=Mathf.Clamp(rotationZ,-zLimitLeft,zLimitLeft);

            transform.rotation = Quaternion.Euler(0, 0, rotationZ);

            direction = transform.right;
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
        /// Sets the weapon direction towards the player.
        /// </summary>
        /// <param name="myTransform"></param>
        protected void SetDirection()
        {
            if (player != null)
            {
                direction = (player.position - transform.position).normalized;
            }
        }
        private Vector2 Vector2FromAngle(float a)
        {
            a *= Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(a), Mathf.Sin(a));
        }
        protected void DoDrop()
        {
            GameEvents.drop.Invoke(transform.position);
            BackToShootPoint();
        }
    }
}