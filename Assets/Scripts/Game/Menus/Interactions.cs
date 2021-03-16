using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Interactions:MonoBehaviour
{
    [SerializeField]ButtonUtilities buttonEssentials;
    [SerializeField]UnityEvent backItemMenu,backMap;
    [SerializeField]Button gameVolumeSettings,optionsMainMenu;
    
    private Button settingsFirst,mainMenuFirst,itemMenuFirst,slotButton,settingsButton,pauseFirst, retryFirst,continueFirst;
    public Button SettingsFirst{get=>gameVolumeSettings;}
    public Button OptionMainMenu{get=>optionsMainMenu;}
    public Button MenuFirst{get=>mainMenuFirst;set=>mainMenuFirst=value;}
    public Button PauseFirst 
    { 
        get => pauseFirst;
        set
        {
            pauseFirst = value;
            if (pauseFirst) SetGameObjectToEventSystem(pauseFirst);
        }
    }
    public Button RetryFirst
    {
        get => retryFirst;
        set
        {
            retryFirst = value;
            if (retryFirst) SetGameObjectToEventSystem(retryFirst);
        }
    }
    public void BackPressed(){
        if (Pause.onSlots)
        {
            if(slotButton)slotButton.interactable = !slotButton.interactable;
            if (continueFirst){
                continueFirst.gameObject.GetParent().SetActive(false);//Options panel
                SetGameObjectToEventSystem(slotButton);//Get the slot button
                continueFirst=slotButton=null;
            }
        }
        else if (Pause.onItemMenu)backItemMenu.Invoke();
        else if (Pause.onMap)backMap.Invoke();
    }
    public void SetGameObjectToEventSystem(Button first){
        EventSystem.current.SetSelectedGameObject(first.gameObject);
        EventSystem.current.firstSelectedGameObject=first.gameObject;
        if(first.gameObject.name=="Continue"){
            continueFirst=first;
        }
    }
    public void SetGameObjectToEventSystem(Slider first)
    {
        EventSystem.current.SetSelectedGameObject(first.gameObject);
        EventSystem.current.firstSelectedGameObject = first.gameObject;
    }
    public void SetInteractableButtonSlot(Button button){
        button.interactable=!button.interactable;
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
    public void OnMap(bool value){
        Pause.onMap=value;
    }
}
