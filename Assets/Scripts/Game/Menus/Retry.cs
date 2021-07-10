﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
/// <summary>
/// This class represents the retry screen when player is dead.
/// </summary>
public class Retry : MonoBehaviour
{
    #region Properties
    
    [SerializeField]Transform canvas;
    [SerializeField] UnityEvent playerDeath;
    [SerializeField]private SaveAndLoad saveLoad;
    [SerializeField]private GameObject allObjectContainer;
    [SerializeField]AssetReference retryMenuPrefab;
    [SerializeField] Interactions menuFirst;
    [SerializeField]GameObject hud,player;
    [SerializeField] LoadScenes sceneLoader;
    private GameObject retryMenu,retryReference;
    private Button retry,mainMenu;
    public static System.Action Start;
    public static System.Action Completed;
    public static System.Action Selected;
    #endregion
    #region Unity Methods
    private void Awake() {
        retryMenuPrefab.LoadAssetAsync<GameObject>().Completed+=OnLoadDone;
    }
    private void OnEnable()
    {
        Completed += EnableRetry;
    }
    private void OnDisable()
    {
        Completed -= EnableRetry;
    }
    #endregion
    #region Private Methods
    private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj){
        retryReference =obj.Result;
    }
    private void EnableRetry()
    {
        playerDeath.AddListener(()=>InstantiateAndPutOnCanvas());
        playerDeath.Invoke();
    }
    private void InstantiateAndPutOnCanvas(){
        retryMenu =Instantiate(retryReference,canvas.position,Quaternion.identity,canvas);
        //Adding events to mainMenu button
        mainMenu=retryMenu.transform.GetChild(1).GetComponent<Button>();
        mainMenu.onClick.AddListener(() =>{
            PlayerHealth.isDead = false;
            Destroy(allObjectContainer);
        });
        //Adding events to retry button
        retry=retryMenu.transform.GetChild(0).GetComponent<Button>();
        retry.onClick.AddListener(() =>
        {
            Selected.Invoke();
            player.SetActive(true);
            hud.SetActive(true);
            RetryGame();
        });
      
        //Setting the first select.
        menuFirst.SetGameObjectToEventSystem(mainMenu);
    }
    #endregion    
    public void RetryGame()
    {
        int slot = SaveAndLoad.slot;
        PlayerHealth.isDead=false;
        GameData data = SaveSystem.LoadPlayerSlot(slot);
        if (data != null) saveLoad.LoadPlayerSlot(slot);
        else VoidData();
        DesactivePause();
    }
    private void VoidData()
    {
        AudioListener.pause = false;
        SaveAndLoad.newGame = true;
        sceneLoader.LoadStartScene();
    }
    public void DesactivePause()
    {
        Destroy(retryMenu);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Pause.onAnyMenu = false;
    }
}