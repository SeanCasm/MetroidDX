using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScrollKeyboard : MonoBehaviour
{
    [SerializeField] InputActionAsset playerInput;
    [SerializeField] int totalItems;
    [Tooltip("Container of the items.")]
    [SerializeField] RectTransform container;
    [Tooltip("The spacing between items")]
    [SerializeField] float spacing;
    [Header("Input System UI Input Module values")]
    [SerializeField] float moveRepeatDelay;
    [SerializeField] float moveRepeatRate;
    private InputAction moveVer;
    private InputActionMap _inputActionMap;
    int currentItem = 0, middlep1=3, middlep2=11, add;
    float delay;
    bool onMiddle, plus, minus, holding;
    private void Awake()
    {
        _inputActionMap = playerInput.FindActionMap("Player");
        moveVer=_inputActionMap.FindAction("WS");
    }
    private void OnEnable()
    {
        moveVer.performed += MoveVer;
        moveVer.canceled += CancelVer;
    }
    private void OnDisable()
    {
        moveVer.performed -= MoveVer;
        moveVer.canceled -= CancelVer;
        container.anchoredPosition=Vector2.zero;
        add=currentItem=0;
        delay=0;
        plus=minus=false;
    }
    public void MoveVer(InputAction.CallbackContext context)
    {
        StopAllCoroutines();
        float verticalAxis = context.ReadValue<float>();
        
        if (verticalAxis < 0 )
        {
            Plus();
            IsOnMiddle();

            StartCoroutine("OnHolding");
            if (onMiddle) container.anchoredPosition = new Vector3(0, container.anchoredPosition.y + spacing, 0);
        }
        else if (verticalAxis > 0)
        {
            Minus();
            IsOnMiddle();

            StartCoroutine("OnHolding");
            if (onMiddle) container.anchoredPosition = new Vector3(0, container.anchoredPosition.y - spacing, 0);
        }
        Check();
    }
    private void CancelVer(InputAction.CallbackContext context)
    {
        holding = false;
        StopAllCoroutines();
    }
    private void Check()
    {
        if (currentItem > totalItems-1) currentItem = 0;
        else if (currentItem < 0) currentItem = totalItems;
    }
    private void Plus(){
        currentItem++;
        if (currentItem == middlep2) container.anchoredPosition = new Vector3(0, container.anchoredPosition.y + spacing, 0);
        if (currentItem > totalItems - 1)
        {
            currentItem = 0;
            container.anchoredPosition = new Vector3(0, 0, 0);//container in his original position
        }
        holding = plus = true;
        minus = false;
        add = 1;
    }
    private void Minus(){
        currentItem--;
        if (currentItem == middlep1) container.anchoredPosition = new Vector3(0, container.anchoredPosition.y - spacing, 0);
        if (currentItem < 0)
        {
            currentItem = totalItems - 1;
            container.anchoredPosition = new Vector3(0, 264f, 0);//container in his max y position
        }
        plus = false;
        holding = minus = true;
        add = -1;
    }
    private void IsOnMiddle(){
        if (currentItem <= middlep1 || currentItem >= middlep2) onMiddle = false;
        else onMiddle = true;
    }
    IEnumerator OnHolding()
    {
        delay = moveRepeatDelay;
        while (holding)
        {
            yield return new WaitForSeconds(delay);
            if (plus) Plus();
            else if (minus) Minus();
            IsOnMiddle();
            if (onMiddle) container.anchoredPosition = new Vector3(0, container.anchoredPosition.y + spacing * add, 0);

            Check();
            delay = moveRepeatRate;
        }
    }
}
