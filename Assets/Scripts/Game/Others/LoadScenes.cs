using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    [SerializeField]GameObject initialRoom;
    [SerializeField]EnableAllPlayer playerEnabler;
    [SerializeField]GameObject spawnPoint;
     
    /// <summary>
    /// Load async the scene 1 and unload the scene 0, only when starts new game.
    /// </summary>
    public void LoadStartScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        StartCoroutine(CheckUnload(operation));
    }
    IEnumerator CheckUnload(AsyncOperation operation)
    {
        while (!operation.isDone)
        {
            yield return null;
        }
        StartCoroutine(CheckRoomLoad());
    }
    IEnumerator CheckRoomLoad(){
        GameObject room=Instantiate(initialRoom);
        while(room==null){
            yield return new WaitForSeconds(.01f);
        }
        playerEnabler.transform.position=spawnPoint.transform.position;
        Pause.gamePaused=Pause.onSlots=false;
        GameEvents.enablePlayer.Invoke();
    }
}
