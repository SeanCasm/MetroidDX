using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SomePlayerFX : MonoBehaviour
{
    [SerializeField]PlayerFX playerFX;
    private AudioSource audioPlayer;
    // Start is called before the first frame update
    void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
    }
    public void Steps()
    {
        int i = Random.Range(0, 3);
        audioPlayer.loop = false;
        playerFX.PlayRandomStep(audioPlayer);
    }
    public void NormalJump()
    {
        audioPlayer.loop = false;
        audioPlayer.ClipAndPlay(playerFX.Jump);
    }
    public void RollJump(bool value)
    {
        if (value)
        {
            audioPlayer.loop = true;
            audioPlayer.ClipAndPlay(playerFX.Roll);
        }
        else
        {
            audioPlayer.loop=false;
            audioPlayer.Stop();
        }
    }
    public void Balled()
    {
        audioPlayer.loop = false;
        audioPlayer.ClipAndPlay(playerFX.Morfballed);
    }
    public void BallJump()
    {
        audioPlayer.loop = false;
        audioPlayer.ClipAndPlay(playerFX.BallJump);
    }
    public void HyperJump()
    {
        audioPlayer.loop = true;
        audioPlayer.ClipAndPlay(playerFX.HyperJumpCharged);
    }
    public void ScrewAttack(bool value)
    {
        if (value)
        {
            audioPlayer.loop = false;
            audioPlayer.ClipAndPlay(playerFX.ScrewAttack);
            StartCoroutine("PlayScrewAttackLoop", playerFX.ScrewAttackLoop);
        }else audioPlayer.Stop();
    }
    IEnumerator PlayScrewAttackLoop(AudioClip clip)
    {
        yield return new WaitForSeconds(clip.length);
        audioPlayer.Stop();
        audioPlayer.loop = true;
        audioPlayer.ClipAndPlay(clip);
    }
}
