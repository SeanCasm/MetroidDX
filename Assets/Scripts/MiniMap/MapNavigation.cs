using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Cinemachine;
public class MapNavigation : MonoBehaviour
{
    [SerializeField] InputActionAsset playerInput;
    [SerializeField] float speedTroughtNavigation;
    [SerializeField] RenderTexture camRender;
    [SerializeField] UnityEvent back, enable;
    [SerializeField] Camera main;
    private InputActionMap _inputActionMap;
    private InputAction moveVer, moveHor, centerCamera,backAction;
    bool canMove;
    float verticalAxis, horizontalAxis;
    public float SpeedNavigation { get => speedTroughtNavigation; set => speedTroughtNavigation = value; }
    void Awake()
    {
        _inputActionMap = playerInput.FindActionMap("Player");
        moveHor = _inputActionMap.FindAction("AD");
        centerCamera = _inputActionMap.FindAction("Jump");
        moveVer = _inputActionMap.FindAction("WS");
        backAction=playerInput.FindActionMap("UI").FindAction("Back");
    }
    private void OnEnable()
    {
        canMove = true;
        enable.Invoke();
        main.orthographicSize = 35;
        main.targetTexture = null;

        moveHor.performed += MoveHor;
        moveHor.canceled += CancelHor;
        moveVer.performed += MoveVer;
        moveVer.canceled += CancelVer;
        centerCamera.started += CenterCam;
        backAction.started+=Back;
    }
    private void OnDisable()
    {
        canMove = false;
        back?.Invoke();
        main.targetTexture = camRender;
        main.orthographicSize = 5.3f;

        moveHor.performed -= MoveHor;
        moveHor.canceled -= CancelHor;
        moveVer.performed -= MoveVer;
        moveVer.canceled -= CancelVer;
        centerCamera.started -= CenterCam;
        backAction.started -= Back;
    }
    private void Back(InputAction.CallbackContext context){
        gameObject.SetActive(false);
    }
    private void CenterCam(InputAction.CallbackContext context)
    {
        main.transform.localPosition = Vector2.zero;
    }
    private void MoveVer(InputAction.CallbackContext context)
    {
        verticalAxis = context.ReadValue<float>();
        StartCoroutine("VerticalCamMovement");
    }
    private void CancelVer(InputAction.CallbackContext context)
    {
        verticalAxis = 0;
        StopCoroutine("VerticalCamMovement");
    }
    private void CancelHor(InputAction.CallbackContext context)
    {
        horizontalAxis = 0;
        StopCoroutine("HorizontalCamMovement");
    }
    private void MoveHor(InputAction.CallbackContext context)
    {
        horizontalAxis = context.ReadValue<float>();
        StartCoroutine("HorizontalCamMovement");
    }
    IEnumerator HorizontalCamMovement()
    {
        while (horizontalAxis != 0)
        {
            main.transform.localPosition = new Vector3(main.transform.localPosition.x + speedTroughtNavigation * horizontalAxis, main.transform.localPosition.y, 0f);
            yield return new WaitForSecondsRealtime(.05f);
        }
    }
    IEnumerator VerticalCamMovement()
    {
        while (verticalAxis != 0)
        {
            main.transform.localPosition = new Vector3(main.transform.localPosition.x, main.transform.localPosition.y + speedTroughtNavigation * verticalAxis, 0f);
            yield return new WaitForSecondsRealtime(.05f);
        }
    }
}