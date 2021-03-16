using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AndroidController : MonoBehaviour
{
    /*
    bool jumpTouching, fireTouching, tap, idle;
    int tapCount;
    public void PlayerJump(bool jumpState)
    {
        jumpTouching = jumpState;

        if (jumpTouching && _isGrounded && !crouch && !isJumping)
        {
            if (!balled && (horizontalInput < 0.5f && horizontalInput > -0.5f)) onJumpingState = true;
            onJumpState = jump = true;
            jumpTimeCounter = jumpTime;
            gravityJump = isGravityJumping = _isGrounded = false;
            crouch = false;
        }
        if (jumpTouching && jump)
        {
            isJumping = true;
            if (jumpTimeCounter > 0f)
            {
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                jumpTouching = onJumpState = isJumping = jump = false;
            }
        }
        if (!jumpTouching)
        {
            onJumpState = isJumping = jump = false;
        }
        //Is trying to wall jump?
        if (!balled && !_isGrounded && onRoll && (Physics2D.Raycast(transform.position, Vector2.left, 0.4f, groundLayer) && horizontalInput > 0.5f
            || Physics2D.Raycast(transform.position, Vector2.right, 0.4f, groundLayer) && horizontalInput < 0.5f) && jumpTouching)
        {
            isWallJumping = onRoll = jump = true;
            jumpTimeCounter = jumpTime / 1.5f;
        }
        else
        {
            isWallJumping = false;
        }
    }
    void ResetTapCount()
    {
        tapCount = 0;
    }
    public void CrouchMorfBall()
    {
        CancelInvoke("ResetTapCount");
        tapCount++;
        if (tapCount == 1) Invoke("ResetTapCount", 0.4f);
        switch (tapCount)
        {
            case 1:
                switch (crouch)
                {
                    case true:
                        if (_isGrounded) crouch = false;
                        break;
                    case false:
                        switch (balled)
                        {
                            case true:
                                if (_isGrounded)
                                {
                                    if (!Physics2D.Raycast(transform.position, Vector2.up, groundDistance, groundLayer))
                                    {
                                        balled = false; crouch = true;
                                    }
                                }
                                else
                                {
                                    if (!Physics2D.Raycast(transform.position, Vector2.up, groundDistance, groundLayer))
                                    {
                                        balled = false;
                                    }
                                }
                                break;
                            case false:
                                crouch = true;
                                break;
                        }
                        break;
                }
                break;
            case 2:
                if (!balled && inventory.SelectMorfBall) { balled = true; crouch = false; }
                tapCount = 0;
                break;
        }
    }
    public void DiagonalUp()
    {
        if ((!crouch || crouch) && (!walkingFloor || walkingFloor) && movement)
        {
            CancelInvoke("CancelDiagonal");
            diagonalLook = true; lookDownDiagonal = false;
            Invoke("CancelDiagonal", 3f);
        }
    }
    public void CancelDiagonal()
    {
        lookDownDiagonal = diagonalLook = false;
    }
    public void DiagonalDown()
    {
        if ((!crouch || crouch) && (!walkingFloor || walkingFloor) && movement)
        {
            CancelInvoke("CancelDiagonal");
            diagonalLook = false; lookDownDiagonal = true;
            Invoke("CancelDiagonal", 3f);
        }
    }
    public void SelectAmmo(int itemSelect)
    {
        switch (itemSelect)
        {
            case 1:
                if (inventory.missiles > 0)
                {
                    sMissilesSelected = superBombsSelected = false;
                    missilesSelected = true;
                    inventory.SelectedItems(1, true);
                }
                break;
            case 2:
                if (inventory.superMissiles > 0)
                {
                    missilesSelected = superBombsSelected = false;
                    sMissilesSelected = true;
                    inventory.SelectedItems(2, true);
                }
                break;
            case 3:
                if (inventory.superBombs > 0)
                {
                    missilesSelected = sMissilesSelected = false;
                    superBombsSelected = true;
                    inventory.SelectedItems(3, true);
                }
                break;
            case 4:
                missilesSelected = sMissilesSelected = superBombsSelected = false;
                inventory.SelectedItems();
                break;
        }
    }
    private void variousMovement()
    {
        _movement = new Vector2(horizontalInput, 0f);
        switch (!movement)
        {
            case true:
                horizontalInput = 0; verticalInput = 0;
                _movement = new Vector2(0f, 0f);
                break;

            case false:
                _movement = new Vector2(horizontalInput, 0f);
                break;
        }
        switch (_isGrounded)
        {
            case true:
                if (!runBooster)
                {
                    moveOnJumpState = false;
                    currentSpeed = speed;
                    if (!running) currentSpeed = speed;
                }
                noFall = movingOnAir = false;
                if (_rigidbody.velocity.y == 0f) onJumpingState = false;
                if (horizontalInput < -0.25f || horizontalInput > 0.25f)
                {
                    crouch = idle = false;
                    if (horizontalInput < -0.25f) leftLook = true;
                    else if (horizontalInput > 0.25f) leftLook = false;
                    _rigidbody.sharedMaterial.friction = 0f;
                    Invoke("enableWalking", 0.05f);
                }
                else
                {
                    idle = true;
                    CancelInvoke("enableWalking");
                    _rigidbody.sharedMaterial.friction = 10f;
                    walkingFloor = false;
                }
                break;
            case false:
                walkingFloor = false;
                if (horizontalInput < -0.25f || horizontalInput > 0.25f)
                {
                    if (horizontalInput < -0.25f) leftLook = true;
                    else if (horizontalInput > 0.25f) leftLook = false;
                    movingOnAir = true;
                    _rigidbody.sharedMaterial.friction = 0f;
                }
                if (verticalInput < -0.25f) lookDown = true;
                else lookDown = false;
                break;
        }

        if (verticalInput > 0.25f && !walkingFloor && !crouch) lookUp = true;
        else lookUp = false;

        if (lookUp && (walkingFloor || diagonalLook || lookDownDiagonal))
        {
            lookUp = false;
        }

        if ((inventory.HighJump || inventory.HighJump_Temp) && (inventory.SelectHJ || inventory.SelectHJ_Temp)) currentJumpForce = inventory.jumpForceHJ;
        else currentJumpForce = jumpForce;

        if ((_isGrounded && canRollFalse && _rigidbody.velocity.y == 0f) || fireTouching) { onRoll = canRollFalse = false; }

        if (jumpTouching && !crouch && movingOnAir && !balled && !inventory.SelectGJ)
        {
            walkingFloor = _isGrounded = moveOnJumpState = false;
            onRoll = true;
            CancelInvoke("RollingCanBeFalse");
            Invoke("RollingCanBeFalse", 0.2f);
        }

        if (inventory.SelectScrew && onRoll)
        {
            screwing = true; // screw attack
        }
        else screwing = false;
    }
    public void Fire()
    {
        if (playerCanInstantiate)
        {
            if (instantiates.FirePoint.activeSelf || (!onRoll && walkingFloor))
            {
                noFall = fireTouching = true;
                //Is Shooting or Shooting when sprinting?
                switch (balled)
                {
                    case true:
                        instantiates.plantBombs();
                        break;
                    case false:
                        if (fireTouching && !onRoll)
                        {
                            if (walkingFloor)
                            {
                                CancelInvoke("shootingClocking");
                                shooting = true;
                                Invoke("shootingClocking", 2f);
                            }
                            if (!missilesSelected && !sMissilesSelected)
                            {
                                switch (inventory.SelectIceB)
                                {
                                    case true:
                                        if (!inventory.SelectSpazer) instantiates.shootIBeam();
                                        else instantiates.shootSpazerIce();

                                        break;
                                    case false:
                                        if (!inventory.SelectSpazer) instantiates.Shoot();
                                        else instantiates.shootSpazerBeam();
                                        break;
                                }
                            }
                            if (missilesSelected && inventory.missiles > 0) instantiates.MissileShoot();
                            else if (inventory.missiles <= 0 && missilesSelected) { inventory.SelectedItems(1, false); missilesSelected = false; }
                            if (sMissilesSelected && inventory.superMissiles > 0) instantiates.SMissileShoot();
                            else if (inventory.superMissiles <= 0 && sMissilesSelected) { inventory.SelectedItems(2, false); sMissilesSelected = false; }
                        }
                        break;
                }
                if (shooting && (horizontalInput == 0f || jump || crouch || balled || onRoll))//to stop the shooting anim
                {
                    CancelInvoke("shootingClocking");
                    shooting = false;
                }
                fireTouching = false;
            }
        }
    }
    */
}