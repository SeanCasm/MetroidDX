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
    public float SpeedNavigation{get=>speedTroughtNavigation;set=>speedTroughtNavigation=value;}
    void Awake()
    {
        mapCamera = GetComponent<Camera>();
    }
    public void CameraSetOutputTexture()
    {
        if (mapCamera.targetTexture) mapCamera.targetTexture = null;
        else mapCamera.targetTexture = camRender;
    }
    public void Movement(InputAction.CallbackContext context)
    {
        if (context.performed && mapCursor.gameObject.activeSelf)
        {
            float verticalAxis = context.ReadValue<Vector2>().y;
            float horizontalAxis = context.ReadValue<Vector2>().x;
            if (verticalAxis < 0)
            {
                mapCursor.localPosition = new Vector3(mapCursor.localPosition.x, mapCursor.localPosition.y - speedTroughtNavigation, 0f);
            }
            else if (verticalAxis > 0)
            {
                mapCursor.localPosition = new Vector3(mapCursor.localPosition.x, mapCursor.localPosition.y + speedTroughtNavigation, 0f);
            }else
            if (horizontalAxis < 0)
            {
                mapCursor.localPosition = new Vector3(mapCursor.localPosition.x - speedTroughtNavigation, mapCursor.localPosition.y, 0f);
            }
            else if (horizontalAxis > 0)
            {
                mapCursor.localPosition = new Vector3(mapCursor.localPosition.x + speedTroughtNavigation, mapCursor.localPosition.y, 0f);
            }
        }
    } 
}