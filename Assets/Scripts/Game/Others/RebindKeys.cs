using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using Game.Device;
public class RebindKeys : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField]InputActionReference fire,jump,movement,aim,pause,playerPause,submit,
    back,ball,running,selectItems,menuMove;
    public static Device deviceType;
    private Text keyText,UIText;
    private string actualInput;
    //Use for set a reference to Player or Menu/action
    public InputActionReference actionReference{get;set;}
    //Use for set a reference to Menu/action, when action composites are the same
    private InputActionReference actionReference2; 
    public InputActionReference ActionReference2{
        set{
            actionReference2=value;
            playerMenuInput=true;
    }}
    private const string text="Waiting for input...";
    private string uiText;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    private bool playerMenuInput;
    public int compositeIndex{get;set;}=0;
    private void Awake() 
    {
        GameObject[] childs=gameObject.GetChilds(14);
        if(deviceType==Device.Keyboard){
            foreach (GameObject element in childs)
            {
                switch (element.name)
                {
                    case "Fire":
                        GetText(element).text = BindToText(fire);
                        break;
                    case "Jump":
                        GetText(element).text = BindToText(jump);
                        break;
                    case "Left":
                        GetText(element).text = BindToText(movement, 3);
                        break;
                    case "Up":
                        GetText(element).text = BindToText(movement, 1);
                        break;
                    case "Right":
                        GetText(element).text = BindToText(movement, 4);
                        break;
                    case "Down":
                        GetText(element).text = BindToText(movement, 2);
                        break;
                    case "Insta morfball":
                        GetText(element).text = BindToText(ball);
                        break;
                    case "Ammo selection":
                        GetText(element).text = BindToText(selectItems);
                        break;
                    case "Pause":
                        GetText(element).text = BindToText(pause);
                        break;
                    case "Player pause":
                        GetText(element).text = BindToText(playerPause);
                        break;
                    case "Diagonal up":
                        GetText(element).text = BindToText(aim, 1);
                        break;
                    case "Diagonal down":
                        GetText(element).text = BindToText(aim, 2);
                        break;
                    case "Back":
                        GetText(element).text = BindToText(back);
                        break;
                    case "Select":
                        GetText(element).text = BindToText(submit);
                        break;
                    case "Speed booster":
                        GetText(element).text = BindToText(running);
                        break;
                }
            }
        }else if(deviceType==Device.Gamepad){
            foreach (GameObject element in childs)
            {
                switch (element.name)
                {
                    case "Fire":
                        GetText(element).text = BindToText(fire,1);
                        break;
                    case "Jump":
                        GetText(element).text = BindToText(jump,1);
                        break;
                    case "Left":
                        GetText(element).text = BindToText(movement, 8);
                        break;
                    case "Up":
                        GetText(element).text = BindToText(movement, 6);
                        break;
                    case "Right":
                        GetText(element).text = BindToText(movement, 9);
                        break;
                    case "Down":
                        GetText(element).text = BindToText(movement, 7);
                        break;
                    case "Insta morfball":
                        GetText(element).text = BindToText(ball,1);
                        break;
                    case "Ammo selection":
                        GetText(element).text = BindToText(selectItems,1);
                        break;
                    case "Pause":
                        GetText(element).text = BindToText(pause,1);
                        break;
                    case "Player pause":
                        GetText(element).text = BindToText(playerPause,1);
                        break;
                    case "Diagonal up":
                        GetText(element).text = BindToText(aim, 4);
                        break;
                    case "Diagonal down":
                        GetText(element).text = BindToText(aim, 5);
                        break;
                    case "Back":
                        GetText(element).text = BindToText(back);
                        break;
                    case "Select":
                        GetText(element).text = BindToText(submit);
                        break;
                    case "Speed booster":
                        GetText(element).text = BindToText(running,1);
                        break;
                }
            }
        }
    }
    public void StartRebindind(GameObject theGameObject){
        UIText=theGameObject.GetChild(0).GetComponent<Text>();
        keyText=theGameObject.GetChild(1).GetComponent<Text>();
        uiText=UIText.text;
        UIText.text=text;
        playerInput.SwitchCurrentActionMap("NNN");
        PerformIR(actionReference);
    }
    public void Save(){
        string bindings=playerInput.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds",bindings);
    }
    private void PerformIR(InputActionReference inputAction){
        rebindingOperation = inputAction.action.PerformInteractiveRebinding(compositeIndex)
        .WithControlsExcluding("Mouse").OnMatchWaitForAnother(0.1f).OnComplete(operation => RebindComplete(inputAction)).Start();
    }
    private void RebindComplete(InputActionReference inputAction){
        keyText.text=BindToText(inputAction);
        playerInput.SwitchCurrentActionMap("Player");
        rebindingOperation.Dispose();
        UIText.text = uiText;
        playerMenuInput = false;
        compositeIndex = 0;
    }
    private string BindToText(InputActionReference actionReference){
        return InputControlPath.ToHumanReadableString(
            actionReference.action.bindings[compositeIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );
    }
    private string BindToText(InputActionReference actionReference, int index)
    {
        return InputControlPath.ToHumanReadableString(
            actionReference.action.bindings[index].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );
    }
    private Text GetText(GameObject theGameObject){
        return theGameObject.GetChild(1).GetComponent<Text>();
    }
}