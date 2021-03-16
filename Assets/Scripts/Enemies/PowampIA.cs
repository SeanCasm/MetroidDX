using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
using Enemy;
public class PowampIA : EnemyBase
{
    [SerializeField]private LayerMask ground;
    [SerializeField]private float groundDistance,swimVelocity;
    [SerializeField]private GameObject bulletPrefab;
    private PlayerDetect pD;
    private bool facingUp;
    private GameObject[] bullets=new GameObject[8];
    private Weapon[] weaponComponent = new Weapon[8];
    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        pD = GetComponentInChildren<PlayerDetect>();
    }
    void Start()
    {
        facingUp = true;
    }
    Vector2 direction;
    private void Update()
    {
        if (facingUp)
        {
            direction = Vector2.up;
        }
        else
        {
            direction = Vector2.down;
        }

        if(Physics2D.Raycast(transform.position, direction, groundDistance, ground))
        {
            facingUp =!facingUp;
        }
    }
    private void FixedUpdate()
    {
        if (facingUp)
        {
            rigid.velocity = new Vector2(0f, swimVelocity);
        }
        else
        {
            rigid.velocity = new Vector2(0f,swimVelocity*-1f);
        }
    }
    private void LateUpdate()
    {
        if(!eh.freezed) anim.SetBool("detected", pD.detected);
    }
    public void Shoot()
    {
        int degrees = 45;
        for(int i = 0; i < bullets.Length; i++)
        {
            bullets[i] = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            weaponComponent[i] = bullets[i].GetComponent<Weapon>();
            weaponComponent[i].SetDirectionAndRotation(degrees);
            degrees += 45;
        }
    }
}
