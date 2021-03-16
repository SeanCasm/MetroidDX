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
        childAnim = GetComponentInChildren<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
        anim = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
    }
    public void PlayChargingClip()
    {
        if (!audioPlayer.isPlaying)
        {
            audioPlayer.clip = chargingClip;
            audioPlayer.loop = false;
            audioPlayer.Play();
        }
    }
    public void StopChargingClip()
    {
        audioPlayer.Stop();
        anim.SetBool("Charged", true);
        childAnim.SetBool("Charged", true);
    }

    public void PlayChargedClip()
    {
        if (!audioPlayer.isPlaying)
        {
            audioPlayer.clip = chargedClip;
            audioPlayer.loop = true;
            audioPlayer.Play();
        }
    }
}
