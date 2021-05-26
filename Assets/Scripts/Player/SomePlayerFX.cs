using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class SomePlayerFX : MonoBehaviour
{
    [SerializeField] AssetReference morphJumpRef,normalJump,morphballed,screwAttack,screwAttackLoop,stepRef,rollJumpLoop,hyperJumpRef;
    [SerializeField] AudioClip[] playerSteps;
    
 
    private GameObject morphJump,nJump,balled,screw,screwLoop,steps,rollLoop,hyperJump;
    private AudioSource morphjumpAS,nJumpAS,balledAS,screwAS,screwLoopAS,stepsAS,rLoopAS,hyperJumpAS;
    private int totalSteps;
    void Awake()
    {
        morphJumpRef.LoadAssetAsync<GameObject>().Completed+=OnLoadDone;
        normalJump.LoadAssetAsync<GameObject>().Completed+=OnLoadDone;
        morphballed.LoadAssetAsync<GameObject>().Completed+=OnLoadDone;
        screwAttack.LoadAssetAsync<GameObject>().Completed+=OnLoadDone;
        screwAttackLoop.LoadAssetAsync<GameObject>().Completed += OnLoadDone;
        stepRef.LoadAssetAsync<GameObject>().Completed += OnLoadDone;
        rollJumpLoop.LoadAssetAsync<GameObject>().Completed+=OnLoadDone;
        hyperJumpRef.LoadAssetAsync<GameObject>().Completed += OnLoadDone;
        totalSteps=playerSteps.Length;
    }
     
    void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj){
        switch(obj.Result.name){
            case "MorphJump":
                morphJump = Instantiate(obj.Result, transform.position, Quaternion.identity, transform);
                morphjumpAS = morphJump.GetComponent<AudioSource>();
                morphJump.SetActive(false);
            break;
            case "NormalJump":
                nJump = Instantiate(obj.Result, transform.position, Quaternion.identity, transform);
                nJumpAS = nJump.GetComponent<AudioSource>();
                nJump.SetActive(false);
            break;
            case "Balled":
                balled = Instantiate(obj.Result, transform.position, Quaternion.identity, transform);
                balledAS = balled.GetComponent<AudioSource>();
                balled.SetActive(false);
            break;
            case "Screw":
                screw =Instantiate(obj.Result,transform.position,Quaternion.identity,transform);
                screwAS=screw.GetComponent<AudioSource>();
                screw.SetActive(false);
            break;
            case "ScrewLoop":
                screwLoop = Instantiate(obj.Result, transform.position, Quaternion.identity, transform);
                screwLoopAS = screw.GetComponent<AudioSource>();
                screwLoop.SetActive(false);
                break;
            case "Steps":
                steps=Instantiate(obj.Result,transform.position,Quaternion.identity,transform);
                stepsAS=steps.GetComponent<AudioSource>();
                steps.SetActive(false);
            break;
            case "RollJump":
                rollLoop = Instantiate(obj.Result, transform.position, Quaternion.identity, transform);
                rLoopAS = rollLoop.GetComponent<AudioSource>();
                rollLoop.SetActive(false);
            break;
            case "HyperJump":
                hyperJump = Instantiate(obj.Result, transform.position, Quaternion.identity, transform);
                hyperJumpAS = hyperJump.GetComponent<AudioSource>();
                hyperJump.SetActive(false);
                break;
        }
    }
    /// <summary>
    /// Play the step sounds in player animation events.
    /// </summary>
    public void Steps()
    {
        StopAllCoroutines();
        int i=Random.Range(0,totalSteps);
        stepsAS.clip=playerSteps[i];
        steps.SetActive(true);
        stepsAS.Play();
    }
    /// <summary>
    /// Play the normal jump sound in player animation events.
    /// </summary>
    public void NormalJump()
    {
        StopAllCoroutines();
        nJump.SetActive(true);
        StartCoroutine(DeactiveGameObject(nJumpAS));
    }
    public void RollJump(bool value)
    {
        StopAllCoroutines();
        if(value){
            rollLoop.SetActive(true);
            StartCoroutine(DeactiveGameObject(rLoopAS));
        }else{
            rollLoop.SetActive(false);
        }
         
    }
    public void Balled()
    {
        StopAllCoroutines();
        balled.SetActive(true);
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
        hyperJump.SetActive(true);
        StartCoroutine(DeactiveGameObject(hyperJumpAS));
    }
    public void ScrewAttack(bool value)
    {
        StopAllCoroutines();
        if(value){
            screw.SetActive(true);
            screwAS.Play();
            StartCoroutine(DeactiveGameObjectInLoop(screwAS, screwLoopAS));
        }else{
            screw.SetActive(false);
        }
    }
    public void StopLoopClips(){
        rollLoop.SetActive(false);screwLoop.SetActive(false);hyperJump.SetActive(false);
    }
}
