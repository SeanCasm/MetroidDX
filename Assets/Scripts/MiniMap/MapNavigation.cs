using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.Security.Cryptography;

public class MapNavigation : MonoBehaviour
{
    [SerializeField]float speedTroughtNavigation;
    [SerializeField]Transform mapCursor,playerIcon;
    [SerializeField] RenderTexture camRender;
    Camera mapCamera;
    bool canMove;
    float verticalAxis,horizontalAxis;
    public float SpeedNavigation{get=>speedTroughtNavigation;set=>speedTroughtNavigation=value;}
    void Awake()
    {
        mapCamera = GetComponent<Camera>();
    }
    private void OnEnable() {
        canMove=true;
    }
    private void OnDisable() {
        canMove=false;
    }
    public void CameraSetOutputTexture()
    {
        if (mapCamera.targetTexture) mapCamera.targetTexture = null;
        else mapCamera.targetTexture = camRender;
    }
    public void MoveVer(InputAction.CallbackContext context)
    {
        if (context.performed && Pause.onMap)
        {
            verticalAxis = context.ReadValue<float>();
            StartCoroutine(VerticalCamMovement());
        }else if (context.canceled && Pause.onMap)
        {
            verticalAxis=0;
            StopCoroutine(VerticalCamMovement());
        }
    }
    public void MoveHor(InputAction.CallbackContext context)
    {
        if (context.performed && Pause.onMap)
        {
            horizontalAxis = context.ReadValue<float>();
            StartCoroutine(HorizontalCamMovement());
        }else if(context.canceled && Pause.onMap){
            horizontalAxis=0;
            StopCoroutine(HorizontalCamMovement());
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