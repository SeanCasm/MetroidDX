using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Game.Device;
public class RebindKeys : MonoBehaviour
{
    [SerializeField] PlayerInput playerInput;
    public PlayerInput Input=>playerInput;
    public static Device deviceType;
    /// <summary>
    /// Saves the input bindings at button onClick.
    /// </summary>
    public void Save(){
        string bindings=playerInput.actions.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString("rebinds",bindings);
    }
    
}