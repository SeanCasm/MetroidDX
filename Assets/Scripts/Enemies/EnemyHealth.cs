using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health<float>,IDamageable<float>,IFreezeable,IInvulnerable
{
    [SerializeField] bool invMissiles, invSuperMissiles, invBeams, invBombs, invSuperBombs, invFreeze,invPlasma;
    [SerializeField] Materials materials;
    [SerializeField]GameObject deathClip,freezedCol;
    [SerializeField]Collider2D rigidCol;
    private float totalHealth;
    public int collideDamage;
    public GameObject deadPrefab;
    private Behaviour[] components;
    private Collider2D box;
    public bool freezed { get;set; }
    public bool InvPlasma=>invPlasma;
    public bool InvMissiles => invMissiles;public bool InvSuperMissiles => invSuperMissiles;
    public bool InvBeams => invBeams;public bool InvBombs => invBombs;
    public bool InvSuperBombs => invSuperBombs;public bool InvFreeze => invFreeze;

    #region Unity methods
    private void Awake()
    {
        anim = GetComponentInParent<Animator>();
        _renderer = GetComponentInParent<SpriteRenderer>();
        rb2d = GetComponentInParent<Rigidbody2D>();
        box=GetComponent<Collider2D>();
    }
    void Start()
    {
        totalHealth = health;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && !freezed){
            GameEvents.damagePlayer.Invoke(collideDamage,transform.position.x);
        }
    }
    #endregion
    #region Public methods
    public void FreezeMe()
    {
        CancelInvoke("Unfreeze");
        StopAllCoroutines();
        rigidCol.enabled=false;
        freezedCol.SetActive(true);freezed = true;
        Invoke("Unfreeze",4f);
        _renderer.material = materials.freeze;
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        components = transform.parent.gameObject.GetComponents<Behaviour>();
        Utilities.SetBehaviours(components, false);
        Physics2D.IgnoreLayerCollision(8, 9, false);
        StartCoroutine(FreezeVisualFeedBack());     
    }
     
    public void Unfreeze()
    {
        rigidCol.enabled = true;
        freezedCol.SetActive(false); box.enabled=false;
        Utilities.SetBehaviours(components, true);
        _renderer.material = materials.defaultMaterial;
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        Physics2D.IgnoreLayerCollision(8, 9, true);
        freezed = false;
        box.enabled = true;
    }
    public void AddDamage(float amount)
    {
        if(!freezed){
            health-=amount;
            StartCoroutine(VisualFeedBack());
            if (health <= 0)
            {
                if (deathClip != null && deadPrefab != null)
                {
                    GameEvents.drop.Invoke(transform.position);
                    gameObject.GetParent().SetActive(false);
                    Instantiate(deathClip);
                    Instantiate(deadPrefab, transform.position, Quaternion.identity);
                }
                health = 0;
                Destroy(gameObject.GetParent());
            }
        } 
    }
    #endregion
    #region Private methods
     
    private IEnumerator VisualFeedBack()
    {
        _renderer.color = Color.red;
        yield return new WaitForSecondsRealtime(0.1f);
        _renderer.color = Color.white;
    }
    public IEnumerator FreezeVisualFeedBack()
    {
        _renderer.color=_renderer.color.Default();
        yield return new WaitForSeconds(2f);
        while (freezed)
        {
            for (float i = 1; i >= 0.5f; i -= .5f)
            {
                Color color = _renderer.color;
                _renderer.color = color.SetColorRGB(i);
                yield return new WaitForSeconds(0.05f);
            }
            for (float i = 0.5f; i <= 1; i += .5f)
            {
                Color color = _renderer.color;
                _renderer.color = color.SetColorRGB(i);
                yield return new WaitForSeconds(0.05f);
            }
        }
    }
    #endregion
}
