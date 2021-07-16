using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UI.Controller;

public class PlayerNavigation : Controller
{
    [SerializeField] Button up,right,down,left;
    void Awake()
    {
        move = playerInput.FindActionMap("UI").FindAction("Move");
    }
     protected override void OnEnable()
    {
        move.started+=MoveAround;
    }
     protected override void OnDisable(){
        move.started -= MoveAround;
    }
    protected override void MoveAround(InputAction.CallbackContext context){
        float x=context.ReadValue<Vector2>().x;
        float y = context.ReadValue<Vector2>().y;
        if(x==1)right.onClick?.Invoke();
        else if(x==-1)left.onClick?.Invoke();
        else if(y==1)up.onClick?.Invoke();
        else if(y==-1)down.onClick?.Invoke();
    }
}