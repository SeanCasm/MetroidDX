using System.Collections;
using System.Collections.Generic;
using UI.Controller;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuPointer : Controller
{
    [Tooltip("RectTrasform pos x and y of the main menu buttons")]
    [SerializeField] Vector2[] menuButtons;
    [Tooltip("RectTrasform pos x and y of the settings menu")]
    [SerializeField] Vector2[] settingsButtons;
    [Tooltip("RectTrasform pos x and y of the pause menu")]
    [SerializeField] Vector2[] pauseButtons;
    [SerializeField] Vector2[] controlSettingsButtons;
    [Header("Input System UI Input Module values")]
    [SerializeField] float moveRepeatDelay;
    [SerializeField] float moveRepeatRate;
    float delay;
    public static bool canMove=true;
    private bool holding,plus, minus;
    private int index=0,lenght;
    private RectTransform rect;
    Vector2[] currentMenu;
    private void Start() {
        rect=GetComponent<RectTransform>();
        currentMenu=menuButtons;
        lenght=currentMenu.Length;
    }
    void Awake()
    {
        base.moveVer=base.GetActionFromActionMap("Move");
        base.select = base.GetActionFromActionMap("Select");
    }
    protected override void OnEnable() {
        canMove=true;
        base.moveVer.started += MoveVer;
        base.moveVer.canceled+=CancelVer;
    }
    protected override void OnDisable() {
        base.moveVer.started -= MoveVer;
        base.moveVer.canceled -= CancelVer;
        StopAllCoroutines();
        index=0;
        plus = holding = minus=false;
    }
    #region UI
    public void DisablePointer(bool disable){
        if(disable)canMove=false;
        else canMove = true;
    }
    /// <summary>
    /// Sets the current menu from UI selection.
    /// </summary>
    /// <param name="name"></param>
    public void SetCurrentMenu(string name){
        switch(name){
            case "settings":
                currentMenu=settingsButtons;
                index=0;
                rect.anchoredPosition = new Vector3(currentMenu[0].x, currentMenu[0].y, 0);
                break;

            case "main":
                currentMenu=menuButtons;
                break;
            case "pause":
                currentMenu=pauseButtons;
                break;
            case "controls":
                currentMenu=controlSettingsButtons;
                break;
        }
        lenght = currentMenu.Length;

    }
    public void SetCurrentPointerPosition(int index){
        this.index = index;
        rect.anchoredPosition = new Vector3(currentMenu[index].x, currentMenu[index].y, 0);
    }
    #endregion
    protected override void MoveVer(InputAction.CallbackContext context)
    {
        if(canMove){
            StopAllCoroutines();
            float verticalAxis = context.ReadValue<Vector2>().y;
            if (verticalAxis < 0)
            {
                index++;
                holding=plus=true;
                minus=false;
                if (index > lenght - 1)
                {
                    index = 0;
                    rect.anchoredPosition = new Vector3(currentMenu[0].x, currentMenu[0].y, 0);
                }
                else
                {
                    rect.anchoredPosition = new Vector3(currentMenu[index].x, currentMenu[index].y, 0);
                }
                StartCoroutine("OnHolding");
            }
            else if (verticalAxis > 0)
            {
                index--;
                plus = false;
                holding=minus = true;
                if (index < 0)
                {
                    index = lenght - 1;
                    rect.anchoredPosition = new Vector3(currentMenu[lenght - 1].x, currentMenu[lenght - 1].y, 0);
                }
                else
                {
                    rect.anchoredPosition = new Vector3(currentMenu[index].x, currentMenu[index].y, 0);
                }
                StartCoroutine("OnHolding");
            }
        }
    }
    private void CancelVer(InputAction.CallbackContext context)
    {
        holding = false;
        StopAllCoroutines();
    }
    IEnumerator OnHolding()
    {
        delay = moveRepeatDelay;
        
        while (holding)
        {
            yield return new WaitForSecondsRealtime(delay);

            if (plus) index++;
            else if (minus) index--;
            if (index > lenght - 1){
                index = 0;
                rect.anchoredPosition = new Vector3(currentMenu[0].x, currentMenu[0].y, 0);
            }
            else if (index < 0){
                index = lenght - 1;
                rect.anchoredPosition = new Vector3(currentMenu[lenght - 1].x, currentMenu[lenght - 1].y, 0);
            }else{
                rect.anchoredPosition = new Vector3(currentMenu[index].x, currentMenu[index].y, 0);
            }
            delay = moveRepeatRate;
        }
    }
    
}