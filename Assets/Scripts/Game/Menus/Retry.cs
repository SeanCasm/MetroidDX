using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
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
    private GameObject retryMenu,retryReference;
    private Button retry,mainMenu;
    #endregion
    #region Unity Methods
    private void Awake() {
        retryMenuPrefab.LoadAssetAsync<GameObject>().Completed+=OnLoadDone;
    }
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
    private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj){
        retryReference =obj.Result;
    }
    private void EnableRetry(bool value)
    {
        playerDeath.AddListener(()=>InstantiateAndPutOnCanvas());
        playerDeath.Invoke();
    }
    private void InstantiateAndPutOnCanvas(){
        retryMenu =Instantiate(retryReference,canvas.position,Quaternion.identity,canvas);
        //Adding events to mainMenu button
        mainMenu=retryMenu.transform.GetChild(1).GetComponent<Button>();
        mainMenu.onClick.AddListener(() =>{
            Destroy(allObjectContainer);
        });
        //Adding events to retry button
        retry=retryMenu.transform.GetChild(0).GetComponent<Button>();
        retry.onClick.AddListener(() =>
        {
            GameEvents.OnRetry.Invoke();
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
        Destroy(retryMenu);
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Pause.onAnyMenu = false;
    }
}