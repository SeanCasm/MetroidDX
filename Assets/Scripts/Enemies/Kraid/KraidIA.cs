using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KraidIA : Boss
{
    [SerializeField] Transform[] holes;
    [SerializeField] Animator head;
    [SerializeField] AudioClip roarClip;
    [SerializeField] GameObject holeClaw;
    private BossHealth health;
    private AudioSource audioPlayer;
    private bool onRoar;
    private int hitsCount=0;
    void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
        health = GetComponentInChildren<BossHealth>();
        audioPlayer.clip = roarClip;
    }
    new void Start()
    {
        base.Start();
        StartCoroutine(RandomHoleShoot());
    }
    private void Update() {
        if(health.Damaged){
            if(!onRoar)OnHit();
            health.Damaged=false;
        }
    }
    void OnHit(){
        hitsCount++;
        if(hitsCount==4){hitsCount = 0; onRoar=true;}
        if (onRoar)
        {
            head.SetTrigger("Damaged");
            CancelInvoke();
            Invoke("Roar", 0.8f);
            Invoke("StopRoar", 2.74f);
        }
    }
    void StopRoar(){
        onRoar=false;
    }
    IEnumerator RandomHoleShoot()
    {
        while (health.MyHealth > 0)
        {
            yield return new WaitForSeconds(2.5f);
            Instantiate(holeClaw, holes[Random.Range(0, 2)].position,Quaternion.identity);
        }
    }
     
    private void Roar()
    {
        audioPlayer.Play();
    }
}