using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class RebindKeys : MonoBehaviour
{
    [SerializeField] InputActionAsset inputAsset;
    [SerializeField] PlayerInput playerInput;
    public PlayerInput Input=>playerInput;
    public static Device deviceType;
    private InputActionMap _inputActionMap;
  
    /// <summary>
    /// Saves the input bindings at button onClick.
    /// </summary>
    public void Save(){
        string bindings=playerInput.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebindsKeyboard",bindings);
    }
    
}