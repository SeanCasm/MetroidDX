using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Audio;
public class Pause : MonoBehaviour
{
    #region Properties
    [SerializeField] UnityEvent unpauseEvent,pauseEvent;
    [SerializeField]GameSettings gameSettings;
    [SerializeField]Transform canvas;
    [SerializeField]GameObject pauseMenuPrefab,settings,allObjectsContainer;
    [SerializeField]OptionsMenu optionsMenu;
    [SerializeField] Interactions menuFirst;
    GameObject pause;
    public static System.Action<bool> touchpadPaused;
    public static bool onItemMenu,onMap,onSlots, gamePaused, onAnyMenu, onGame,onSave;
    public GameObject player;
    public GameObject playerMenu;
    private PlayerController playerC;
    private bool enterPause,escPause;
    /// <summary>
    /// When player starts navigate on the sub-menus from esc or enter menu
    /// </summary>
    public bool onSubMenu { get; set; }
    #endregion
    #region Unity Methods
    void Awake()
    {
        playerC = player.GetComponent<PlayerController>();
    }
    private void OnDisable() {
        onItemMenu=onMap=onSlots=gamePaused=onAnyMenu=onGame=onSave=false;
    }
    #endregion
    #region Public Methods
    public void PauseMenu(InputAction.CallbackContext context)
    {
        if(CheckBeforePause() && context.performed)
        {
            if (gamePaused)unpauseEvent.Invoke();
            else{escPause = true;EscPause();pauseEvent.Invoke();}
        }
    } 
    public void Menu(InputAction.CallbackContext context)
    {
        if (CheckBeforePause() && context.performed)
        {
            if (gamePaused)unpauseEvent.Invoke();
            else{enterPause = true;EnterPause(false);pauseEvent.Invoke(); }
        }
    }
    public static void PausePlayer(PlayerController playerC){
        playerC.canInstantiate=playerC.movement = false;
        playerC.rb.velocity = Vector2.zero;
        Time.timeScale = 0f;
    }
    public static void UnpausePlayer(PlayerController playerC){
        playerC.canInstantiate = playerC.movement = true;
        Time.timeScale = 1f;
    }
    public void PauseOnMiniMapTouch_Mobile(){
        enterPause = true; EnterPause(true); pauseEvent.Invoke();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion
    #region Private Methods
    private bool CheckBeforePause(){
        if(!onAnyMenu && player.activeSelf && !enterPause && !onSubMenu && onGame)return true;
        else return false;
    }
    #region UnityEvent
    private void GeneralPause()
    {
        touchpadPaused?.Invoke(false);
        gameSettings.SetEffectsVolume(true);
        gameSettings.SetMusicVolume(true);
        playerC.movement = playerC.canInstantiate = false;
        playerC.rb.velocity = Vector2.zero;
        Time.timeScale = 0f;
        gamePaused = true;
    }
    private void Unpause()
    {
        GameEvents.pauseTimeCounter.Invoke(playerMenu.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>(), false);
        touchpadPaused?.Invoke(true);
        gameSettings.SetEffectsVolume(false);
        gameSettings.SetMusicVolume(false);
        playerC.canInstantiate = playerC.movement = true;
        Time.timeScale = 1f;
        if(pause!=null)Destroy(pause);
        else if (playerMenu.activeSelf) playerMenu.SetActive(false);
        escPause = enterPause = gamePaused = false;
    }
    /// <summary>
    /// Used in Resume button onclick event at playerMenu.
    /// </summary>
    private void UnpauseEvent()
    {
        unpauseEvent.Invoke();
    }
    #endregion
    
    void EnterPause(bool onMobile)
    {
        if(!onMobile)playerMenu.SetActive(true);
        GameEvents.pauseTimeCounter.Invoke(playerMenu.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>(),true);
        menuFirst.SetGameObjectToEventSystem(playerMenu.GetChild(1).GetChild(3).GetComponent<Button>());
    }
    void EscPause() {
         
        pause=Instantiate(pauseMenuPrefab,canvas);
        optionsMenu.pauseMenu=pause.GetChild(0);

        Button resume, options, mainMenu, quitGame;
        TMPro.TextMeshProUGUI time;

        resume=GetComponentAtIndex(pause.GetChild(0),1);
        resume.onClick.AddListener(()=>{
            unpauseEvent.Invoke();
            GameEvents.timeCounter.Invoke(true);//unpauses the time counter.
            Destroy(pause);
        });
        //Setting the first select.
        menuFirst.PauseFirst=resume;
        options=GetComponentAtIndex(pause.GetChild(0), 2);
        options.onClick.AddListener(() => {
            pause.GetChild(0).SetActive(false);
            settings.SetActive(true);
            settings.GetComponent<OptionsMenu>().fromMenuCalled = false;
            onSubMenu = true;
            menuFirst.SetGameObjectToEventSystem(menuFirst.SettingsFirst);
        });
        
        mainMenu=GetComponentAtIndex(pause.GetChild(0),3);
        mainMenu.onClick.AddListener(()=>{
            Destroy(allObjectsContainer);
        });
        
        quitGame=GetComponentAtIndex(pause.GetChild(0),4);
        quitGame.onClick.AddListener(()=>QuitGame());
        
        GameEvents.timeCounter.Invoke(false);//pauses the time counter.
        time=pause.GetChild(0).GetChild(5).GetComponent<TMPro.TextMeshProUGUI>();
        time.text="Time:"+TimeCounter.CurrentTime;
    }
    private Button GetComponentAtIndex(GameObject someObject,int index){
        return someObject.transform.GetChild(index).GetComponent<Button>();
    }
     #endregion
     private void OnDestroy() {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0);
        Time.timeScale=1f;
    }
}