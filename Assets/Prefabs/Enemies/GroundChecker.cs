using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Enemy{
    public class GroundChecker : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float wallAware, floorAware;
        [SerializeField] CircleCollider2D coll;
        [SerializeField, Range(.01f, .2f)] float slopeFrontRay = 0.08f;
        [SerializeField, Range(.01f, .2f)] float slopeBackRay = 0.08f;
        [SerializeField, Range(.01f, 1f)] float groundHitSlope;
        [SerializeField] float rayOffset;
        [Tooltip("Add a offset to transform.position.x to detect slopes on up or down before start climb/down")]
        [SerializeField] LayerMask groundLayer;
    
        private Vector2 posFrontRay, posBackRay, slopePerp;
        
        private RaycastHit2D frontHit, backHit,wallHit;
        private bool facingRight,onSlope,wallInFront;
        private float spriteWitdh,slopeAngle,frontAngle,backAngle,xVelocity,wallAngle;
        public bool FacingRight { get { return facingRight; } }
        bool onGround;
        public bool OnGround { get { return onGround; } }
        public LayerMask GroundLayer=>groundLayer;
        public float dir{get;private set;}
        private Rigidbody2D rigid;

        private void Awake()
        {
            spriteWitdh = GetComponent<SpriteRenderer>().bounds.extents.x;
            rigid = GetComponent<Rigidbody2D>();
        } 
        private void Start()
        {
            if (transform.eulerAngles.y == 0){dir=1;facingRight = true;}
            else {facingRight = false;dir=-1;}
        }
        void Update()
        {
            if (!Physics2D.Raycast(transform.position, -transform.up, floorAware, groundLayer))
            {
                onGround = false;
                Flip();
            }
            else onGround = true;
            CheckWall();
            OnSlope();
            if (wallInFront)Flip();
        }
        private void CheckWall(){
            wallHit=Physics2D.Raycast(transform.position, transform.right, wallAware, groundLayer);
            Debug.DrawRay(transform.position, transform.right * wallAware, Color.red);
            if(wallHit){
                wallAngle=Vector2.Angle(wallHit.normal,Vector2.up);
                wallInFront= wallAngle>=90 ? true :false;
            }
        }
        public void Flip()
        {
            float yRotation;
            facingRight = !facingRight;
            if (facingRight)
            {
                transform.position = new Vector3(transform.position.x + spriteWitdh / 2, transform.position.y, 0);
                yRotation = 0;
                dir=1;
            }
            else
            {
                transform.position = new Vector3(transform.position.x - spriteWitdh / 2, transform.position.y, 0);
                dir=-1;
                yRotation = 180;
            }
            wallInFront=false;
            transform.eulerAngles = new Vector2(0, yRotation);
        }
        private void OnSlope()
        {
            posFrontRay = new Vector2(transform.position.x + ((coll.radius / rayOffset) * dir), coll.bounds.min.y + .025f);
            RaycastHit2D hit2D = Physics2D.Raycast(posFrontRay, Vector2.down, slopeFrontRay, groundLayer);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundHitSlope, groundLayer);

            if (hit2D && hit)
            {
                frontAngle = Vector2.Angle(hit2D.normal, Vector2.up);
                slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                slopePerp = Vector2.Perpendicular(hit2D.normal).normalized;
                if ((slopePerp.y < 0 && dir < 0) || (slopePerp.y > 0 && dir > 0)) frontAngle *= -1;
                if ((frontAngle > 0) || (frontAngle == 0 && slopeAngle != 0) || slopeAngle != 0) onSlope = true;
                else onSlope = false;
                Debug.DrawRay(transform.position,Vector2.down*groundHitSlope,Color.red);
            }

            posBackRay = new Vector2(transform.position.x - ((coll.radius / rayOffset) * dir), coll.bounds.min.y + .025f);
            frontHit = Physics2D.Raycast(posFrontRay, Vector2.down, slopeFrontRay, groundLayer);
            backHit = Physics2D.Raycast(posBackRay, Vector2.down, slopeBackRay, groundLayer);
            if (frontHit && backHit)
            {
                frontAngle = Vector2.Angle(frontHit.normal, Vector2.up);
                backAngle = Vector2.Angle(backHit.normal, Vector2.up);
            }
            Debug.DrawRay(posFrontRay, Vector2.down * slopeFrontRay, Color.yellow);
            Debug.DrawRay(posBackRay, Vector2.down * slopeBackRay, Color.cyan);
        }        
        public void SetOnGroundVelocity(float speed)
        {
            xVelocity = dir * speed * Time.deltaTime;
            rigid.velocity = (!onSlope) ? new Vector2(xVelocity, 0f) : new Vector2(-xVelocity * slopePerp.x, -xVelocity * slopePerp.y);
            if ((frontAngle == 0 && backAngle != frontAngle) && (frontHit.point.y > backHit.point.y) && backAngle != 0)
            {
                rigid.SetVelocity(xVelocity, 0f);
            }
        }
    }
}
