using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    private PhysicsMaterial2D material;
    private SomePlayerFX playerFX;
    protected float jumpTime=0.35f;
    protected bool isJumping,wallJumping,onRoll;
    /*
    public bool OnRoll
    {
        set
        {
            onRoll = value;
            if (value && !screwing) playerFX.RollJump(true);
            else playerFX.RollJump(false);
        }
    }
    public bool IsJumping
    {
        get => isJumping;
        set
        {
            isJumping = value;
            if (isJumping)
            {
                floorChecker.enabled = false;
                StartCoroutine(CheckFloor());
                IsGrounded = moveOnFloor = false;
            }
            material.friction = 0;
        }
    }
    private void Awake() {
        playerFX=GetComponent<SomePlayerFX>();
        material=GetComponent<Rigidbody2D>().sharedMaterial;
    }
    public virtual void PlayerJumping(InputAction.CallbackContext context)
    {
        if (context.canceled) IsJumping = false;
        if (context.started && onRoll && CheckWallJump())
        {
            wallJumping = OnRoll = true;
            Invoke("DisableWallJump", 0.25f);
            jumpTimeCounter = jumpTime;
        }
    }
    */
}
