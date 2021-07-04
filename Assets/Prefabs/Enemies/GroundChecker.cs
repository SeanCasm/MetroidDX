using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Enemy{
    public class GroundChecker : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float wallAware, floorAware;
        [SerializeField] CapsuleCollider2D capsule;
        [SerializeField, Range(.01f, .2f)] float slopeFrontRay = 0.08f;
        [SerializeField, Range(.01f, .2f)] float slopeBackRay = 0.08f;
        [SerializeField, Range(.01f, 1f)] float groundHitSlope;
        [Tooltip("Add a offset to transform.position.x to detect slopes on up or down before start climb/down")]
        [SerializeField] LayerMask groundLayer;
    
        private Vector2 posFrontRay, posBackRay, slopePerp;
        
        private RaycastHit2D frontHit, backHit;
        private bool facingRight,onSlope;
        private float spriteWitdh,slopeAngle,frontAngle,backAngle;
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

            if (Physics2D.Raycast(transform.position, transform.right, wallAware, groundLayer))
            {
                Flip();
            }
            OnSlope();
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
            transform.eulerAngles = new Vector2(0, yRotation);
        }
        private void OnSlope()
        {
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.down, groundHitSlope, groundLayer);
            Debug.DrawRay(transform.position, Vector2.down * groundHitSlope, Color.green);
            if (hit2D)
            {
                slopeAngle = Vector2.Angle(hit2D.normal, Vector2.up);
                slopePerp = Vector2.Perpendicular(hit2D.normal).normalized;
                if ((slopePerp.y < 0 && dir < 0) || (slopePerp.y > 0 && dir > 0)) slopeAngle *= -1;
                if (slopeAngle != 0) onSlope = true;
                else onSlope = false;
            }
            posFrontRay = new Vector2(transform.position.x + ((capsule.size.x/2)* dir), capsule.bounds.min.y+ .025f);
            posBackRay = new Vector2(transform.position.x - ((capsule.size.x/2) * dir), capsule.bounds.min.y+ .025f);
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
        public void SetOnGroundVelocity(float amount)
        {
            if (!onSlope) rigid.SetVelocity(amount, rigid.velocity.y);
            else
            {
                rigid.SetVelocity(-amount * slopePerp.x, -amount * slopePerp.y);
            }
            /*if ((frontAngle == 0 && backAngle != frontAngle) && (frontHit.point.y > backHit.point.y) && backAngle != 0)
            {
                rigid.SetVelocity(rigid.velocity.x, 0f);
            }*/
        }
    }
}
