using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy.Weapons;
public class KraidCloneIA : MonoBehaviour
{
    [SerializeField] Transform[] holes;
    [SerializeField] Animator head, feets;
    [SerializeField] AudioClip roarClip;
    [SerializeField] GameObject holeClaw;
    private BossHealth health;
    private AudioSource audioPlayer;
    private bool onRoar;
    private int hitsCount = 0;
    void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
        health = GetComponentInChildren<BossHealth>();
        audioPlayer.clip = roarClip;
    }
    void Start()
    {
        StartCoroutine(RandomHoleShoot());
    }
    private void Update()
    {
        if (health.Damaged)
        {
            if (!onRoar) OnHit();
            health.Damaged = false;
        }
    }
    void OnHit()
    {
        hitsCount++;
        if (hitsCount == 4) { hitsCount = 0; onRoar = true; }
        if (onRoar)
        {
            head.SetTrigger("Damaged");
            CancelInvoke();
            Invoke("Roar", 0.8f);
            Invoke("StopRoar", 2.74f);
        }
    }
    IEnumerator RandomHoleShoot()
    {
        while (health.MyHealth > 0)
        {
            yield return new WaitForSeconds(1.9f);
            GameObject claw=Instantiate(holeClaw, holes[Random.Range(0, 2)].position, Quaternion.identity);
            claw.transform.Rotate(0,transform.eulerAngles.y,0);
            if(transform.eulerAngles.y==180){
                claw.GetComponent<Weapon>().Direction = new Vector2(-1,0);
            }
        }
    }
    private void Roar()
    {
        audioPlayer.Play();
    }
}
