using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWalking : MonoBehaviour
{
    #region Properties
    [SerializeField] int direction;
    [SerializeField] float speed,floorAware,wallAware;
    [Tooltip("Adds a delay after change the eulerAngles on slopes")]
    [SerializeField] float checkDelay;
    [SerializeField] LayerMask groundLayer;
    RaycastHit2D wallHit;
    private Transform floorCorner;
    private Vector2 velocity,slopePerp;
    private float wallAngle,slopeAngle,prevAngle,curAngle;
    private bool wallInFront,onSlope,checkFloor=true;
    private Rigidbody2D _rigidbody;
    #endregion
    #region Unity Methods
    private void Start() {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    void Awake()
    {
        floorCorner = transform.GetChild(0);
    }
    void Update()
    {
        CheckAlign();
        CheckWall();
        if (wallInFront)
        {
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z + 90f);
            wallInFront = false;
        }
        if (!Physics2D.Raycast(transform.position, -transform.up, floorAware, groundLayer))
        {
            transform.position = floorCorner.position;
            transform.eulerAngles = new Vector3(0, 0, transform.eulerAngles.z - 90f);
        }
    }
    private void FixedUpdate()
    {
        velocity = speed * Time.deltaTime*transform.right;
        _rigidbody.velocity = velocity;
    }
    #endregion
    #region Private Methods 
    private void CheckAlign(){
        RaycastHit2D align=Physics2D.Raycast(transform.position,-transform.up,floorAware,groundLayer);
        if(align){
            slopeAngle = Vector2.Angle(align.normal, Vector2.up);
            slopePerp = Vector2.Perpendicular(align.normal).normalized;
            if ((slopePerp.y < 0 && direction < 0) || (slopePerp.y > 0 && direction > 0)) slopeAngle *= -1;
            if(slopeAngle!=0 && slopeAngle!=90 && slopeAngle!=-90 && slopeAngle!=180)onSlope=true;
            else onSlope=false;
            prevAngle=curAngle;
            curAngle=slopeAngle;
            
            if(prevAngle!=curAngle)checkFloor=false;
            if(checkFloor)transform.eulerAngles = new Vector3(0, 0, slopeAngle);
            else if(!IsInvoking("CheckFloor"))Invoke("CheckFloor",checkDelay);
        }
        Debug.DrawRay(transform.position, -transform.up * floorAware, Color.red);

    }
    void CheckFloor(){
        checkFloor=true;
    }
    private void CheckWall()
    {
        wallHit = Physics2D.Raycast(transform.position, transform.right, wallAware, groundLayer);
        Debug.DrawRay(transform.position, transform.right * wallAware, Color.green);
        if (wallHit)
        {
            wallAngle = Vector2.Angle(wallHit.normal, Vector2.up);
            if (CheckWallAngle(89,91) || CheckWallAngle(-1,1) || CheckWallAngle(179, 181)) wallInFront = true;
        }
        else wallAngle = 0;
    }
    private bool CheckWallAngle(float value1,float value2){
        if(wallAngle>=value1 && wallAngle<=value2)return true;
        else return false;
    }
    #endregion
}
