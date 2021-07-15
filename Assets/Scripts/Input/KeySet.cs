using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeySet : MonoBehaviour
{
    [SerializeField] int compositeIndex = -1, compositeIndexGP = -1;
    [SerializeField] InputActionReference actionReference;
    [SerializeField] RebindKeys rebindKeys;
    private string actionTextAux;
    private const string text = "Waiting for input...";
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    private int index;

    private Text keyText, actionText;
    private void Awake()
    {
        actionText = gameObject.GetChild(0).GetComponent<Text>();
        keyText = gameObject.GetChild(1).GetComponent<Text>();
        if (RebindKeys.deviceType ==Device.Keyboard) index = compositeIndex;
        else index = compositeIndexGP;
        keyText.text = BindToText();
    }
    public void SetIndex(bool keyboard){
        if(keyboard)index = compositeIndex;
        else index = compositeIndexGP;
    }
    public void SetKeyText(){
        keyText = gameObject.GetChild(1).GetComponent<Text>();
        keyText.text=BindToText();
    }
    private string BindToText()
    {
        return InputControlPath.ToHumanReadableString(
            actionReference.action.bindings[index].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice
        );
    }
    public void StartRebindind()
    {
        actionTextAux = actionText.text;
        actionText.text = text;
        rebindKeys.Input.SwitchCurrentActionMap("NNN");
        PerformIR();
    }

    public void PerformIR()
    {
        actionReference.action.Disable();
        rebindingOperation = actionReference.action.PerformInteractiveRebinding(index)
        .WithControlsExcluding("Mouse").OnMatchWaitForAnother(0.1f).OnComplete(operation => RebindComplete()).Start();
    }
    public void RebindComplete()
    {
        actionReference.action.Enable();
        keyText.text = BindToText();
        rebindKeys.Input.SwitchCurrentActionMap("Player");
        rebindingOperation.Dispose();
        actionText.text = actionTextAux;
        switch (actionText.text)
        {
            case "Select":
                GameEvents.OnInputBinded.Invoke(keyText.text,Input.Select);
                break;
            case "Back":
                GameEvents.OnInputBinded.Invoke(keyText.text,Input.Back);
                break;
            case "Jump":
                GameEvents.OnInputBinded.Invoke(keyText.text, Input.Space);
                break;
        }
    }
}
public enum Input{Select, Back, Space}