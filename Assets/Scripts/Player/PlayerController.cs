using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    #region Properties
    [SerializeField]CapsuleCollider2D capsule;
    [SerializeField]BoxCollider2D hurtBox;
    [SerializeField] LayerMask groundLayer,enemyLayer;
    [Header("Floor config")]
    [SerializeField] Transform feetPosition;
    [Range(.01f,.2f)]
    [SerializeField]float groundDistance= 0.18f;
    [Header("Running and Speed Booster config")]
    [Tooltip("Standard running without any boost.")]
    [Range(0,115)]
    [SerializeField]float runningSpeed;
    [Tooltip("Speed Booster max speed")]
    [Range(130,150)]
    [SerializeField]float speedBooster=130;
    [Range(115,200)]
    [SerializeField] float maxSpeed;
    [Range(1, 3)]
    [SerializeField]float speedIncreaseOverTime=1.5f;
    private SkinSwapper skin;
    private float jumpForce = 88, speed = 88,
        hyperJumpForceMultiplier = 1.8f, jumpTime = 0.35f;
    private float xInput = 0, yInput = 0, xVelocity, yVelocity, jumpTimeCounter,currentSpeed;
    public static Vector2 direction,hyperJumpDir;
    private Animator anim;
    private PlayerInventory inventory;
    private SomePlayerFX playerFX;
    private SpriteRenderer spriteRenderer;
    private PlayerInstantiates instantiates;
    private bool crouch, fall, wallJumping, airShoot, movingOnAir, isJumping, damaged, moveOnFloor, aimDiagonalDown,gravityJump,aimDown,aiming,running,aimUp,
       onJumpingState, charged, holdingFire, balled, shootOnWalk, isGrounded, screwing, hyperJumping, onRoll, onSlope,aimDiagonal,inHyperJumpDirection,canCheckFloor=true;
    private PhysicsMaterial2D material;
    public System.Action OnJump,OnSpeedBooster;
    int pressCount = 0;
    int[] animatorHash = new int[27];
    public float currentJumpForce { get; set; }
    private bool OnRoll
    {
        set
        {
            onRoll = value;
            if (value && !screwing) playerFX.RollJump(true);
            else playerFX.RollJump(false);
        }
    }
    public float MaxSpeed{get=>maxSpeed;set=>maxSpeed =value;}
    public float SpeedBS{get=>speedBooster;}
    public float RunningSpeed{get=>runningSpeed;}
    public bool hittedLeft{get;set;}
    public bool hitted{get;set;}
    public bool inSBVelo { get; set; }
    public bool hyperJumpCharged { get; set; }
    public bool Damaged
    {
        get => damaged;
        set
        {
            damaged = value;
            if (damaged) ResetState();
        }
    }
    public bool HyperJumping
    {
        get => hyperJumping;
        set
        {
            hyperJumping = value;
            skin.SetSpeedBooster(value); rb.isKinematic = value;
            if (value){
                playerFX.HyperJump();
                rb.gravityScale=0;
                onJumpingState = inHyperJumpDirection=movement=canInstantiate=false;
                
                
            }else{
                movement=canInstantiate=true;
            }
        }
    }
    public bool canInstantiate { get; set; }
    public bool Screwing
    {
        get => screwing;
        set
        {
            screwing = value;
            if (screwing) { skin?.SetScrewAttack(true); playerFX.ScrewAttack(true); }
            else { skin?.SetScrewAttack(false); playerFX.ScrewAttack(false); }
        }
    }
    public bool leftLook { get; set; }
    public bool IsJumping
    {
        get => isJumping;
        set
        {
            isJumping = value;
            if (isJumping)
            {
                canCheckFloor=false;
                StartCoroutine(CheckFloor());
                IsGrounded = moveOnFloor = false;
            }
            material.friction = 0;
        }
    }
    IEnumerator CheckFloor()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        canCheckFloor=true;

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
                Screwing = OnRoll = crouch = aimDiagonal = aimDown = aimUp = aimDiagonalDown = false;
            }
            else anim.SetFloat(animatorHash[23], 1);
        }
    }
    public bool movement { get; set; }
    public Rigidbody2D rb { get; set; }
    public bool IsGrounded{get => isGrounded;set{isGrounded = value;}}
    #endregion
    #region Unity methods
    void Awake()
    {
        OnJump += OnNormalJump;
        spriteRenderer=GetComponentInChildren<SpriteRenderer>();
        playerFX = GetComponentInChildren<SomePlayerFX>();
        instantiates = GetComponentInChildren<PlayerInstantiates>();
        inventory = GetComponent<PlayerInventory>();
        rb = GetComponent<Rigidbody2D>();
        material = rb.sharedMaterial; 
        anim = GetComponentInChildren<Animator>();
        skin = GetComponent<SkinSwapper>();
    }
    void Start()
    {
        movement = canInstantiate = true;
        maxSpeed=runningSpeed;
        for (int i = 0; i < anim.parameterCount - 2; i++) animatorHash[i] = Animator.StringToHash(anim.parameters[i].name);
        currentSpeed = speed; currentJumpForce = jumpForce;
    }
    void Update()
    {
        if (movement)
        {
            if(canCheckFloor)checkGrounded();
            hurtBox.offset=capsule.offset=new Vector2(0,0);
            hurtBox.size=new Vector2(.10f,spriteRenderer.bounds.size.y);
            capsule.size=new Vector2(.10f,spriteRenderer.bounds.size.y / transform.lossyScale.y);

            if (isGrounded) OnGround();
            else OnAir();
            if (xInput < 0) leftLook = true;
            else if (xInput > 0) leftLook = false;

#if UNITY_ANDROID
            MobileMovement();
#endif
        }
    }
    void checkGrounded(){
        RaycastHit2D raycastHit2D=Physics2D.BoxCast(capsule.bounds.center,capsule.bounds.size,0f,Vector2.down,groundDistance,groundLayer);
        RaycastHit2D raycastHitEnemy=Physics2D.BoxCast(capsule.bounds.center, capsule.bounds.size, 0f, Vector2.down, groundDistance, enemyLayer);

        if(raycastHit2D || raycastHitEnemy.collider.gameObject.GetComponent<EnemyHealth>().freezed )isGrounded=true;
        else isGrounded=false;
        
        Debug.DrawRay(capsule.bounds.center+new Vector3(capsule.bounds.extents.x,0),Vector2.down*(capsule.bounds.extents.y+groundDistance),Color.green);
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
            yVelocity = currentSpeed * Time.deltaTime;
            if (isJumping && (movement || jumpTimeCounter > 0f))rb.velocity = Vector2.up * currentJumpForce * Time.deltaTime;
            if (wallJumping) rb.velocity = Vector2.up * (currentJumpForce / 1.2f) * Time.deltaTime;
            if (moveOnFloor)
            {
                if (!onSlope) rb.SetVelocity(xVelocity, 0);
                else
                {
                    if (slopeUp) rb.SetVelocity(xVelocity, yVelocity / 1.2f);
                    else rb.SetVelocity(xVelocity, -yVelocity);
                }
            }
            else if (movingOnAir) rb.SetVelocity(xVelocity, rb.velocity.y);
            else if(!movingOnAir)rb.SetVelocity(0, rb.velocity.y);
        }
        else
        if (hyperJumping) rb.velocity = hyperJumpDir * currentJumpForce*2.5f * hyperJumpForceMultiplier * Time.deltaTime;
    }
    void LateUpdate()
    {
        if(!hyperJumping){
            anim.SetBool(animatorHash[0], aimDiagonalDown);
            anim.SetBool(animatorHash[1], aimDiagonal);
            anim.SetBool(animatorHash[3], moveOnFloor);
            anim.SetBool(animatorHash[4], shootOnWalk || (holdingFire && isGrounded));
            anim.SetBool(animatorHash[5], crouch);
            anim.SetBool(animatorHash[6], leftLook);
            anim.SetBool(animatorHash[7], aimUp);
            anim.SetBool(animatorHash[8], isGrounded && xInput == 0);//idle
            anim.SetBool(animatorHash[10], onRoll);
            anim.SetBool(animatorHash[11], aimDown);
            anim.SetBool(animatorHash[13], airShoot || (holdingFire && !isGrounded));
            anim.SetBool(animatorHash[14], screwing);
            //anim.SetBool(animatorHash[15], hyperJumping);
            anim.SetBool(animatorHash[15], onJumpingState);
            anim.SetBool(animatorHash[16], fall);
            anim.SetBool(animatorHash[17], gravityJump);
            anim.SetBool(animatorHash[2], balled);
            anim.SetBool(animatorHash[9], isGrounded);
        }
        anim.SetFloat(animatorHash[12], rb.velocity.y);


    }
    #endregion
    #region Private methods
    private void CancelAndInvoke(string methodName, float time)
    {
        CancelInvoke(methodName);
        Invoke(methodName, time);
    }
    void OnAir()
    {
        FalseAnyAnimStateAtAir();
        if (onJumpingState && xInput != 0) currentSpeed = speed / 2;
        if (xInput != 0f) movingOnAir = true;
        else movingOnAir = false;
        if (!onJumpingState && !onRoll && !aimDown && !aimUp && !gravityJump && !isJumping && !damaged &&
           !screwing && !airShoot && !balled && !hyperJumping && !holdingFire && !charged && !aimDiagonal &&
           !aimDiagonalDown)
        {
            fall = true;
        }
        else fall = false;

        if (isJumping)
        {
            if (Physics2D.Raycast(transform.position, Vector2.up, 0.22f, groundLayer))
            {
                jumpTimeCounter = 0f;
                IsJumping = false;
            }
            else if (jumpTimeCounter > 0f) jumpTimeCounter -= Time.deltaTime;
            else IsJumping = false;
        }
    }
    void OnGround()
    {
        FalseAnyAnimStateAtGrounding();
        playerFX.StopLoopClips();
        if (xInput != 0f)
        {
            if(running){
                currentSpeed += speedIncreaseOverTime;
                if (currentSpeed >= speedBooster) { currentSpeed = speedBooster; inSBVelo = true; }
            }else currentSpeed=speed;
            OnSlope();
            material.friction = 0f;
            if (yInput > 0) { aimDiagonal = true; aimUp = aimDown = aimDiagonalDown = false; }
            else if (yInput < 0) { aimDiagonalDown = true; aimUp = aimDown = aimDiagonal = false; }
            if (!moveOnFloor) Invoke("enableWalking", 0.05f);
        }
        else
        {
            rb.velocity = Vector2.zero;
            //currentSpeed = speed;
        }
        if (!balled){
            OnSpeedBooster?.Invoke();
        }
        else inSBVelo = false;
    }
    public void SpeedBoosterChecker(){
        if (inSBVelo && yInput < 0 && !hyperJumpCharged) { hyperJumpCharged = true; inSBVelo = false; skin?.SetSpeedBooster(true); }
        if (hyperJumpCharged && !IsInvoking("HyperJumpTimeAction") && !hyperJumping) { Invoke("HyperJumpTimeAction", 2f); }
    }
    float slopeAngle = 0; bool slopeUp;
    private void OnSlope()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position, Vector2.down, 0.3f, groundLayer);
        if (hit2D)
        {
            slopeAngle = Vector2.Angle(hit2D.normal, Vector2.up);
            if (slopeAngle != 0)
            {
                onSlope = true;
                if (Physics2D.Raycast(feetPosition.position, direction, 0.2f, groundLayer)) slopeUp = true;
                else slopeUp = false;
            }
            else slopeUp = onSlope = false;
        }
    }
    private bool CheckWallJump()
    {
        if (RayCast(Vector2.left, 0.28f, groundLayer) && xInput > 0) return true;
        else if (RayCast(Vector2.right, 0.28f, groundLayer) && xInput < 0) return true;
        return false;
    }
    private bool RayCast(Vector2 vector, float distance, LayerMask layer)
    {
        if (Physics2D.Raycast(transform.position, vector, distance, layer)) return true;
        return false;
    }
    #region Delayed Methods
    void Charged()
    {
        charged = true;
    }
    void ChargingShoot()
    {
        instantiates.Charge(true); Invoke("Charged", 1.8f);
    }
    void shootingClocking()
    {
        ShootOnWalk = false;
    }
    void StopPlayerFire()
    {
        airShoot = false;
    }
    void HoldFire()
    {
        holdingFire = true;
    }
    void DisableWallJump()
    {
        wallJumping = false;
    }
    void enableWalking()
    {
        moveOnFloor = true;
    }
    void HyperJumpTimeAction()
    {
        hyperJumpCharged = inSBVelo = false;
        if(!hyperJumping)skin.SetSpeedBooster(false);
    }
    void ResetSBVelo(){
        inSBVelo=false;
    }
    #endregion
    #endregion
    #region Public methods
    private void FalseAnyAnimStateAtGrounding()
    {
        airShoot = onJumpingState = Screwing = OnRoll = gravityJump = fall = movingOnAir = false;
    }
    private void FalseAnyAnimStateAtAir()
    {
        rb.gravityScale = 1;
        slopeUp = onSlope = ShootOnWalk = moveOnFloor = false;
    }
    public void ResetState()
    {
        StopAllCoroutines();
        CancelInvoke();
        inSBVelo=crouch = fall = wallJumping = airShoot = movingOnAir = isJumping = moveOnFloor = gravityJump =
        hyperJumpCharged=onJumpingState = charged = holdingFire = ShootOnWalk = IsGrounded = Screwing = HyperJumping = Balled =
        movement = canInstantiate = OnRoll = false;
        xInput = yInput = 0;
        rb.velocity = Vector2.zero;
    }
    public void Freeze()
    {
        xInput = yInput = 0;
    }
    public void OnAim(InputAction.CallbackContext context)
    {
        if (movement)
        {
            float aim = context.ReadValue<float>();
            if (context.performed)
            {
                if (aim > 0) { aimDiagonal = true; aimUp = aimDown = aimDiagonalDown = false; 
                    if(inHyperJumpDirection){
                        if(leftLook){
                            HyperJumping = true;
                            hyperJumpDir=Vector2.left+Vector2.up;
                            anim.SetTrigger("HyperJump L");
                        }else{
                            HyperJumping = true;
                            hyperJumpDir = Vector2.right + Vector2.up;
                            anim.SetTrigger("HyperJump R");
                        }
                    }
                }
                else if (aim < 0) { aimUp = aimDown = aimDiagonal = false; aimDiagonalDown = true; }
                aiming = true; gravityJump = Screwing = ShootOnWalk = false;
                CheckAirShoot();
            }
            else if (context.canceled)
            {
                if (aim == 0) aimUp = aimDown = aimDiagonal = aimDiagonalDown = false;
                aiming = false;
            }
        }
    }
    public void SelectingAmmo(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            pressCount++;
            pressCount = inventory.AmmoSelection(pressCount);
            if (pressCount == 4) pressCount = 0;
        }
    }
    public void Movement(InputAction.CallbackContext context)
    {
        if (movement)
        {
            xInput = context.ReadValue<Vector2>().x;
            yInput = context.ReadValue<Vector2>().y;
            if (xInput == 0) InputX();
            else
            {
                if(!inHyperJumpDirection){
                    crouch = false;
                    if (xInput < 0) direction = Vector2.left;
                    else if (xInput > 0) direction = Vector2.right;
                }else{
                    hyperJumpDir=direction;
                    if(xInput<0)anim.SetTrigger("HyperJump L");
                    else if (xInput > 0) anim.SetTrigger("HyperJump R");
                    HyperJumping=true;  
                }
            }
            if (yInput == 0) InputY();
            else
            {
                if(!inHyperJumpDirection){
                    if (!balled && !crouch)
                    {
                        if (yInput < 0f && !isGrounded) { aimDown = true; aimDiagonal = aimDiagonalDown = aimUp = false; }
                        if (yInput > 0f && xInput == 0f && !aiming) { aimUp = true; aimDiagonalDown = aimDiagonal = aimDown = false; }
                        gravityJump = Screwing = false;
                    }
                    if (isGrounded && context.started)
                    {
                        if (crouch)
                        {
                            if (yInput > 0) crouch = false;
                            else if (yInput < 0 && inventory.CheckItem(4)) Balled = true;
                        }
                        else
                        {
                            if (balled) { if (yInput > 0) { crouch = true; Balled = false; } }
                            else { if (yInput < 0) crouch = true; }
                        }
                    }
                    else
                    {
                        if (balled)
                        {
                            if (yInput > 0 && Physics2D.Raycast(transform.position, Vector2.up, groundDistance, groundLayer))
                            {
                                Balled = false;
                            }
                        }
                    }
                }else{
                    if(yInput>0){hyperJumpDir=Vector2.up;anim.SetTrigger("HyperJump up");}
                    HyperJumping=true; 
                }
                 
            }
        }
        else if (!movement)
        {
            if (xInput == 0) InputX();
            if (yInput == 0) InputY();
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
            else
            {
                Balled = true;
            }
        }
    }
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started && canInstantiate)
        {
            if (!isGrounded) { CheckAirShoot(); OnRoll = gravityJump = Screwing = onJumpingState = false; }
            if (balled) { instantiates.SetBomb(); }
            else
            {
                if (pressCount != 3)
                {
                    if (moveOnFloor && !onRoll && !aiming) ShootOnWalk = true;
                    GameEvents.playerFire.Invoke(false);
                }
            }
            if (inventory.CheckItem(0)) Invoke("ChargingShoot", 0.25f);//charge beam
        }
        if (context.canceled)
        {
            if (charged) { GameEvents.playerFire.Invoke(true); charged = false; }
            CancelInvoke("Charged"); instantiates.Charge(false);
            CancelInvoke("ChargingShoot"); holdingFire = false;
            CancelInvoke("HoldFire");
        }
        else { CancelAndInvoke("HoldFire", 0.5f); }
    }
    public void OnRunning(InputAction.CallbackContext context)
    {
        if (isGrounded && !balled && context.performed && movement && xInput!=0)
        {
            rb.gravityScale = 2;running=true;
        }
        if (context.canceled && isGrounded && !balled) { currentSpeed = speed; running=false; rb.gravityScale = 1;Invoke("ResetSBVelo",.5f); }
    }
    #region Player jumping Methods
    public void OnGravityJump()
    {
        if (xInput != 0 && !isGrounded && !balled)
        {
            if (!crouch && movement)
            {
                airShoot = false;
                if (inventory.CheckItem(5)) gravityJump = Screwing = true;//screw
                else { Screwing = onJumpingState = OnRoll = false; gravityJump = true; }
                jumpTimeCounter = jumpTime;
                IsJumping = true;
            }
        }
        else
        {
            if (hyperJumpCharged ) { onJumpingState = inHyperJumpDirection = true; }
            else if (!hyperJumpCharged && isGrounded)
            {
                onJumpingState = true; gravityJump = OnRoll = false;
                jumpTimeCounter = jumpTime; IsJumping = true;
            }
        }
    }
    public void OnNormalJump()
    {
        if (isGrounded && !crouch && !IsJumping && movement)
        {
            IsJumping = true;
            if (balled) playerFX.BallJump();
            else
            {
                if (xInput != 0)
                {
                    if (inventory.CheckItem(5)) Screwing = true; //screw
                    else { Screwing = false; OnRoll = true; }//no gravity jump
                }
                else
                {
                    if (hyperJumpCharged) { onJumpingState = inHyperJumpDirection = true; }
                    else if (!hyperJumpCharged) { onJumpingState = true; gravityJump = OnRoll = false; }
                }
            }
            jumpTimeCounter = jumpTime;
        }
    }
    public void PlayerJumping(InputAction.CallbackContext context)
    {
        OnJump.Invoke();
        if (context.canceled) IsJumping = false;
        if (context.started && onRoll && CheckWallJump())
        {
            wallJumping = OnRoll = true;
            Invoke("DisableWallJump", 0.25f);
            jumpTimeCounter = jumpTime;
        }
    }
    #endregion
    private void InputX()
    {
        CancelInvoke("enableWalking");
        movingOnAir=ShootOnWalk = moveOnFloor = false;
    }
    private void InputY()
    {
        aimUp = aimDown = false;
        if (xInput != 0) aimDiagonalDown = aimDiagonal = false;
    }

