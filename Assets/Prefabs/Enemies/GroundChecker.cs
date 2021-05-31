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
        [SerializeField] Transform groundPoint;
        [SerializeField]Vector2 slopeDownOffset;
        [SerializeField]Vector2 slopeUpOffset;
        private bool facingRight;
        private float spriteWitdh, slopeAngle;
        public bool FacingRight { get { return facingRight; } }
        bool onSlope, onGround, slopeUp;
        public bool OnGround { get { return onGround; } }
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
        RaycastHit2D hit,slopeHit;
        private void SlopeChecker()
        {
            hit = Physics2D.Raycast(groundPoint.position, -transform.up, floorAware, groundLayer);
            if (hit)
            {
                slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != 0) onSlope = true;
                else slopeUp = onSlope = false;
                if (onSlope)
                {
                    slopeHit =Physics2D.Raycast(groundPoint.position, transform.right, slopeAware, groundLayer);
                    if (slopeHit) slopeUp = true;
                }
            }
        }
        private bool canCheck=true;
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
            if (!onSlope)
            {
                if (facingRight) rigid.SetVelocity(amount, rigid.velocity.y);
                else rigid.SetVelocity(-amount, rigid.velocity.y);
            }
            else
            {
                if (slopeUp)
                {
                    if (facingRight) rigid.SetVelocity(amount, amount);
                    else rigid.SetVelocity(-amount, amount);
                    groundPoint.localPosition=new Vector3(slopeUpOffset.x,slopeUpOffset.y,0);
                }
                 
                else
                {
                    groundPoint.localPosition=new Vector3(slopeDownOffset.x,slopeDownOffset.y,0);
                    if (facingRight) rigid.SetVelocity(amount, -amount);
                    else rigid.SetVelocity(-amount, -amount);
                }
            }
        }
    }
}
