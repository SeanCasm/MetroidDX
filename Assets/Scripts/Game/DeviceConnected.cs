using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.Users;

public class DeviceConnected : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI controlPanel;
    [SerializeField] List<KeySet> keyCollection;

    [Tooltip("Objects who's needs a device to work propertly, used in android build")]
    [SerializeField] List<Button> deviceWorkersCollection;
    [SerializeField] GameObject eventSystemStandalone, eventSystemAndroid,menuPointer;
    public static Device actualDevice;

#if UNITY_ANDROID

    private void Start()
    {
        deviceWorkersCollection.ForEach(item =>
        {
            item.interactable=false;
        });
    }
#endif

    private void OnEnable()
    {
        InputUser.onChange += CheckDevice;
    }
    private void OnDisable()
    {
        InputUser.onChange -= CheckDevice;
    }
    void CheckDevice(InputUser user, InputUserChange change, InputDevice device)
    {
        if (change == InputUserChange.ControlSchemeChanged)
        {
            switch (user.controlScheme.Value.name)
            {
                case "Gamepad":
                    RebindKeys.deviceType = Device.Gamepad;
                    actualDevice = Device.Gamepad;
                    controlPanel.text = "Gamepad options";
#if UNITY_ANDROID
                    deviceWorkersCollection.ForEach(item =>
                    {
                        item.interactable = true;

                    });
                    eventSystemStandalone.SetActive(true);
                    eventSystemAndroid.SetActive(false);
#endif

                    keyCollection.ForEach(item =>
                    {
                        item.SetIndex(false);
                        item.SetKeyText();
                    });
                    break;
#if UNITY_STANDALONE
                case "Keyboard&Mouse":
                    RebindKeys.deviceType = Device.Keyboard;
                    actualDevice=Device.Keyboard;
                    controlPanel.text = "Keyboard options";
                    keyCollection.ForEach(item =>
                    {
                        item.SetIndex(true);

                        item.SetKeyText();
                    });
                    break;
#endif
#if UNITY_ANDROID
                case "Keyboard&Mouse":
                    actualDevice=Device.Touch;
                    eventSystemStandalone.SetActive(false);
                    eventSystemAndroid.SetActive(true);
                    menuPointer.SetActive(false);
                    deviceWorkersCollection.ForEach(item =>
                   {
                       item.interactable = false;

                   });
                    break;
#endif
            }
        }
    }
}
public enum Device{Gamepad, Keyboard,Touch}