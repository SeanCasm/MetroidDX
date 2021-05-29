using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : Health<float>, IDamageable<float>,IInvulnerable
{
    [SerializeField] bool invMissiles, invSuperMissiles, invBeams, invBombs, invSuperBombs, invFreeze,invPlasma;
    [SerializeField] Materials materials;
    [SerializeField] SpriteRenderer[] sprites;
    [SerializeField] Colors bossColors;
    [SerializeField] private ScriptableDrop drop;
    private Color currentHealthColor;
    private Boss boss;
    public bool Damaged{get;set;}
    public bool onInvulnerable{get;set;}
    public bool InvPlasma=>invPlasma;
    private float totalHealth;
    public float TotalHealth{get=>totalHealth;}

    public bool InvMissiles => invMissiles;public bool InvSuperMissiles => invSuperMissiles;

    public bool InvBeams => invBeams;public bool InvBombs => invBombs;

    public bool InvSuperBombs => invSuperBombs;public bool InvFreeze => invFreeze;
    private bool quarterReached,halfReached,lowReached;
    private void Awake(){
        //anim = GetComponentInParent<Animator>();
        //_renderer = GetComponentInParent<SpriteRenderer>();
        boss=GetComponentInParent<Boss>();
        rb2d = GetComponentInParent<Rigidbody2D>();
        totalHealth=health;
        currentHealthColor=Color.white;
    }
    public void AddDamage(float damage)
    {
        if(!onInvulnerable){
            StartCoroutine(VisualFeedBack());
            health-=damage;
            Damaged=true;
            CheckHealthState();
            if(health<=0){
                OnDeath();
                Destroy(gameObject.GetParent());
            }
        }
    }
    private IEnumerator VisualFeedBack()
    {
        foreach(SpriteRenderer element in sprites){
            element.color=Color.red;
        }
        yield return new WaitForSecondsRealtime(0.1f);
        foreach (SpriteRenderer element in sprites)
        {
            element.color = currentHealthColor;
        }
    }
    private void CheckHealthState()
    {
        if (health <= totalHealth * 3 / 4 && !quarterReached)
        {
            SetHealthColor(bossColors.bossQuarterHealth);
            quarterReached = true;
        }
        else
        if (health <= totalHealth * 2 / 4 && !halfReached)
        {
            SetHealthColor(bossColors.bossHalfHealth);
            halfReached = true;
        }
        else
        if (health <= totalHealth * 1 / 4 && !lowReached)
        {
            SetHealthColor(bossColors.bossLowHealth);
            lowReached = true;
        }
    }
    private void OnDeath()
    {
        Instantiate(drop.reloadAll, transform.position, Quaternion.identity, null);
        boss.OnDeath();
    }
    private void SetHealthColor(Color color)
    {
        foreach (SpriteRenderer element in sprites)
        {
            element.color = color;
        }
        currentHealthColor=color;
    }
}
