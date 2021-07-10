using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
 

public class MapNavigation : MonoBehaviour
{
    [SerializeField] InputActionAsset playerInput;
    [SerializeField]float speedTroughtNavigation;
    [SerializeField]Transform mapCursor,playerIcon;
    [SerializeField] RenderTexture camRender;
    [SerializeField] UnityEvent back,enable;
    Camera mapCamera;
    private InputActionMap _inputActionMap;
    private InputAction moveVer,moveHor;
    bool canMove;
    float verticalAxis,horizontalAxis;
    public float SpeedNavigation{get=>speedTroughtNavigation;set=>speedTroughtNavigation=value;}
    void Awake()
    {
        mapCamera = GetComponent<Camera>();
        _inputActionMap=playerInput.FindActionMap("Player");
        moveHor=_inputActionMap.FindAction("AD");
        moveVer=_inputActionMap.FindAction("WS");
    }
    private void OnEnable() {
        canMove=true;
        enable.Invoke();
        mapCamera.orthographicSize=35;
        mapCamera.targetTexture = null;

        moveHor.performed+=MoveHor;
        moveHor.canceled+= CancelHor;
        moveVer.performed+=MoveVer;
        moveVer.canceled+=CancelVer;
    }
    private void OnDisable() {
        canMove=false;
        mapCamera.targetTexture = camRender;
        mapCamera.orthographicSize = 5.3f;

        moveHor.performed -= MoveHor;
        moveHor.canceled -= CancelHor;
        moveVer.performed -= MoveVer;
        moveVer.canceled -= CancelVer;
        back.Invoke();
    }
    private void MoveVer(InputAction.CallbackContext context)
    {
        if (context.performed && Pause.onMap)
        {
            verticalAxis = context.ReadValue<float>();
            StartCoroutine("VerticalCamMovement");
        }
    }
    private void CancelVer(InputAction.CallbackContext context){
        if (context.canceled && Pause.onMap)
        {
            verticalAxis = 0;
            StopCoroutine("VerticalCamMovement");
        }
    }
    private void CancelHor(InputAction.CallbackContext context){
        if (context.canceled && Pause.onMap)
        {
            horizontalAxis = 0;
            StopCoroutine("HorizontalCamMovement");
        }
    }
    private void MoveHor(InputAction.CallbackContext context)
    {
        if (context.performed && Pause.onMap)
        {
            horizontalAxis = context.ReadValue<float>();
            StartCoroutine("HorizontalCamMovement");
        }
    }
    IEnumerator HorizontalCamMovement(){
        while(horizontalAxis!=0){
            mapCursor.localPosition=new Vector3(mapCursor.localPosition.x + speedTroughtNavigation*horizontalAxis, mapCursor.localPosition.y, 0f);
            yield return new WaitForSecondsRealtime(.05f);
        }
    }
    IEnumerator VerticalCamMovement()
    {
        while (verticalAxis != 0)
        {
            mapCursor.localPosition = new Vector3(mapCursor.localPosition.x, mapCursor.localPosition.y + speedTroughtNavigation * verticalAxis, 0f);
            yield return new WaitForSecondsRealtime(.05f);
        }
    }
}