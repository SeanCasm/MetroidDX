using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UI.Controller;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemMenuController : Controller
{
    [SerializeField] EventSystem eventSystem;
    [SerializeField] UnityEvent backEvent;
    [SerializeField] ButtonUtilities buttonEssentials;
    void Awake()
    {
        base.back = playerInput.FindActionMap("UI").FindAction("Back");
    }
    protected override void OnEnable()
    {
        base.back.started+=Back;
        SetButtonNavigation();
        Pause.onItemMenu=true;
    }
    protected override void OnDisable()
    {
        base.back.started -= Back;
        Pause.onItemMenu = false;
    }
    protected override void Back(InputAction.CallbackContext context)
    {
        backEvent.Invoke();
    }
    /// <summary>
    /// Sets the navigation to all buttons in item menu UI.
    /// </summary>
    private void SetButtonNavigation()
    {
        var buttons = buttonEssentials.itemButtons;
        bool firsted = false;
        for (int i = 0; i < buttons.Count; i++)
        {
            Button currentButton = buttons[i];
            if (currentButton.gameObject.activeSelf)
            {
                if (!firsted)
                {
                    //Sets the reference to the first selected button.
                    eventSystem.SetSelectedGameObject(currentButton.gameObject);
                    firsted = true;
                }
                Navigation navigation;
                //Sets button.navigation.ondown
                for (int j = i + 1; j < buttons.Count; j++)
                {
                    navigation = currentButton.navigation;
                    Button onDownButton = buttons[j];
                    if (onDownButton.gameObject.activeSelf)
                    {
                        navigation.selectOnDown = onDownButton;
                        currentButton.navigation = navigation;
                        break;
                    }
                }
                //Sets button.navigation.onUp
                for (int k = i - 1; k >= 0; k--)
                {
                    navigation = currentButton.navigation;
                    Button onUpButton = buttons[k];
                    if (onUpButton.gameObject.activeSelf)
                    {
                        navigation.selectOnUp = onUpButton;
                        currentButton.navigation = navigation;
                        break;
                    }
                }
            }
        }
    }
}