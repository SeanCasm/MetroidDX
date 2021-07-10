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
    int currentItem = 1, middleItem, middlep1, middlep2, add;
    float delay;
    bool onMiddle, plus, minus, holding;
    private void Awake()
    {
        _inputActionMap = playerInput.FindActionMap("Player");
        moveVer=_inputActionMap.FindAction("WS");
    }
    private void Start()
    {
        middleItem = totalItems / currentItem;
        middlep2 = totalItems * 3 / 4;
        middlep1 = totalItems * 1 / 4;
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
    }
    public void MoveVer(InputAction.CallbackContext context)
    {
        StopAllCoroutines();
        float verticalAxis = context.ReadValue<float>();
        if (verticalAxis < 0 && (currentItem < totalItems))
        {
            currentItem++;
            holding = plus = true;
            minus = false;
            add = 1;
            StartCoroutine("OnHolding");
            if (onMiddle) container.anchoredPosition = new Vector3(0, container.anchoredPosition.y + spacing, 0);
        }
        else if (verticalAxis > 0 && (currentItem > 1))
        {
            currentItem--;
            plus = false;
            holding = minus = true;
            add = -1;
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
        if (currentItem >= totalItems) currentItem = totalItems;
        else if (currentItem <= 1) currentItem = 1;
        if (currentItem <= middlep1 || currentItem >= middlep2) onMiddle = false;
        else onMiddle = true;
        if(container.anchoredPosition.y<=0)container.anchoredPosition=new Vector2(0,0);
         
    }

    IEnumerator OnHolding()
    {
        delay = moveRepeatDelay;
        while (holding)
        {
            yield return new WaitForSeconds(delay);
            Check();
            if (plus) currentItem++;
            else if (minus) currentItem--;
            if (onMiddle) container.anchoredPosition = new Vector3(0, container.anchoredPosition.y + spacing * add, 0);
            delay = moveRepeatRate;
        }
    }
}
