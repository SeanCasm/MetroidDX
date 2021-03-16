using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player.Weapon;
public class IceBeam : Beam
{
    [Tooltip("When ice beam freezed a enemy play this clip.")]
    [SerializeField]AudioClip freezeClip;
    #region Unity Methods
    new private void Awake() {
        base.Awake();
    }
    new void Start()
    {
        base.Start();
    }
    new void FixedUpdate() {
       base.FixedUpdate(); 
    }
    new  void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")){
            IDamageable<float> health = other.GetComponent<IDamageable<float>>();
            IFreezeable ifreeze = other.GetComponent<IFreezeable>();
            IInvulnerable iInvulnerable = other.GetComponent<IInvulnerable>();
            if (ifreeze != null && health != null)
            {
                TryDoFreeze(health, ifreeze, iInvulnerable);
                if (!rejected) { Instantiate(impactPrefab, transform.position, Quaternion.identity, null); Destroy(gameObject); }
                else
                {
                    Reject();
                }
            }
        }else if ((other.IsTouching(floorCol) && other.tag == "Suelo"))FloorCollision();
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    #endregion
    private void TryDoFreeze(IDamageable<float> healthManager, IFreezeable ifreeze, IInvulnerable iInvulnerable)
    {
        if (!iInvulnerable.InvFreeze) { healthManager.AddDamage(damage); ifreeze.FreezeMe(); }
        else rejected = true;
    }
}
