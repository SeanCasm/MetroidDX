using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    public bool facingRight{get;set;}
    public Vector2 direction{get;private set;}
    public bool doJump{get;set;}
    public System.Action OnJump,OutJump;
    public void Flip()
    {
        facingRight = !facingRight;
        direction=direction*-Vector2.right;
        float localScaleX = transform.localScale.x;
        localScaleX = localScaleX * -1f;
        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    public void Jump()
    {
        doJump = !doJump;
        if(doJump)OnJump?.Invoke();
        else OutJump?.Invoke();
    }
}
