using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SomePlayerFX : MonoBehaviour
{
    [SerializeField] GameObject morphJump,normalJump,morphballed,screwAttack,screwAttackLoop,stepRef,rollJumpLoop,hyperJumpRef;
    [SerializeField] AudioClip[] playerSteps;
    private AudioSource morphjumpAS,nJumpAS,balledAS,screwAS,screwLoopAS,stepsAS,rLoopAS,hyperJumpAS;
    private int totalSteps;
    void Start()
    {
        morphjumpAS = morphJump.GetComponent<AudioSource>();
        nJumpAS = normalJump.GetComponent<AudioSource>();
        balledAS = morphballed.GetComponent<AudioSource>();
        screwAS = screwAttack.GetComponent<AudioSource>();
        screwLoopAS = screwAttackLoop.GetComponent<AudioSource>();
        stepsAS = stepRef.GetComponent<AudioSource>();
        rLoopAS = rollJumpLoop.GetComponent<AudioSource>();
        hyperJumpAS = hyperJumpRef.GetComponent<AudioSource>();
        totalSteps=playerSteps.Length;
    }
    /// <summary>
    /// Play the step sounds in player animation events.
    /// </summary>
    public void Steps()
    {
        StopAllCoroutines();
        int i=Random.Range(0,totalSteps);
        stepsAS.clip=playerSteps[i];
        stepRef.SetActive(true);
        stepsAS.Play();
    }
    /// <summary>
    /// Play the normal jump sound in player animation events.
    /// </summary>
    public void NormalJump()
    {
        StopAllCoroutines();
        normalJump.SetActive(true);
        StartCoroutine(DeactiveGameObject(nJumpAS));
    }
    public void RollJump(bool value)
    {
        StopAllCoroutines();
        if(value){
            rollJumpLoop.SetActive(true);
            StartCoroutine(DeactiveGameObject(rLoopAS));
        }else{
            rollJumpLoop.SetActive(false);
        }
         
    }
    public void Balled()
    {
        StopAllCoroutines();
        morphballed.SetActive(true);
        StartCoroutine(DeactiveGameObject(balledAS));
    }
    public void BallJump()
    {
        StopAllCoroutines();
        morphJump.SetActive(true);
        StartCoroutine(DeactiveGameObject(morphjumpAS));
    }
    IEnumerator DeactiveGameObject(AudioSource audio){
        audio.Play();
        while(audio.isPlaying){
            yield return null;
        }
        audio.gameObject.SetActive(false);
    }
    IEnumerator DeactiveGameObjectInLoop(AudioSource audio,AudioSource loop)
    {
        while (audio.isPlaying)
        {
            yield return null;
        }
        audio.gameObject.SetActive(false);
        loop.gameObject.SetActive(true);
        loop.Play();
        while(loop.isPlaying){
            yield return null;
        }
        loop.gameObject.SetActive(false);
    }
    public void HyperJump()
    {
        StopAllCoroutines();
        hyperJumpRef.SetActive(true);
        StartCoroutine(DeactiveGameObject(hyperJumpAS));
    }
    public void ScrewAttack(bool value)
    {
        StopAllCoroutines();
        if(value){
            screwAttack.SetActive(true);
            screwAS.Play();
            StartCoroutine(DeactiveGameObjectInLoop(screwAS, screwLoopAS));
        }else{
            screwAttack.SetActive(false);
        }
    }
    public void StopLoopClips(){
        rollJumpLoop.SetActive(false);screwAttackLoop.SetActive(false);hyperJumpRef.SetActive(false);
    }
}
