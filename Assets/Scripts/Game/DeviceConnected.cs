using System.Collections;
using System.Collections.Generic;
using Game.Device;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DeviceConnected : MonoBehaviour
{
    [SerializeField] Button keyboard, gamepad, firstOnSettings, backLast;
    [SerializeField] PlayerInput input;

    private void OnEnable()
    {
        switch (input.currentControlScheme)
        {
            case "Gamepad":
                gamepad.gameObject.SetActive(true);
                RebindKeys.deviceType = Device.Gamepad;

                SetNavigation(firstOnSettings, gamepad, false);
                SetNavigation(backLast, gamepad, true);
                break;
            case "Keyboard&Mouse":
                keyboard.gameObject.SetActive(true);
                RebindKeys.deviceType = Device.Keyboard;

                SetNavigation(firstOnSettings, keyboard, false);
                SetNavigation(backLast, keyboard, true);
                break;
        }
    }
    private void SetNavigation(Button button, Button buttonDevice, bool onUp)
    {
        Navigation nav = button.navigation;
        if (onUp)
        {
            nav.selectOnUp = buttonDevice;
            nav.selectOnDown = firstOnSettings;

        }
        else
        {
            nav.selectOnDown = buttonDevice;
            nav.selectOnUp = backLast;
        }
        button.navigation = nav;
    }
}
