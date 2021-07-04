using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Weapon;
public class SuperBomb : Bomb
{
    #region Unity methods
    new void Awake()
    {
        base.Awake();
    }
    new void Start()
    {
        base.Start();
    }
    new void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            health = col.GetComponent<IDamageable<float>>();
            iInvulnerable = col.GetComponent<IInvulnerable>();
            if (health != null && iInvulnerable != null)
            {
                InvokeRepeating("ReapeatingDamage", .25f, .25f);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col) {
        if (col.CompareTag("Enemy"))
        {
            health = null;
            iInvulnerable = null;
            //make exit...
        }
    }
    #endregion
    private void Explode()
    {
        animator.SetTrigger("Explode");
    }
    #region Public methods
    public new void PlayExplosion()
    {
        base.PlayExplosion();
    }
    public new void Destroy()
    {
        Destroy(gameObject);
    }

    #endregion
    private void RepeatingDamage()
    {
        TryDoDamage(damage, health, beamType, iInvulnerable);
    }
}
