using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    #region Properties
    [SerializeField]CapsuleCollider2D capsule;
    [SerializeField] BoxCollider2D floorChecker;
    [SerializeField] LayerMask groundLayer;
    [SerializeField]Transform feetPosition;
    [SerializeField]Joystick joystick;
    private SkinSwapper skin;
    private const float groundDistance = 0.18f,jumpForce=88,speed=88,speedBooster=130, 
        hyperJumpForceMultiplier=1.8f, jumpTime=0.35f,speedIncreaseOverTime=1.5f;
    private float xInput=0, yInput=0, xVelocity,yVelocity, jumpTimeCounter;
    public static Vector2 direction;
    Vector2 slopePerp;
    private Animator anim;
    private PlayerInventory inventory;
    private SomePlayerFX playerFX;
    private PlayerInstantiates instantiates;
    private bool crouch, fall, wallJumping, airShoot, movingOnAir,isJumping, runBooster,damaged,moveOnFloor,aimDiagonalDown,
       onJumpingState, charged, holdingFire, balled, shootOnWalk, isGrounded,screwing,hyperJumping,onRoll, onSlope,aimDiagonal;
    private PhysicsMaterial2D material;
    int pressCount = 0;
    int[] animatorHash = new int[27];
    public bool gravityJump { get; set; }public float currentSpeed { get; set; }
    public float currentJumpForce { get; set; }public bool hitted { get; set; }
    public bool hittedLeft { get; set; }public bool aimDown { get; set; }
    public bool OnRoll {
     set{
         onRoll=value;
         if(value && !screwing)playerFX.RollJump(true);
         else playerFX.RollJump(false);
        }
     }
    public bool speedJump { get; set; }
    public bool hyperJumpCharged { get; set; }
    public bool Damaged { get => damaged;
        set{
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
            skin.SetSpeedBooster(value);rb.isKinematic=value;
            if(value)playerFX.HyperJump();
        }
    }
    public bool canInstantiate { get; set; }
    public bool Screwing { 
        get => screwing;
        set 
        { 
            screwing = value;
            print(screwing);
            if (screwing){ skin?.SetScrewAttack(true);playerFX.ScrewAttack(true);}
            else {skin?.SetScrewAttack(false);playerFX.ScrewAttack(false); }
        }
    }
    public bool leftLook { get; set; }public bool aimUp { get; set; }
    public bool RunBooster
    {
        get => runBooster;
        set
        {
            runBooster = value;

            if (runBooster)
            {
                rb.gravityScale = 0;
                skin?.SetSpeedBooster(true);
            }
            else
            {
                currentSpeed = speed;
                skin?.SetSpeedBooster(false);
                if (isGrounded) rb.gravityScale = 3;
            }
        }
    }
    public bool IsJumping {
        get => isJumping;
        set { 
            isJumping = value;
            if (isJumping)
            {
                floorChecker.enabled = false;
                StartCoroutine(CheckFloor());
                IsGrounded =moveOnFloor = false;
            }
            material.friction = 0;
        }
    }
    IEnumerator CheckFloor()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        floorChecker.enabled = true;
    }
    public bool ShootOnWalk
    {
        get=>shootOnWalk;
        set
        {
            shootOnWalk = value;
            if(shootOnWalk)CancelAndInvoke("shootingClocking", 2f);
            else CancelInvoke("shootingClocking");
        }
    }
    public bool Balled {
        get => balled;
        set{ 
            balled = value;
            if (balled)
            {
                playerFX.Balled();
                Screwing=OnRoll=crouch =aimDiagonal = aimDown = aimUp = aimDiagonalDown = false;
            }
            else anim.SetFloat(animatorHash[23], 1);
        }
    }
    public bool movement { get; set; }
    public Rigidbody2D rb { get; set; }
    public bool IsGrounded { 
        get => isGrounded; 
        set{
            isGrounded = value;
            if (isGrounded && !runBooster)currentSpeed = speed;
            /*{
                //if(!runBooster)currentSpeed = speed;
                //airShoot=onJumpingState=screwing = OnRoll = gravityJump = fall = movingOnAir = false;
            }
            else
            {
                rb.gravityScale = 1;
                onSlope=RunBooster=ShootOnWalk = moveOnFloor = false;
            }*/
        }
    }
    #endregion
    #region Unity methods
    void Awake()
    {
        playerFX = GetComponent<SomePlayerFX>();
        instantiates = GetComponentInChildren<PlayerInstantiates>();
        inventory = GetComponent<PlayerInventory>();
        rb = GetComponent<Rigidbody2D>();
        material = rb.sharedMaterial;
        anim = GetComponent<Animator>();
        skin = GetComponent<SkinSwapper>();
    }
    void Start()
    {
        movement = canInstantiate = true;
        for (int i = 0; i < anim.parameterCount-2; i++) animatorHash[i] = Animator.StringToHash(anim.parameters[i].name);
        currentSpeed = speed; currentJumpForce = jumpForce;
    }
    void Update()
    {
        if (movement)
        {
            if (isGrounded) OnGround();
            else OnAir();
            if (xInput < 0) leftLook = true;
            else if(xInput>0)leftLook = false;

            #if UNITY_ANDROID
            MobileMovement();
            #endif
        }
    }
    void OnDisable()
    {
        ResetState();
    }
    void FixedUpdate()
    {
        if (!damaged && !hyperJumping)
        {
            xVelocity = xInput * currentSpeed * Time.deltaTime;
            yVelocity=currentSpeed * Time.deltaTime;
            if (isJumping)
            {
                if (movement || jumpTimeCounter > 0f) rb.velocity = Vector2.up * currentJumpForce * Time.deltaTime;
            }
            if (wallJumping) rb.velocity = Vector2.up* (currentJumpForce / 1.2f) * Time.deltaTime;
            if(moveOnFloor){
                if (!onSlope) rb.SetVelocity(xVelocity, 0);
                else
                {
                    if (slopeUp) rb.SetVelocity(xVelocity,yVelocity/1.2f);
                    else rb.SetVelocity(xVelocity, -yVelocity);
                }
            }else if(movingOnAir)rb.SetVelocity(xVelocity, rb.velocity.y);
        }
        else
        if (hyperJumping)rb.velocity = Vector2.up * currentJumpForce * hyperJumpForceMultiplier * Time.deltaTime;
    }
    void LateUpdate()
    {
        if(Time.timeScale>0){
            anim.SetBool(animatorHash[0], aimDiagonalDown);
            anim.SetBool(animatorHash[1], aimDiagonal);
            anim.SetBool(animatorHash[3], moveOnFloor);
            anim.SetBool(animatorHash[4], shootOnWalk || (holdingFire && isGrounded));
            anim.SetBool(animatorHash[5], crouch);
            anim.SetBool(animatorHash[6], leftLook);
            anim.SetBool(animatorHash[7], aimUp);
            anim.SetBool(animatorHash[8], isGrounded && xInput == 0);
            anim.SetBool(animatorHash[10], onRoll);
            anim.SetBool(animatorHash[11], aimDown);
            anim.SetBool(animatorHash[13], airShoot || (holdingFire && !isGrounded));
            anim.SetBool(animatorHash[14], screwing);
            anim.SetBool(animatorHash[15], hyperJumping);
            anim.SetBool(animatorHash[16], onJumpingState);
            anim.SetBool(animatorHash[17], fall);
            anim.SetBool(animatorHash[18], gravityJump);
            anim.SetBool(animatorHash[2], balled);
            anim.SetBool(animatorHash[9], isGrounded);
            anim.SetFloat(animatorHash[12], rb.velocity.y);
        }
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
        slopeUp = onSlope=moveOnFloor=false;
        if (balled) anim.SetFloat(animatorHash[23], 1);
        else if (onJumpingState && xInput != 0) currentSpeed = speed / 2;
        
        if (xInput != 0f) movingOnAir = true;
        else movingOnAir=false;
        if (!onJumpingState && !onRoll && !aimDown && !aimUp && !gravityJump && !isJumping && !damaged &&
           !screwing && !airShoot && !balled && !hyperJumping && !runBooster && !holdingFire && !charged &&!aimDiagonal &&
           !aimDiagonalDown)
        {
            fall = true; 
        }else fall=false;

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
        if (xInput != 0f)
        {
            material.friction = 0f;
            if (balled) anim.SetFloat(animatorHash[23], 1);
            else if (runBooster)
            {
                rb.gravityScale = 2;
                currentSpeed += speedIncreaseOverTime;
                if (currentSpeed >= speedBooster) { currentSpeed = speedBooster; speedJump = true; }
            }
            else rb.gravityScale = 1;
            if (yInput > 0) { aimDiagonal = true; aimUp = aimDown = aimDiagonalDown = false; }
            else if (yInput < 0) { aimDiagonalDown = true; aimUp = aimDown = aimDiagonal = false; }
            if (!moveOnFloor) Invoke("enableWalking", 0.05f);
            OnSlope();
        }
        else
        {
            if (balled) anim.SetFloat(animatorHash[23], 0);
            rb.velocity = new Vector2(0, 0);
            material.friction = 100;
        }
        if (!balled)
        {
            if (speedJump && yInput < 0) { hyperJumpCharged = true; speedJump = false; }
            else if (hyperJumpCharged && !IsInvoking("HyperJumpTimeAction") && !hyperJumping) { Invoke("HyperJumpTimeAction", 2f); }
            if ((balled ||damaged) && !runBooster) { RunBooster = hyperJumpCharged = speedJump = false; }
        }
    }
    float slopeAngle=0;bool slopeUp;
    private void OnSlope(){
        RaycastHit2D hit2D= Physics2D.Raycast(transform.position, Vector2.down, 0.3f, groundLayer);
        if(hit2D){
            slopePerp=Vector2.Perpendicular(hit2D.normal).normalized;
            slopeAngle = Vector2.Angle(hit2D.normal, Vector2.up);
            if (slopeAngle !=0){
                onSlope = true;
                if (Physics2D.Raycast(feetPosition.position, direction, 0.2f, groundLayer)) slopeUp = true;
                else slopeUp = false;
            }else slopeUp=onSlope = false;
        }
    }
    private bool CheckWallJump()
    {
        if (RayCast(Vector2.left,0.28f,groundLayer) && xInput > 0)return true;
        else if (RayCast(Vector2.right, 0.28f, groundLayer) && xInput < 0)return true;
        return false;
    }
    private bool RayCast(Vector2 vector,float distance,LayerMask layer){
        if(Physics2D.Raycast(transform.position, vector, distance, layer))return true;
        return false;
    }
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
    void HoldFire(){
        holdingFire=true;
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
        hyperJumpCharged = speedJump = false;
    }
    #endregion
    #region Public methods
    public void FalseAnyAnimStateAtGrounding(){
        airShoot = onJumpingState = Screwing = OnRoll = gravityJump = fall = movingOnAir = false;
    }
    public void FalseAnyAnimStateAtAir(){
        rb.gravityScale = 1;
        onSlope = RunBooster = ShootOnWalk = moveOnFloor = false;
    }
    public void ResetState()
    {
        StopAllCoroutines();
        CancelInvoke();
        crouch= fall= wallJumping= airShoot= movingOnAir= isJumping= runBooster=moveOnFloor=gravityJump=
        onJumpingState= charged= holdingFire= ShootOnWalk = isGrounded= Screwing= hyperJumping=balled=
        movement = canInstantiate=OnRoll=false;
        xInput=yInput=0;
        rb.velocity = Vector2.zero;
    }
    bool aiming;
    
    public void OnAim(InputAction.CallbackContext context){
        if(movement){
            float aim = context.ReadValue<float>();
            if(context.performed){
               if (aim > 0) { aimDiagonal = true; aimUp = aimDown = aimDiagonalDown = false; }
               else if (aim < 0) { aimUp = aimDown = aimDiagonal = false; aimDiagonalDown = true; }
               aiming=true;gravityJump = Screwing = ShootOnWalk=false;
               CheckAirShoot();
            }else if(context.canceled){
                if (aim == 0) aimUp = aimDown = aimDiagonal = aimDiagonalDown = false;
                aiming=false;
            }
        }
    }
    public void SelectingAmmo(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            pressCount++;
            pressCount=inventory.AmmoSelection(pressCount);
            if (pressCount == 4) pressCount = 0;
        }
    }
    public void Movement(InputAction.CallbackContext context)
    {
            xInput = context.ReadValue<Vector2>().x;
            yInput = context.ReadValue<Vector2>().y;
        if (movement)
        {
            if (xInput == 0) InputX();
            else
            {
                crouch = false;
                if (xInput < 0)direction = Vector2.left;
                else if (xInput > 0) direction = Vector2.right;
            }
            if (yInput==0)InputY();
            else{
                if (!balled && !crouch)
                {
                    if (yInput < 0f && !isGrounded) { aimDown = true; aimDiagonal = aimDiagonalDown = aimUp = false; }
                    if (yInput > 0f && xInput == 0f && !aiming) { aimUp = true; aimDiagonalDown = aimDiagonal = aimDown = false; }
                    gravityJump = Screwing =false;
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
                        if (balled){if (yInput > 0) { crouch = true; Balled = false; }}
                        else{if (yInput < 0) crouch = true;}
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
        }else{
            if (xInput == 0)InputX();
            if(yInput==0)InputY();
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
                anim.SetFloat(animatorHash[23], 1);
                Balled = true;
            }
        }
    } 
    public void Fire(InputAction.CallbackContext context)
    {
        if (context.started && canInstantiate)
        {
            if (!isGrounded) {CheckAirShoot(); OnRoll = gravityJump = Screwing = onJumpingState = false; }
            if (balled) { instantiates.SetBomb(); }
            else
            {
                if(pressCount!=3){
                    if (moveOnFloor && !onRoll && !aiming) ShootOnWalk = true;
                    GameEvents.playerFire.Invoke(false);
                }
            }
            if (inventory.CheckItem(0)) Invoke("ChargingShoot", 0.25f);//charge beam
        }
        if (context.canceled)
        {
            if (charged){GameEvents.playerFire.Invoke(true);charged = false;}
            CancelInvoke("Charged"); instantiates.Charge(false);
            CancelInvoke("ChargingShoot");holdingFire = false;
            CancelInvoke("HoldFire");
        }else{ CancelAndInvoke("HoldFire",0.5f); }
    }
    public void OnRunning(InputAction.CallbackContext context)
    {
        if (isGrounded && !balled && context.performed && movement)
        {
            if (inventory.CheckItem(8))RunBooster = true;
        }
        if (context.canceled){currentSpeed = speed; speedJump = RunBooster = false;}
    }
    public void PlayerJumping(InputAction.CallbackContext context)
    {
        if (!inventory.CheckItem(9))//gravity jump
        {
            if (context.started && isGrounded && !crouch && !IsJumping && movement)
            {
                IsJumping = true;
                if (balled)playerFX.BallJump();
                else
                {
                    if (xInput != 0)
                    {
                        if (inventory.CheckItem(5))Screwing = true; //screw
                        else OnRoll = true;//no gravity jump
                    }
                    else
                    {
                        if (hyperJumpCharged && yInput > 0) { HyperJumping = true;  }
                        else if (!hyperJumpCharged) { onJumpingState = true; gravityJump = OnRoll = false; }
                    }
                }
                jumpTimeCounter = jumpTime;
            }
        }
        else
        {
            if (xInput != 0)
            {
                if (context.started && !crouch && movement)
                {
                    airShoot=false;
                    if (inventory.CheckItem(5)) gravityJump = Screwing = true;//screw
                    else { onJumpingState = OnRoll = false; gravityJump = true; }
                    jumpTimeCounter = jumpTime;
                    IsJumping = true;
                }
            }
            else
            {
                if (hyperJumpCharged && yInput > 0) { HyperJumping = true; }
                else if (!hyperJumpCharged && isGrounded) { onJumpingState = true; gravityJump = OnRoll = false; 
                    jumpTimeCounter = jumpTime;IsJumping = true;}
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

    private void InputX(){
        CancelInvoke("enableWalking");
        ShootOnWalk = moveOnFloor = false;
    }
    private void InputY(){
        aimUp = aimDown = false;
        if (xInput != 0) aimDiagonalDown = aimDiagonal = false;
    }
    
    #if UNITY_ANDROID
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
     
    private void CheckAirShoot(){
        if(!aimDown && !aimUp && !aiming && !balled){
            airShoot=true;
            CancelAndInvoke("StopPlayerFire",1f);
        }else airShoot=false;
    }
#endregion
}