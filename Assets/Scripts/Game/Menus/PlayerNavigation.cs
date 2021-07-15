using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerNavigation : MonoBehaviour
{
    [SerializeField] Button up,right,down,left;
    [SerializeField] InputActionAsset playerInput;
    private InputAction move;
    private void OnEnable()
    {
        move=playerInput.FindActionMap("UI").FindAction("Move");
        move.started+=Move;
    }
    private void OnDisable(){
        move.started -= Move;
    }
    private void Move(InputAction.CallbackContext context){
        float x=context.ReadValue<Vector2>().x;
        float y = context.ReadValue<Vector2>().y;
        if(x==1)right.onClick?.Invoke();
        else if(x==-1)left.onClick?.Invoke();
        else if(y==1)up.onClick?.Invoke();
        else if(y==-1)down.onClick?.Invoke();
    }
    
}