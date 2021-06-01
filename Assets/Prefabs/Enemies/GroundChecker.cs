using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Enemy{
    public class GroundChecker : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float wallAware, floorAware,slopeAware;
        [Tooltip("Add a offset to transform.position.x to detect slopes on up or down before start climb/down")]
        [SerializeField] LayerMask groundLayer;
        [Tooltip("Checks if front slope is downing or upping")]
        [SerializeField] Transform frontGroundPoint,backGroundPoint;
        private bool facingRight;
        private float spriteWitdh;
        public bool FacingRight { get { return facingRight; } }
        bool onGround, slopeUp,slopeDown;
        public bool OnGround { get { return onGround; } }
        public float FloorAware=>floorAware;
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
            if (transform.eulerAngles.y == 0)facingRight = true;
            else facingRight = false;
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
            SlopeChecker();
        }
        RaycastHit2D frontHit,backHit;
        float backSlopeAngle,frontSlopeAngle;
        private void SlopeChecker()
        {
            frontHit = Physics2D.Raycast(frontGroundPoint.position, -transform.up, slopeAware, groundLayer);
            frontSlopeAngle=Vector2.Angle(frontHit.normal,Vector2.up);
            backHit = Physics2D.Raycast(backGroundPoint.position, -transform.up, slopeAware, groundLayer);
            backSlopeAngle = Vector2.Angle(backHit.normal, Vector2.up);
            if (frontSlopeAngle!=0) { slopeDown = false; slopeUp = true; }
            else
            {
                slopeUp = false;
                if (backSlopeAngle != 0)slopeDown = true;
                else slopeDown=false;
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
            }
            else
            {
                transform.position = new Vector3(transform.position.x - spriteWitdh / 2, transform.position.y, 0);
                yRotation = 180;
            }
            transform.eulerAngles = new Vector2(0, yRotation);
        }
        public void SetOnGroundVelocity(float amount)
        {
            if (!slopeDown && !slopeUp)
            {
                if (facingRight){dir=1; rigid.SetVelocity(amount, rigid.velocity.y);}
                else {rigid.SetVelocity(-amount, rigid.velocity.y);dir=-1;}
            }
            else
            {
                if (slopeUp)
                {
                    if (facingRight) rigid.SetVelocity(amount, amount);
                    else rigid.SetVelocity(-amount, amount); 
                }
                 
                else
                { 
                    if (facingRight) rigid.SetVelocity(amount, -amount);
                    else rigid.SetVelocity(-amount, -amount);
                }
            }
        }
    }
}
