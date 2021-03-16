using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    private AudioSource audioP;
    public AudioClip ScrewEffect;
    public AudioClip ScrewEffectLoop;
    private PlayerController player;
    private bool playLoop;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        audioP = GetComponent<AudioSource>();
    }
    void Update()
    {
        AudioPlayer();
    }
    public void AudioPlayer()
    {
        if(!player.IsGrounded && player.Screwing && !audioP.isPlaying && !stop)
        {
            audioP.clip = ScrewEffect;
            audioP.Play();
            Invoke("StopAudioSound", audioP.clip.length);
        }
        if (player.IsGrounded)
        {
            audioP.Stop(); audioP.clip = ScrewEffect;
            audioP.loop = false;
        }
        if (stop)
        {
            audioP.Stop();
            audioP.clip = ScrewEffectLoop;
            audioP.loop = true;
            audioP.Play();
            stop = false;
        }
    }
    bool stop;
    void StopAudioSound()
    {
        stop = true;
    }
}
