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
    Transform player;
    private PathFollower path;
    float currentSpeed,pathSpeed;
    BossHealth health;
    bool attacking,ulti,onUltimate;
    void Awake()
    {
        path = GetComponent<PathCreation.Examples.PathFollower>();
        health = GetComponentInChildren<BossHealth>();
        player=References.Player.transform;
    }
    new void Start()
    {
        base.Start();
        pathSpeed = path.speed;
        currentSpeed = speed;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.x>transform.position.x)transform.localScale=new Vector3(1,1);
        else transform.localScale = new Vector3(-1, 1);
        if (RandomMove(0, 400) == 51 && !attacking && !ulti)
        {
            attacking = true;
            path.speed = 0;
            StopAllCoroutines();
            StartCoroutine(Attack());
        }
        if (health.MyHealth < health.TotalHealth / 3)
        {
            if(RandomMove(0, 300) == 51 && !attacking && !ulti && !onUltimate)
            {
                CancelInvoke();
                attacking = false;
                path.speed = 0;
                StopAllCoroutines();
                StartCoroutine(Ultimatum());
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
        onUltimate = true;
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
