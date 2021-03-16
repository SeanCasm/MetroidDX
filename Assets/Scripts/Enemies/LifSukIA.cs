using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
public class LifSukIA : EnemyBase
{
    #region Properties
    [SerializeField]int lifeStole;
    [SerializeField]CircleCollider2D stickPlayer;
    private float currentSpeed;
    private bool jump, onPlayer,jumpAgain;
    private GroundChecker efd;
    private Transform player;
    private PlayerHealth playerH;
    private PlayerController playerC;
    #endregion
    #region Unity Methods
    new void Awake()
    {
        base.Awake();
        efd = GetComponent<GroundChecker>();
    }
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = speed;
    }
    private void Update()
    {
        if (!onPlayer)
        {
            if (pDetect.detected && efd.OnGround && !jump)
            {
                if(player!=null){
                    if (player.GetX() > transform.position.x && !efd.FacingRight)
                    {
                        efd.Flip();
                    }
                    if (player.GetX() < transform.position.x && efd.FacingRight)
                    {
                        efd.Flip();
                    }
                }
                jump = true;
                if(jumpAgain){
                    anim.SetTrigger("Jump");
                }
            }
        }
        else
        {
            transform.position=playerC.transform.position;
            if (playerC.leftLook) transform.eulerAngles=new Vector3(0,180,0);
            else transform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
    private void FixedUpdate()
    {
        if (efd.OnGround) efd.SetOnGroundVelocity(currentSpeed);
        else rigid.SetVelocity(0,0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.IsTouching(stickPlayer))
            {
                playerC = collision.GetComponentInParent<PlayerController>();
                playerH = collision.GetComponentInParent<PlayerHealth>();
                onPlayer = true;
                transform.parent = collision.transform.parent;
                StartCoroutine(ConstantDamage());
            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.IsTouching(pDetect.detector))
        {
            player = other.transform.parent;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Suelo")
        {
            efd.enabled = true;
            jump = false;
            Invoke("JumpAgain",1.5f);
        }
    }
    private void LateUpdate()
    {
        anim.SetBool("OnPlayer", onPlayer);
        anim.SetBool("OnGround", efd.OnGround);
    }
    #endregion
    void JumpAgain(){
        jumpAgain=true;
    }
    IEnumerator ConstantDamage()
    {
        while(onPlayer){
            playerH.ConstantDamage(lifeStole);
            yield return new WaitForSeconds(0.2f);
        }
    }
    #region Public Methods
    public void SetOnGroundVelocity(float amount)
    {
        if (!efd.OnSlope)
        {
            if (efd.FacingRight) rigid.velocity = new Vector2(amount, rigid.velocity.y);
            else rigid.velocity = new Vector2(-amount, rigid.velocity.y);
        }
        else
        {
            if (efd.FacingRight) rigid.velocity = new Vector2(amount * efd.SlopePerp.x * -1, amount * efd.SlopePerp.y * -1);
            else rigid.velocity = new Vector2(-amount * efd.SlopePerp.x * -1, amount * efd.SlopePerp.y);
        }
    }
    public void OnRotation()
    {
        currentSpeed*=transform.right.x;
    }
    #region Animation Event Methods
    public void SetJumpState(){
        anim.SetBool("CanGround", false);
        rigid.bodyType=RigidbodyType2D.Static;
        jumpAgain=efd.enabled=false;
    }
    public void CanGround(){
        anim.SetBool("CanGround",true);
        rigid.bodyType = RigidbodyType2D.Dynamic;
    }
    #endregion
    #endregion
}