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
    [SerializeField] UnityEvent unpauseEvent,pauseEvent,quickMinimap;
    [SerializeField]GameSettings gameSettings;
    [SerializeField] GameObject pauseMenu; 
    [SerializeField]OptionsMenu optionsMenu;
    [SerializeField] Interactions menuFirst;
    [SerializeField] MenuPointer menuPointer;
    public static System.Action<bool> touchpadPaused;
    public static System.Action<bool> OnPause;
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
    private void OnEnable() {
        GameEvents.MinimapShortcout+=QuickMinimap;
    }
    private void OnDisable() {
        GameEvents.MinimapShortcout-=QuickMinimap;
        onItemMenu=onMap=onSlots=gamePaused=onAnyMenu=onGame=onSave=false;
    }
    #endregion
    #region Public Methods
    public void PauseMenu(InputAction.CallbackContext context)
    {
        if(CheckBeforePause() && context.performed)
        {
            if (gamePaused){
                menuPointer.gameObject.SetActive(false);
                unpauseEvent.Invoke();
            }
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
    public static void PausePlayer(PlayerController playerC,bool calledfromItemOrWarp){
        PlayerController.canInstantiate=playerC.movement = false;
        playerC.rb.velocity = Vector2.zero;
        if(!calledfromItemOrWarp)OnPause?.Invoke(true);
        Time.timeScale = 0f;
    }
    public static void UnpausePlayer(PlayerController playerC){
        PlayerController.canInstantiate = playerC.movement = true;
        OnPause?.Invoke(false);
        Time.timeScale = 1f;
    }
#if UNITY_ANDROID
    public void PauseOnMiniMapTouch_Mobile(){
        enterPause = true; EnterPause(true); pauseEvent.Invoke();
    }
#endif
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
        OnPause?.Invoke(true);
        touchpadPaused?.Invoke(false);
        gameSettings.SetEffectsVolume(true);
        gameSettings.SetMusicVolume(true);
        playerC.movement = false;
        playerC.rb.velocity = Vector2.zero;
        Time.timeScale = 0f;
        gamePaused = true;
    }
    private void Unpause()
    {
        OnPause?.Invoke(false);
        GameEvents.pauseTimeCounter.Invoke(false);
        touchpadPaused?.Invoke(true);
        gameSettings.SetEffectsVolume(false);
        gameSettings.SetMusicVolume(false);
        playerC.movement = true;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        playerMenu.SetActive(false);
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
    void QuickMinimap(){
        enterPause = true;
#if UNITY_STANDALONE
        GameEvents.pauseTimeCounter.Invoke(true);
        menuFirst.SetGameObjectToEventSystem(playerMenu.GetChild(1).GetChild(3).GetComponent<Button>());
#endif
#if UNITY_ANDROID
        EnterPause(true); 
#endif
        quickMinimap.Invoke(); pauseEvent.Invoke();
    }

    void EnterPause(bool onMobile)
    {
        if(!onMobile)playerMenu.SetActive(true);
        GameEvents.pauseTimeCounter.Invoke(true);
        menuFirst.SetGameObjectToEventSystem(playerMenu.GetChild(1).GetChild(3).GetComponent<Button>());
    }
    void EscPause() {
        pauseMenu.SetActive(true);
        optionsMenu.pauseMenu=pauseMenu;

        Button resume, options;
        menuPointer.gameObject.SetActive(true);
        resume=GetComponentAtIndex(pauseMenu,1);
        menuFirst.SetPauseResumeFirst();
        menuPointer.SetCurrentMenu("pause");
        menuPointer.SetCurrentPointerPosition(0);
        resume.onClick.AddListener(()=>{
            unpauseEvent.Invoke();
            GameEvents.timeCounter.Invoke(true);//unpauses the time counter.
        });
        //Setting the first select.
        options=GetComponentAtIndex(pauseMenu, 2);

        options.onClick.AddListener(() => {
            onSubMenu = true;
        });
        
        GameEvents.timeCounter.Invoke(false);//pauses the time counter.
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