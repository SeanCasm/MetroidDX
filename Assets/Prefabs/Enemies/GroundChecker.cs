using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Enemy{
    public class GroundChecker : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] float wallAware, floorAware;
        [Tooltip("Add a offset to transform.position.x to detect slopes on up or down before start climb/down")]
        [SerializeField] float slopeDetectorOffset;
        [SerializeField] LayerMask groundLayer;
        [Tooltip("Checks if front slope is downing or upping")]
        [SerializeField] Transform groundPoint;
        Transform newX;
        private bool ignoreUpdates, facingRight;
        private float spriteWitdh, slopeAngle;
        public bool FacingRight { get { return facingRight; } }
        bool onSlope, onGround, slopeUp;
        public bool OnSlope { get { return onSlope; } }
        public bool OnGround { get { return onGround; } }
        public bool SlopeUp { get => slopeUp; }
        Vector2 slopePerpendicular;
        private Rigidbody2D rigid;

        public Vector2 SlopePerp { get { return slopePerpendicular; } }

        private void Awake()
        {
            spriteWitdh = GetComponent<SpriteRenderer>().bounds.extents.x;
            rigid = GetComponent<Rigidbody2D>();
            newX = transform;
        }
        private void Start()
        {
            if (transform.eulerAngles.y == 0)
            {
                facingRight = true;
                newX.position = new Vector2(transform.position.x + slopeDetectorOffset, transform.position.y);
            }
            else
            {
                facingRight = false;
                newX.position = new Vector2(transform.position.x - slopeDetectorOffset, transform.position.y);
            }
            newX.SetParent(transform);
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
        RaycastHit2D hit;
        private void SlopeChecker()
        {
            if (!onSlope) hit = Physics2D.Raycast(newX.position, -transform.up, floorAware, groundLayer);
            else hit = Physics2D.Raycast(transform.position, -transform.up, floorAware, groundLayer);
            if (hit)
            {
                slopePerpendicular = Vector2.Perpendicular(hit.normal).normalized;
                slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != 0) onSlope = true;
                else slopeUp = onSlope = false;
                if (onSlope)
                {
                    if (Physics2D.Raycast(groundPoint.position, transform.right, 0.2f, groundLayer)) slopeUp = true;
                    else slopeUp = false;
                }
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
                newX.position.Set(transform.GetX() + slopeDetectorOffset, transform.GetY(), 0);
            }
            else
            {
                transform.position = new Vector3(transform.position.x - spriteWitdh / 2, transform.position.y, 0);
                yRotation = 180;
                newX.position.Set(transform.GetX() - slopeDetectorOffset, transform.GetY(), 0);
            }
            transform.eulerAngles = new Vector2(0, yRotation);
        }
        public void SetOnGroundVelocity(float amount)
        {
            if (!onSlope)
            {
                if (facingRight) rigid.SetVelocity(amount, 0);
                else rigid.SetVelocity(-amount, 0);
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
                    if (facingRight) rigid.SetVelocity(amount, amount * -1);
                    else rigid.SetVelocity(-amount, amount * -1);
                }
            }
        }
    }
}
