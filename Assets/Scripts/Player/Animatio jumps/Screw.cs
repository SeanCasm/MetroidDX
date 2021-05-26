using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Screw : PlayerController
{
    private PlayerController player;
    private void Awake() {
        player=GetComponentInParent<PlayerController>();
    } 
    new public void PlayerJumping(InputAction.CallbackContext context)
    {
        
            if (xInput != 0)
            {
                if (context.started && !crouch && movement)
                {
                    airShoot = false;
                    if (inventory.CheckItem(5)) gravityJump = Screwing = true;//screw
                    jumpTimeCounter = jumpTime;
                    IsJumping = true;
                }
            }
            else
            {
                if (hyperJumpCharged && yInput > 0) { HyperJumping = true; }
                else if (!hyperJumpCharged && isGrounded)
                {
                    onJumpingState = true; gravityJump = OnRoll = false;
                    jumpTimeCounter = jumpTime; IsJumping = true;
                }
            }
        if (context.canceled) IsJumping = false;
        if (context.started && onRoll && CheckWallJump())
        {
            wallJumping = OnRoll = true;
            Invoke("DisableWallJump", 0.25f);
            jumpTimeCounter = jumpTime;
        }
    }
}
