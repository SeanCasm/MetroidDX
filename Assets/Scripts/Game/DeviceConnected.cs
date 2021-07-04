using System.Collections;
using System.Collections.Generic;
using Game.Device;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
public class DeviceConnected : MonoBehaviour
{
    [SerializeField] PlayerInput input;
    [SerializeField] TextMeshProUGUI controlPanel;
    private void OnEnable()
    {
        StartCoroutine(ListeningForDevice());
    }
    private void OnDisable() {
        StopCoroutine(ListeningForDevice());
    }
    IEnumerator ListeningForDevice(){
        while(gameObject.activeSelf){
            switch (input.currentControlScheme)
            {
                case "Gamepad":
                    RebindKeys.deviceType = Device.Gamepad;
                    controlPanel.text = "Gamepad options";
                    break;
                case "Keyboard&Mouse":
                    RebindKeys.deviceType = Device.Keyboard;
                    controlPanel.text = "Keyboard options";
                    break;
            }
            yield return new WaitForSecondsRealtime(.5f);
        }
    }
}
