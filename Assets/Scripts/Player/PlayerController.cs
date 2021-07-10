using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    #region Properties
    [SerializeField] CapsuleCollider2D capsule;
    [SerializeField] BoxCollider2D hurtBox;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform shootpoint;
    [SerializeField] Joystick joystick;
    [SerializeField, Range(.01f, 1f)] float jumpTime = 0.35f;
    [Header("Floor config")]
    [SerializeField, Range(.01f, .2f)] float groundDistance = 0.18f;
    [SerializeField, Range(.01f, .2f)] float slopeFrontRay = 0.08f;
    [SerializeField, Range(.01f, .2f)] float slopeBackRay = 0.08f;
    [SerializeField,Range(.01f,1f)] float groundHitSlope; 
    [Header("Running and Speed Booster config")]
    [Tooltip("Standard running without any boost.")]
    [SerializeField, Range(0, 115)] float runningSpeed=100;
    [Tooltip("Speed Booster max speed")]
    [SerializeField, Range(100, 130)] float speedBooster = 115;
    [SerializeField, Range(0, 200)] float maxSpeed;
    [SerializeField, Range(1, 3)] float speedIncreaseOverTime = 1.5f;
    private Vector2 posFrontRay,posBackRay,slopePerp; 
    private RaycastHit2D frontHit,backHit;
    private SkinSwapper skin;
    private float jumpForce = 88, speed = 88, hyperJumpForceMultiplier = 1.8f,frontAngle,backAngle;
    private float xInput = 0, yInput = 0, xVelocity, jumpTimeCounter, currentSpeed,slopeAngle,aim;
    public static Vector2 direction, hyperJumpDir;
    private Animator anim;
    private PlayerInventory inventory;
    private SomePlayerFX playerFX;
    private SpriteRenderer spriteRenderer;
    private Gun gun;
    private bool crouch, fall, wallJumping, isJumping, aimDown, aiming, running, aimUp, firstLand,firstAir,shooting,airShoot,onScrewJump,
       onJumpingState, charged, holdingFire, balled, shootOnWalk, hyperJumping, onRoll, onSlope, inHyperJumpDirection, canCheckFloor = true;
    public System.Action OnJump, OnSpeedBooster;
    int pressCount = -1;
    int[] animatorHash = new int[27];
    public float currentJumpForce { get; set; }
    public bool damaged{get;set;} public bool gravityJump{get;set;}
    public bool screwSelected{get;set;}
    public bool OnRoll
    {
        get=>onRoll;
        set
        {
            onRoll = value;
            if(value){
                if(!screwSelected)playerFX.RollJump(true);
                else{ playerFX.ScrewAttack(true); PlayerHealth.invulnerability=true;}
            }else{
                playerFX.RollJump(false);
                if(screwSelected){PlayerHealth.invulnerability = false; playerFX.ScrewAttack(false);}
            } 
        }
    }
    public float MaxSpeed { get => maxSpeed; set => maxSpeed = value; }
    public float SpeedBS { get => speedBooster; } public float RunningSpeed { get => runningSpeed; }
    public bool inSBVelo { get; set; } public bool hyperJumpCharged { get; set; }
    public bool HyperJumping
    {
        get => hyperJumping;
        set
        {
            hyperJumping = value;
            skin.SetSpeedBooster(value); rb.isKinematic = value;
            if (value)
            {
                playerFX.HyperJump();
                rb.gravityScale = 0;
                PlayerHealth.invulnerability = true;
                onJumpingState = inHyperJumpDirection = movement = canInstantiate = false;
            }
            else{movement = canInstantiate = true;PlayerHealth.invulnerability=false;}
        }
    }
    public static bool canInstantiate;
    public bool leftLook { get; set; }
    public bool IsJumping
    {
        get => isJumping;
        set
        {
            isJumping = value;
            if (isJumping)
            {
                canCheckFloor = isGrounded = false;
                CancelAndInvoke("CheckFloor",.3f);
            }
        }
    }
    void CheckFloor()
    {
        canCheckFloor = true;
    }
    public bool ShootOnWalk
    {
        get => shootOnWalk;
        set
        {
            shootOnWalk = value;
            if (shootOnWalk) CancelAndInvoke("shootingClocking", 2f);
            else CancelInvoke("shootingClocking");
        }
    }
    public bool Balled
    {
        get => balled;
        set
        {
            balled = value;
            if (balled)
            {
                playerFX.Balled();
                aim = 0;
                shootpoint.eulerAngles = new Vector3(0, 0, 0);
                screwSelected = OnRoll = crouch = aimDown = aimUp = false;
            }else{ 
                anim.SetFloat(animatorHash[18], 1);
                spriteRenderer.flipX = false;
            }
        }
    }
    public bool movement { get; set; }
    public Rigidbody2D rb { get; set; }
    public bool isGrounded { get ; set; }
    #endregion
    #region Unity methods
    void Awake()
    {
        OnJump += OnNormalJump;
        jumpTimeCounter = jumpTime;
    }
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerFX = GetComponentInChildren<SomePlayerFX>();
        gun = GetComponentInChildren<Gun>();
        inventory = GetComponent<PlayerInventory>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        skin = GetComponent<SkinSwapper>();
        movement = canInstantiate = true;
        maxSpeed = runningSpeed;
        for (int i = 0; i < anim.parameterCount - 2; i++) animatorHash[i] = Animator.StringToHash(anim.parameters[i].name);
        currentSpeed = speed; currentJumpForce = jumpForce;
    }
    void Update()
    {
        if (movement && !damaged)
        {
            if (canCheckFloor) checkGrounded();
            hurtBox.offset = capsule.offset = new Vector2(0, 0);
            hurtBox.size = new Vector2(.075f, spriteRenderer.bounds.size.y);
            capsule.size = new Vector2(.075f, spriteRenderer.bounds.size.y / transform.lossyScale.y);
            if(capsule.size.y>0.39f)capsule.size=new Vector2(.075f, 0.39f);
            if (isGrounded) OnGround();
            else OnAir();

            if(isJumping){
                if(jumpTimeCounter>0)jumpTimeCounter-=Time.deltaTime;
                else IsJumping=false;
            }
        #if UNITY_ANDROID
            MobileMovement();
        #endif
        }
    }
    void OnDisable()
    {
        ResetState();
        OnJump -= OnNormalJump;
    }
    void FixedUpdate()
    {
        if (!damaged && !hyperJumping)
        {
            xVelocity = xInput * currentSpeed * Time.deltaTime;
            if (isJumping && (movement || jumpTimeCounter > 0f)) rb.velocity = Vector2.up * currentJumpForce * Time.deltaTime;
            if (wallJumping) rb.velocity = Vector2.up * (currentJumpForce / 1.2f) * Time.deltaTime;
            if (isGrounded && xInput!=0)
            {
                rb.velocity= (!onSlope) ? new Vector2(xVelocity,rb.velocity.y) : new Vector2(-xVelocity * slopePerp.x, -xVelocity * slopePerp.y);
                if((frontAngle==0 && backAngle!=frontAngle) && (frontHit.point.y>backHit.point.y) && backAngle!=0){
                    rb.SetVelocity(rb.velocity.x,0f);
                }
            }
            else if (!isGrounded && xInput != 0) rb.SetVelocity(xVelocity, rb.velocity.y);
            else if (!isGrounded && xInput == 0) rb.SetVelocity(0, rb.velocity.y);
        }
        else
        if (hyperJumping) rb.velocity = hyperJumpDir * currentJumpForce * 2.5f * hyperJumpForceMultiplier * Time.deltaTime;
    }
    void LateUpdate()
    {
        if (!hyperJumping)
        {
            anim.SetBool(animatorHash[0], aim < 0);
            anim.SetBool(animatorHash[1], aim > 0);
            anim.SetBool(animatorHash[3], isGrounded && xInput != 0);
            anim.SetBool(animatorHash[4], shootOnWalk || (holdingFire && isGrounded && xInput != 0));
            anim.SetBool(animatorHash[5], crouch);
            anim.SetBool(animatorHash[6], leftLook);
            anim.SetBool(animatorHash[7], aimUp);
            anim.SetBool(animatorHash[8], isGrounded && xInput == 0);//idle
            anim.SetBool(animatorHash[10], onRoll && !screwSelected && !gravityJump);
            anim.SetBool(animatorHash[11], aimDown);
            anim.SetBool(animatorHash[13], (holdingFire || shooting || airShoot) && !isGrounded);
            anim.SetBool(animatorHash[14], screwSelected && onRoll && (!gravityJump || gravityJump));
            anim.SetBool(animatorHash[15], onJumpingState);
            anim.SetBool(animatorHash[16], fall);
            anim.SetBool(animatorHash[17], gravityJump && onRoll && !screwSelected);
            anim.SetBool(animatorHash[2], balled);
            anim.SetBool(animatorHash[9], isGrounded);
        }
        anim.SetFloat(animatorHash[12], rb.velocity.y);
    }
    #endregion
    private void CancelAndInvoke(string methodName, float time)
    {
        CancelInvoke(methodName);
        Invoke(methodName, time);
    }
     
    #region On ground methods
    void OnGround()
    {
        OnSlope();
        if (xInput != 0f)
        {
            if (balled){
                anim.SetFloat(animatorHash[18], 1);
                spriteRenderer.flipX=(xInput<0) ? true:false;
            }
            if (running)
            {
                currentSpeed += speedIncreaseOverTime;
                if (currentSpeed >= speedBooster) { currentSpeed = speedBooster; inSBVelo = true; }
            }
            else currentSpeed = speed;
        }
        else{
            if (balled) anim.SetFloat(animatorHash[18], 0);
            rb.velocity = Vector2.zero;
        }
        if (!balled) OnSpeedBooster?.Invoke();
        else inSBVelo = false;
    }
    void checkGrounded()
    {
        var size = capsule.bounds.size - new Vector3(.01f, 0f);
        RaycastHit2D raycastHit2D = Physics2D.BoxCast(capsule.bounds.center, size, 0f, Vector2.down, groundDistance, groundLayer);

        if (raycastHit2D)
        {
            if (!firstLand) { FalseAnyAnimStateAtGrounding(); firstLand = true; }
            isGrounded = true;
        }
        else isGrounded = false;
    }
    private void FalseAnyAnimStateAtGrounding()
    {
        airShoot=OnRoll = aimUp = aimDown = firstAir = onJumpingState = fall = false;
        playerFX.StopLoopClips();
    }
    public void SpeedBoosterChecker()
    {
        if (inSBVelo && yInput < 0 && !hyperJumpCharged) { hyperJumpCharged = true; inSBVelo = false; skin?.SetSpeedBooster(true); }
        if (hyperJumpCharged && !IsInvoking("HyperJumpTimeAction") && !hyperJumping) { Invoke("HyperJumpTimeAction", 2f); }
    }
    private void OnSlope()
    {
        posFrontRay = new Vector2(transform.position.x + ((capsule.size.x / 2) * direction.x), capsule.bounds.min.y + .025f);
        RaycastHit2D hit2D = Physics2D.Raycast(posFrontRay, Vector2.down, groundHitSlope, groundLayer);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundHitSlope, groundLayer);

        if (hit2D && hit)
        {
            frontAngle = Vector2.Angle(hit2D.normal, Vector2.up);
            slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            slopePerp = Vector2.Perpendicular(hit2D.normal).normalized;
            if ((slopePerp.y < 0 && xInput < 0) || (slopePerp.y > 0 && xInput > 0)) frontAngle *= -1;
            if ((frontAngle > 0) || (frontAngle == 0 && slopeAngle != 0) || slopeAngle != 0) onSlope = true;
            else onSlope = false;

            Debug.DrawRay(transform.position, Vector2.down * groundHitSlope, Color.red);
        }

        posBackRay = new Vector2(transform.position.x - ((capsule.size.x / 2) * direction.x), capsule.bounds.min.y + .025f);
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
    #endregion
    #region On air methods 
    void OnAir()
    {
        FalseAnyAnimStateAtAir();
        if (balled) anim.SetFloat(animatorHash[18], 1);
        if (onJumpingState && xInput != 0) currentSpeed = speed / 2;
        if (!onJumpingState && !onRoll && !aimDown && !aimUp && !gravityJump && !isJumping && !damaged &&
           !screwSelected && !balled && !hyperJumping && !holdingFire && !charged && aim == 0 && !airShoot)
        {
            fall = true;
        }
        else fall = false;

        if (isJumping)
        {
            if (Physics2D.Raycast(transform.position, Vector2.up, 0.22f, groundLayer))
            {
                fall = true;
                OnRoll = onJumpingState = IsJumping = false;
            }
        }
    }
    private bool CheckWallJump()
    {
        if (Physics2D.Raycast(transform.position, Vector2.left, 0.28f, groundLayer) && xInput > 0) return true;
        else if (Physics2D.Raycast(transform.position, Vector2.right, 0.28f, groundLayer) && xInput < 0) return true;
        return false;
    }
    private void FalseAnyAnimStateAtAir()
    {
        if (!firstAir) { rb.gravityScale = 1; firstAir = true; }
        firstLand = onSlope = ShootOnWalk = false;
    }
    #endregion
    #region Delayed Methods
    void Charged()
    {
        charged = true;
    }
    void ChargingShoot()
    {
        gun.Charge(true); Invoke("Charged", 1.8f);
    }
    void shootingClocking()
    {
        ShootOnWalk = false;
    }
    void HoldFire()
    {
        holdingFire = true;
    }
    void DisableWallJump()
    {
        wallJumping = false;
    }
    void HyperJumpTimeAction()
    {
        hyperJumpCharged = inSBVelo = false;
        if (!hyperJumping) skin.SetSpeedBooster(false);
    }
    void ResetSBVelo()
    {
        inSBVelo = false;
    }
    #endregion
    #region Public methods
    public void ResetState()
    {
        StopAllCoroutines();
        //CancelInvoke();
        inSBVelo = crouch = fall = wallJumping = IsJumping =hyperJumpCharged = onJumpingState = 
        charged = holdingFire = ShootOnWalk = isGrounded = HyperJumping = Balled =
        movement = canInstantiate = OnRoll = airShoot=false;
        xInput = yInput = 0;
        rb.velocity = Vector2.zero;
    }
    public void Freeze()
    {
        xInput = yInput = 0;
    }
 
    #region Aim methods
    public void OnAim(InputAction.CallbackContext context)
    {
        if (movement)
        {
            aim = context.ReadValue<float>();
            if (context.performed)
            {
                if (aim > 0) AimUp();
                else if (aim < 0) AimDown();
                aiming = true; ShootOnWalk = false;
            }else
            if (context.canceled)
            {
                LeftRightShootPoint(180,0);
                aiming=aimUp = aimDown = false;
            }
        }
    }
    private void LeftRightShootPoint(float angleLeft,float angleRight){
        shootpoint.eulerAngles=leftLook ? new Vector3(0, 0, angleLeft):new Vector3(0, 0, angleRight);
    }
    private void AimUp()
    {
        LeftRightShootPoint(135,45);
        aimUp = aimDown = false;
        if (inHyperJumpDirection)
        {
            
            if (leftLook)
            {
                HyperJumping = true;
                hyperJumpDir = Vector2.left + Vector2.up;
                anim.SetTrigger("HyperJump L");
            }
            else
            {
                HyperJumping = true;
                hyperJumpDir = Vector2.right + Vector2.up;
                anim.SetTrigger("HyperJump R");
            }
        }
    }
    private void AimDown()
    {
        LeftRightShootPoint(-135,-45);
        aimUp = aimDown = false;
    }
    public void OnLeft(bool value)
    {
        leftLook = value;
        if (value)
        {
            transform.localScale = new Vector2(-.86f, .86f);
            shootpoint.localScale = new Vector3(-1, 1, 0);
            if (aim == 0) shootpoint.eulerAngles = new Vector3(0, 0, 180);
            else if (aim > 0) AimUp();
            else AimDown();
            direction = Vector2.left;
        }
        else
        {
            transform.localScale = new Vector2(.86f, .86f);
            if (aim == 0) shootpoint.eulerAngles = new Vector3(0, 0, 0);
            else if (aim > 0) AimUp();
            else AimDown();
            shootpoint.localScale = new Vector3(1, 1, 0);
            direction = Vector2.right;
        }
        SkinSwapper.OnLeft.Invoke(value);
    }
    #endregion
    public void SelectingAmmo(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pressCount++;
            pressCount = inventory.AmmoSelection(pressCount);
            if (pressCount>3) pressCount = -1;
        }
    }
     
    public void MovementHor(InputAction.CallbackContext context)
    {
        if (movement)
        {
            xInput = context.ReadValue<float>();
            if (!inHyperJumpDirection)
            {
                crouch = false;
                if (xInput < 0 && context.performed) OnLeft(true);
                else if (xInput > 0 && context.performed) OnLeft(false);
            }
            else
            {
                hyperJumpDir = direction;
                if (xInput < 0) anim.SetTrigger("HyperJump L");
                else if (xInput > 0) anim.SetTrigger("HyperJump R");
                HyperJumping = true;
            }
            if (context.canceled)
            {
                xInput = 0;
                if(aim>0)AimUp();
                else if(aim<0)AimDown();
                else LeftRightShootPoint(180,0);
                ShootOnWalk = false;
            }
        }
    }
    public void MovementVer(InputAction.CallbackContext context)
    {
        if (movement && context.started)
        {
            yInput = context.ReadValue<float>();
            if (!inHyperJumpDirection)
            {
                if (yInput > 0f)
                {
                    if (aim == 0 && !crouch && !balled) shootpoint.eulerAngles = new Vector3(0, 0, 90);
                    if (!balled && !crouch && isGrounded) { aimDown = false; aim = 0; aimUp = true; }
                    if (crouch) crouch = false;
                    if (!balled && xInput == 0f && !aiming && !isGrounded) { aimUp = true; aim = 0; aimDown = false; }
                    else if (balled && isGrounded) { crouch = true; Balled = false; }
                    else if (!isGrounded && balled && yInput > 0 && Physics2D.Raycast(transform.position, Vector2.up, groundDistance, groundLayer))
                    {
                        Balled = false;
                        aimUp = true;
                    }
                }
                else if (yInput < 0f)
                {
                    if (!balled && xInput == 0f && !aiming && !isGrounded) { aimUp = false; aim = 0; aimDown = true; }
                    if (inventory.CheckItem(4) && crouch) Balled = true;
                    if (!balled && isGrounded) crouch = true;
                    if (aim == 0 && !crouch && !balled) shootpoint.eulerAngles = new Vector3(0, 0, -90);
                }
            }
            else
            {
                if (yInput > 0) { hyperJumpDir = Vector2.up; anim.SetTrigger("HyperJump up"); }
                HyperJumping = true;
            }
            if (yInput == 0)
            {
                yInput = 0;
                aimUp = aimDown = false;
                shootpoint.eulerAngles= leftLook ? new Vector3(0, 0, 180) : new Vector3(0, 0, 0); 
            }
        }
    }
    public void InstantMorfBall(InputAction.CallbackContext context)
    {
        if (context.started && movement && inventory.CheckItem(4))
        {
            ShootOnWalk = false;
            if (balled)
            {
                if (isGrounded)
                {
                    if (!Physics2D.Raycast(transform.position, Vector2.up, groundDistance, groundLayer)) { crouch = true; Balled = false; }
                }
                else
                {
                    if (!Physics2D.Raycast(transform.position, Vector2.up, groundDistance, groundLayer)) Balled = crouch = false;
                    OnRoll = true;
                }
            }
            else Balled = true;
        }
    }
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started && canInstantiate && movement)
        {
            if (!isGrounded) {airShoot=true; OnRoll = onJumpingState = false; }
            if (balled) { gun.SetBomb(); }
            else
            {
                if (pressCount != 4)
                {
                    if (xInput!=0 && isGrounded && !onRoll && !aiming) ShootOnWalk = true;
                    gun.Shoot(false);
                }
            }
            if (inventory.CheckItem(0)) Invoke("ChargingShoot", 0.25f);//charge beam
            shooting=true;
        }
        if (context.canceled)
        {
            if (charged) { gun.Shoot(true); charged = false; }
            CancelInvoke("Charged"); gun.Charge(false);
            CancelInvoke("ChargingShoot"); shooting = holdingFire = false;
            CancelInvoke("HoldFire");
        }
        else { CancelAndInvoke("HoldFire", 0.5f); }
    }
    public void OnRunning(InputAction.CallbackContext context)
    {
        if (isGrounded && !balled && context.performed && movement && xInput != 0)
        {
            rb.gravityScale = 2; running = true;
        }
        if (context.canceled && isGrounded && !balled) { currentSpeed = speed; running = false; rb.gravityScale = 1; Invoke("ResetSBVelo", .5f); }
    }
    #region Player jumping Methods
    public void OnGravityJump()
    {
        if (xInput != 0 && !balled){IsJumping = OnRoll = true;}
        else
        {
            if (hyperJumpCharged) { onJumpingState = inHyperJumpDirection = true; }
            else if (!hyperJumpCharged && isGrounded && xInput==0)
            {
                IsJumping = onJumpingState = true; OnRoll = false;
            }
        }
        jumpTimeCounter = jumpTime;
    }
    public void OnNormalJump()
    {
        if (isGrounded && !crouch && !isJumping)
        {
            if (balled) playerFX.BallJump();
            else
            {
                if (xInput != 0)IsJumping = OnRoll = true;//no gravity jump
                else
                {
                    if (hyperJumpCharged) { onJumpingState = inHyperJumpDirection = true; }
                    else if (!hyperJumpCharged) { IsJumping = onJumpingState = true; OnRoll = false; }
                }
            }
        }
    }
    public void PlayerJumping(InputAction.CallbackContext context)
    {
        if (context.started && movement){if (isGrounded) jumpTimeCounter = jumpTime; OnJump.Invoke();}
        if (context.canceled) IsJumping = false;
        if (context.started && onRoll && CheckWallJump() && movement)
        {
            wallJumping =IsJumping= OnRoll = true;
            Invoke("DisableWallJump", 0.25f);
        }
    }
    #endregion

