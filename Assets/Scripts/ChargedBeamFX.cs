using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChargedBeamFX : MonoBehaviour
{
    public AudioClip chargingClip,chargedClip;
    private AudioSource audioPlayer;
    private Animator anim,childAnim;
    
    void OnDisable()
    {
        anim.SetBool("Charged", false);
        childAnim.SetBool("Charged", false);
    }
    void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        childAnim = GetComponentInChildren<Animator>();
    }
    public void PlayChargingClip()
    {
        audioPlayer.clip = chargingClip;
        audioPlayer.loop = false;
        audioPlayer.Play();
    }
    public void StopChargingClip()
    {
        audioPlayer.Stop();
        anim.SetBool("Charged", true);
        childAnim.SetBool("Charged", true);
    }

    public void PlayChargedClip()
    {
        audioPlayer.clip = chargedClip;
        audioPlayer.loop = true;
        audioPlayer.Play();
    }
}
