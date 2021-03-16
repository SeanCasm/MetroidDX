using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PathCreation.Examples;
public class RidleyIA : Boss
{
    [SerializeField] Materials materials;
    [SerializeField] float speed;
    [SerializeField] Animator animHead, animNeck;
    [SerializeField] List<SpriteRenderer> ridleyAllRenderers;
    
    private PathFollower path;
    float currentSpeed,pathSpeed;
    BossHealth health;
    Rigidbody2D rb;
    bool attacking,ulti,onUltimate;
    GameObject roomCenter;
    // Start is called before the first frame update
    void Awake()
    {
        
        path = GetComponent<PathCreation.Examples.PathFollower>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponentInChildren<BossHealth>();
    }
    new void Start()
    {
        base.Start();
        pathSpeed = path.speed;
        currentSpeed = speed;
    }
    new void OnDestroy()
    {
        base.OnDestroy();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (RandomMove(0, 400) == 51 && !attacking && !ulti)
        {
            attacking = true;
            path.speed = 0;
            StartCoroutine(Attack());
        }
        if (health.MyHealth < health.TotalHealth / 3)
        {
            if(RandomMove(0, 500) == 121 && !attacking && !ulti && !onUltimate)
            {
                StopAllCoroutines();
                CancelInvoke();
                attacking = false;
                path.speed = 0;
                StartCoroutine(Ultimatum());
                onUltimate = true;
            }
        }
    }
    private void LateUpdate()
    {
        animHead.SetBool("attacking", attacking && !onUltimate);
        animNeck.SetBool("attacking", attacking && !onUltimate);
    }
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(Random.Range(1, 2));
        int timeAttacking = Random.Range(2, 3);
        Invoke("stopAttacking", timeAttacking);
    }
    void stopAttacking()
    {
        attacking = false;
        path.speed = pathSpeed;
        LoadMaterial(materials.defaultMaterial);
    }
    private void LoadMaterial(Material material)
    {
        foreach (SpriteRenderer element in ridleyAllRenderers)
        {
            element.material = material;
        }
    }
    IEnumerator Ultimatum()
    {
        LoadMaterial(materials.gold);
        health.GetComponent<Collider2D>().enabled = false;
        ulti = true;
        yield return new WaitForSeconds(2.5f);
        health.GetComponent<Collider2D>().enabled = true;
        onUltimate = ulti = false;
        Invoke("stopAttacking", 2f);
    }
    void StopAttack()
    {
        attacking = false;
    }
    int RandomMove(int minValue, int maxValue)
    {
        return Random.Range(minValue, maxValue);
    }
}