#if UNITY_ANDROID
    public void PlayerJumping_Mobile(bool triggered)
    {
        if (triggered)
        {
            OnJump.Invoke();
            if (onRoll && CheckWallJump())
            {
                wallJumping = OnRoll = true;
                Invoke("DisableWallJump", 0.25f);
                StartCoroutine(JumpCoroutine());
            }
        }
        if (!triggered) IsJumping = false;
    }
    private void MobileMovement()
    {
        if (joystick.Horizontal < -0.25) { xInput = -1; direction = Vector2.left; }
        else if (joystick.Horizontal > 0.25) { xInput = 1; direction = Vector2.right; }
        else
        {
            xInput = 0; moveOnFloor = false; xInput = 0;
            CancelInvoke("enableWalking");
            movingOnAir = ShootOnWalk = moveOnFloor = false;
        }
        if (joystick.Vertical < -0.25) yInput = -1;
        else if (joystick.Vertical > 0.25) yInput = 1;
        else { yInput = 0; }
        if (!inHyperJumpDirection)
        {
            crouch = false;
            if (xInput < 0)
            {
                shootpoint.localScale = new Vector3(-1, 1, 0);
                if (aim == 0) shootpoint.eulerAngles = new Vector3(0, 0, 180);
                direction = Vector2.left; SkinSwapper.OnLeft.Invoke(true);
            }
            else if (xInput > 0)
            {
                shootpoint.localScale = new Vector3(1, 1, 0);
                if (aim == 0) shootpoint.eulerAngles = new Vector3(0, 0, 0);
                direction = Vector2.right; SkinSwapper.OnLeft.Invoke(false);
            }
            if(xInput==0){
                if (yInput > 0f)
                {
                    if (!balled && !crouch) gravityJump = Screwing = false;
                    if (aim == 0 && !crouch && !balled) shootpoint.eulerAngles = new Vector3(0, 0, 90);
                    if (!balled && !crouch && isGrounded) { aimDown = false; aim = 0; aimUp = true; }
                    if (crouch) crouch = false;
                    if (!balled && xInput == 0f && !aiming && !isGrounded) { aimUp = true; aim = 0; aimDown = false; }
                    else if (balled && isGrounded) { crouch = true; Balled = false; }
                    else if (!isGrounded && balled && yInput > 0 && Physics2D.Raycast(transform.position, Vector2.up, groundDistance, groundLayer))
                    {
                        Balled = false;
                        aimUp = true;
                    }
                }
                else if (yInput < 0f)
                {
                    if (!balled && xInput == 0f && !aiming && !isGrounded) { aimUp = false; aim = 0; aimDown = true; }
                    if (inventory.CheckItem(4) && crouch) Balled = true;
                    if (!balled && isGrounded) crouch = true;
                    if (aim == 0 && !crouch && !balled) shootpoint.eulerAngles = new Vector3(0, 0, -90);
                }
                else
                {
                    aimUp = aimDown = false;
                    if(aim==0){
                        if (leftLook) shootpoint.eulerAngles = new Vector3(0, 0, 180);
                        else shootpoint.eulerAngles = new Vector3(0, 0, 0);
                    }
                }
            }
        }
        else
        {
            hyperJumpDir = direction;
            if (yInput > 0) { hyperJumpDir = Vector2.up; anim.SetTrigger("HyperJump up"); }
            else if (xInput < 0) anim.SetTrigger("HyperJump L");
            else if (xInput > 0) anim.SetTrigger("HyperJump R");
            HyperJumping = true;
        }
    }
    public void Diagonal_Mobile(int value)
    {
        if (movement)
        {
            aim=value;
            if (value > 0)AimUp();
            else if (value < 0)AimDown();
            
            aiming = true; gravityJump = Screwing = ShootOnWalk = false;
            CheckAirShoot();
            CancelInvoke("DisableAimDiagonal");
            Invoke("DisableAimDiagonal", 3);
        }
    }
    private void DisableAimDiagonal()
    {
        LeftRightShootPoint(180, 0);
        aiming = aimUp = aimDown = false;
        aim = 0;
    }
#endif
    #endregion
}