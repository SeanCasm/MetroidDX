using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapon;
namespace Enemy.Weapons
{
    public class Weapon : WeaponBase<int>
    {
        [SerializeField]float speed;
        [SerializeField] protected Vector3 direction;
        [SerializeField] WeaponType weaponType;
        [SerializeField] float timeTillHit;
        [SerializeField]Rigidbody2D rigid;
        Transform player;
        protected Vector3 target;
        public float Damage { get { return damage; } }
        public Vector3 Direction{get{return direction;}set{direction=value;}}
        public enum WeaponType
        {
            Standard,LookPlayerFirst,PlayerDirection,ShooterDirection,Parabolic
        }
        new void Awake()
        {
            base.Awake();
        }
        protected void Start()
        {
            switch (weaponType)
            {
                case WeaponType.LookPlayerFirst:
                    player = GameObject.FindGameObjectWithTag("Player").transform.parent;
                    SetDirectionAndRotation(transform);
                    break;
                case WeaponType.PlayerDirection:
                    player = GameObject.FindGameObjectWithTag("Player").transform.parent;
                    SetDirection(transform);
                    break;
                case WeaponType.ShooterDirection:
                    direction=transform.right;
                    break;
            }
            Destroy(gameObject, livingTime);
        }
        protected void OnBecameInvisible()
        {
            Destroy(gameObject);
        }
        protected void OnTriggerEnter2D(Collider2D collision)
        {
            switch(collision.tag){
                case "Player":
                    GameEvents.damagePlayer.Invoke(damage, transform.position.x);
                    Destroy(gameObject);
                break;
                case "Suelo":Destroy(gameObject);
                break;
            }
        }
        protected void FixedUpdate()
        {
            rigid.MovePosition(transform.position + direction * Time.deltaTime*speed);
        }
        protected void SetDirection(Transform myTransform)
        {
            if (player != null)
            {
                target = player.position;
                direction = (target - myTransform.position).normalized;
            }
        }
        protected void SetDirectionAndRotation(Transform myTransform)
        {
            SetDirection(myTransform);
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
        /// <summary>
        /// Make a parabolic trajectory through the bullet throw point and target
        /// </summary>
        /// <param name="throwPoint">transform of throw point</param>
        /// <param name="target">transform of the target point</param>
        public void Throw(Transform throwPoint, Transform target)
        {
            float xdistance;
            xdistance = target.position.x - throwPoint.position.x;
            float ydistance;
            ydistance = target.position.y - throwPoint.position.y;
            float throwAngle;
            throwAngle = Mathf.Atan((ydistance + 4.905f * (timeTillHit * timeTillHit)) / xdistance);
            float totalVelo = xdistance / (Mathf.Cos(throwAngle) * timeTillHit);
            float xVelo, yVelo;
            xVelo = totalVelo * Mathf.Cos(throwAngle);
            yVelo = totalVelo * Mathf.Sin(throwAngle);
            rigid.velocity = new Vector2(xVelo, yVelo);
        }
        protected void DoDrop()
        {
            GameEvents.drop.Invoke(transform.position);
            Destroy(gameObject);
        }
    }
}