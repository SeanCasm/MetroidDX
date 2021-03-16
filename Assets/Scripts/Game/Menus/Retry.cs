using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class Retry : MonoBehaviour
{
    #region Properties
    
    [SerializeField]Transform canvas;
    [SerializeField] UnityEvent playerDeath;
    [SerializeField]private SaveAndLoad saveLoad;
    [SerializeField]private GameObject player,retryMenuPrefab,hudMenu,allObjectContainer;
    [SerializeField] Interactions menuFirst;
    private GameObject retryMenu;
    private Button retry,mainMenu;
    #endregion
    #region Unity Methods
    private void OnEnable()
    {
        GameEvents.retry += EnableRetry;
    }
    private void OnDisable()
    {
        GameEvents.retry -= EnableRetry;
    }
    #endregion
    #region Private Methods
    private void EnableRetry(bool value)
    {
        playerDeath.AddListener(()=>InstantiateAndPutOnCanvas());
        playerDeath.Invoke();
    }
    private void InstantiateAndPutOnCanvas(){
        retryMenu =Instantiate(retryMenuPrefab,canvas.position,Quaternion.identity,canvas);
        //Adding events to mainMenu button
        mainMenu=retryMenu.transform.GetChild(1).GetComponent<Button>();
        mainMenu.onClick.AddListener(()=>canvas.GetComponent<LoadScenes>().LoadScene(0));
        mainMenu.onClick.AddListener(() => Destroy(allObjectContainer));
        //Adding events to retry button
        /*retry=retryMenu.transform.GetChild(0).GetComponent<Button>();
        retry.onClick.AddListener(() => RetryGame());
        retry.onClick.AddListener(() =>player.SetActive(true));
        retry.onClick.AddListener(()=>hudMenu.SetActive(true));
        retry.onClick.AddListener(()=>Destroy(retryMenu));*/
        //Setting the first select.
        menuFirst.SetGameObjectToEventSystem(mainMenu);
    }
    #endregion    
    public void RetryGame()
    {
        int slot = SaveAndLoad.slot;
        GameData data = SaveSystem.LoadPlayerSlot(slot);
        if (data != null) saveLoad.LoadPlayerSlot(slot);
        else VoidData();
        DesactivePause();
    }
    private void VoidData()
    {
        SceneManager.LoadScene(1);
        AudioListener.pause = false;
    }
    public void DesactivePause()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Pause.onAnyMenu = false;
    }
}