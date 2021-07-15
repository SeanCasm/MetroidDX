using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Interactions:MonoBehaviour
{
    [SerializeField] InputActionAsset playerInput;
    [SerializeField]ButtonUtilities buttonEssentials;
    [SerializeField]public UnityEvent backItemMenu;
    [SerializeField]Button gameVolumeSettings;
    [SerializeField] GameObject pauseSettings,pauseResume,mainSettings;
    [SerializeField] EventSystem eventSystem;
    private InputActionMap _inputActionMap;
    private InputAction back;
    private Button mainMenuFirst,itemMenuFirst,slotButton,settingsButton,pauseFirst, retryFirst,continueFirst;
    public Button SettingsFirst{get=>gameVolumeSettings;}
    public Button MenuFirst{get=>mainMenuFirst;set=>mainMenuFirst=value;}
    public Button RetryFirst
    {
        get => retryFirst;
        set
        {
            retryFirst = value;
            if (retryFirst) SetGameObjectToEventSystem(retryFirst);
        }
    }
    public void SetMainMenuSettingsFirst(){
        eventSystem.SetSelectedGameObject(mainSettings);
    }
    public void SetPauseResumeFirst(){
        eventSystem.SetSelectedGameObject(pauseResume);
    }
    public void SetPauseSettingsFirst(){
        eventSystem.SetSelectedGameObject(pauseSettings);
    }
    public void SetGameObjectToEventSystem(Button first){
        eventSystem.SetSelectedGameObject(first.gameObject);
        eventSystem.firstSelectedGameObject=first.gameObject;
        if(first.gameObject.name=="Continue"){
            continueFirst=first;
        }
    }
    private void OnEnable() {
        _inputActionMap = playerInput.FindActionMap("UI");
        back = _inputActionMap.FindAction("Back");
        back.started += Back;
    }
    private void OnDisable() {
        back.started-=Back;
    }
    public void Back(InputAction.CallbackContext context){
        if (Pause.onSlots)
        {
            if (slotButton){
                 slotButton.interactable = !slotButton.interactable;
                 MenuPointer.canMove=slotButton.interactable;
            }
            if (continueFirst)
            {
                continueFirst.gameObject.GetParent().SetActive(false);//Options panel
                SetGameObjectToEventSystem(slotButton);//Get the slot button
                continueFirst = slotButton = null;
            }
        }
        else if (Pause.onItemMenu) backItemMenu.Invoke();
    }
    public void SetGameObjectToEventSystem(Slider first)
    {
        EventSystem.current.SetSelectedGameObject(first.gameObject);
        EventSystem.current.firstSelectedGameObject = first.gameObject;
    }
    public void SetInteractableButtonSlot(Button button){
        button.interactable=!button.interactable;
        MenuPointer.canMove=button.interactable;
        slotButton=button;
    }
    public void SetInteractableButtonSettings(Button button){
        button.interactable = !button.interactable;
        if(button.interactable)settingsButton=null;
        settingsButton = button;
    }
    public void SetFirstButtonSelectOnItemMenu(){
        SetGameObjectToEventSystem(itemMenuFirst);
    }
    /// <summary>
    /// Set the navigation to all buttons in item menu UI.
    /// </summary>
    public void SetButtonNavigation(){
        var buttons=buttonEssentials.itemButtons;
        bool firsted=false;
        for(int i=0;i<buttons.Count;i++){
            Button currentButton=buttons[i];
            if (currentButton.gameObject.activeSelf)
            {
                if(!firsted){
                    //Set the reference to the first selected button.
                    itemMenuFirst=currentButton;
                    firsted = true;
                }
                Navigation navigation;
                //Set button.navigation.ondown
                for (int j = i+1; j < buttons.Count; j++)
                {
                    navigation=currentButton.navigation;
                    Button onDownButton=buttons[j];
                    if(onDownButton.gameObject.activeSelf){
                        navigation.selectOnDown=onDownButton;
                        currentButton.navigation=navigation;
                        break;
                    }
                }
                //Set button.navigation.onUp
                for(int k=i-1;k>=0;k--){
                    navigation = currentButton.navigation;
                    Button onUpButton = buttons[k];
                    if (onUpButton.gameObject.activeSelf)
                    {
                        navigation.selectOnUp = onUpButton;
                        currentButton.navigation = navigation;
                        break;
                    }
                }
            }
        }
    }
    public void OnItemMenu(bool value){
        Pause.onItemMenu=value;
    }
}
