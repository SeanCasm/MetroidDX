using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace UI.Controller{
    public abstract class Controller : MonoBehaviour
    {
        [SerializeField] protected InputActionAsset playerInput;
        protected InputActionMap _inputActionMap;
        protected InputAction back,select,moveVer,moveHor,move;
        protected InputAction GetActionFromActionMap(string action){
            _inputActionMap=playerInput.FindActionMap("UI");
            return _inputActionMap.FindAction(action); 
        }
        protected abstract void OnEnable();
        protected abstract void OnDisable();
        #region Action.started methods
        protected virtual void Back(InputAction.CallbackContext context) { }
        protected virtual void Select(InputAction.CallbackContext context) { }
        protected virtual void MoveVer(InputAction.CallbackContext context) { }
        protected virtual void MoveHor(InputAction.CallbackContext context) { }
        protected virtual void MoveAround(InputAction.CallbackContext context) { }
        #endregion
    }
}