#if UNITY_ANDROID
    public void PlayerJumping_Mobile(bool triggered)
    {
        if(triggered){
            OnJump.Invoke();
            if(onRoll && CheckWallJump())
        {
            wallJumping = OnRoll = true;
            Invoke("DisableWallJump", 0.25f);
            jumpTimeCounter = jumpTime;
        }
        }
        if (!triggered) IsJumping = false;
         
    }
    private void MobileMovement()
    {
        if (joystick.Horizontal < -0.25) xInput = -1;
        else if (joystick.Horizontal > 0.25) xInput = 1;
        else { xInput = 0; InputX(); }
        if (joystick.Vertical < -0.25) yInput = -1;
        else if (joystick.Vertical > 0.25) yInput = 1;
        else { yInput = 0; InputY(); }

        if (xInput == 0) InputX();
        else
        {
            crouch = false;
            if (xInput < 0) direction = Vector2.left;
            else if (xInput > 0) direction = Vector2.right;
        }
        if (yInput == 0) InputY();
        else
        {
            if (!balled && !crouch)
            {
                //if (yInput < 0f && !isGrounded) { aimDown = true; aimDiagonal = aimDiagonalDown = aimUp = false; }
                //if (yInput > 0f && xInput == 0f && !aiming) { aimUp = true; aimDiagonalDown = aimDiagonal = aimDown = false; }
                gravityJump = screwing = false;
            }
            if (isGrounded)
            {
                if (crouch)
                {
                    if (yInput > 0) crouch = false;
                    //else if (yInput < 0 && inventory.CheckItem(4)) Balled = true;
                }
                else
                {
                    if (balled) { if (yInput > 0) { crouch = true; Balled = false; } }
                    //else { if (yInput < 0) crouch = true; }
                }
            }
            else
            {
                if (balled)
                {
                    if (yInput > 0 && Physics2D.Raycast(transform.position, Vector2.up, groundDistance, groundLayer))
                    {
                        Balled = false;
                    }
                }
            }
        }
    }
    public void Diagonal_Mobile(int value){
        if (movement)
        {
            if (value > 0) { aimDiagonal = true; aimUp = aimDown = aimDiagonalDown = false; }
            else if (value < 0) { aimUp = aimDown = aimDiagonal = false; aimDiagonalDown = true; }
            aiming = true; gravityJump = screwing = ShootOnWalk = false;
            CheckAirShoot();
            CancelInvoke("DisableAimDiagonal");
            Invoke("DisableAimDiagonal",3);
        }
    }
    private void DisableAimDiagonal(){
        aiming=aimDiagonal = aimUp = aimDown = aimDiagonalDown = false;
    }
#endif
    private void CheckAirShoot()
    {
        if (!aimDown && !aimUp && !aiming && !balled)
        {
            airShoot = true;
            CancelAndInvoke("StopPlayerFire", 1f);
        }
        else airShoot = false;
    }
    #endregion
}