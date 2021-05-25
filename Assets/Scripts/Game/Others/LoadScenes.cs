using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    [SerializeField]AssetReference initialRoom;
    [SerializeField]EnableAllPlayer playerEnabler;
    private GameObject roomInitial;
    private void Awake() {
        initialRoom.LoadAssetAsync<GameObject>().Completed+=OnLoadDone;
    }
    private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj){
        roomInitial=obj.Result;
    }
    public void StartLoadingScene(int sceneIndex){
        SceneManager.LoadSceneAsync(sceneIndex,LoadSceneMode.Single);
    }
    /// <summary>
    /// Load async the next scene, and when his loaded the current scene is unloaded.
    /// </summary>
    /// <param name="actualScene"></param>
    /// <param name="nextScene"></param>
    public void UnloadCurrentScene(int actualScene,int nextScene){
        AsyncOperation operation=SceneManager.LoadSceneAsync(nextScene,LoadSceneMode.Additive);
        StartCoroutine(CheckUnload(operation,actualScene,nextScene));
    }
    /// <summary>
    /// Load async the scene 1 and unload the scene 0, only when starts new game.
    /// </summary>
    public void UnloadCurrentScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        StartCoroutine(CheckUnload(operation));
    }
    /// <summary>
    /// Waits to next scene be loaded in background, when it happens the current scene is unloaded.
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="actualScene">index of actual scene</param>
    /// <param name="nextScene">index of next scene</param>
    /// <returns></returns>
    IEnumerator CheckUnload(AsyncOperation operation,int actualScene,int nextScene){
        while(!operation.isDone){
            yield return null;
        }
        SceneManager.UnloadSceneAsync(actualScene);
    }
    IEnumerator CheckUnload(AsyncOperation operation)
    {
        while (!operation.isDone)
        {
            yield return null;
        }
        /*
        AsyncOperation unloadOperation=SceneManager.UnloadSceneAsync(0);
        while (!unloadOperation.isDone)
        {
            yield return null;
        }*/
        initialRoom.InstantiateAsync(null,roomInitial).Completed+=OnInstantiateDone;
    }
    private void OnInstantiateDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {
        GameEvents.enablePlayer.Invoke();

    }
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        Time.timeScale = 1f;
    }
}
