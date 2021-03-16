using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : Fluid
{
    [SerializeField]float damageCall;
    [SerializeField]int damage;
    private AudioSource audioPlayer;
    private void Awake() {
        audioPlayer=GetComponent<AudioSource>();
    }
    new void OnTriggerEnter2D(Collider2D col)
    {
        base.OnTriggerEnter2D(col);
        if(col.CompareTag("Player") && col.IsTouching(waterCollider)){
            playerH=col.GetComponentInParent<PlayerHealth>();
            StartCoroutine(ConstantDamage());
        }
    }
    new void OnTriggerStay2D(Collider2D col)
    {
        base.OnTriggerStay2D(col);
    }
    new void OnTriggerExit2D(Collider2D col)
    {
        base.OnTriggerExit2D(col);
        if (col.CompareTag("Player"))
        {
            StopAllCoroutines();
            playerH=null;
            audioPlayer.Stop();
        }
    }
    IEnumerator ConstantDamage()
    {
        audioPlayer.Play();
        while (playerH!=null)
        {
            playerH.SetConstantDamage(damage);
            yield return new WaitForSeconds(damageCall);
        }
    }
}
