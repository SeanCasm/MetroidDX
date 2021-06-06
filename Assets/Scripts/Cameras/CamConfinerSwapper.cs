using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CamConfinerSwapper : MonoBehaviour
{
    [SerializeField]Collider2D newConfiner;
    public static System.Action SwapCam;
    private static bool first=true;
    private void OnEnable() {
       if(first)SwapConfiner();
       Warp.OnWarp+=SwapConfiner;
       first=false;
    }
    private void OnDisable() {
        Warp.OnWarp -= SwapConfiner;
    }
    private void SwapConfiner(){
        ActualVirtualCam.CMConfiner.m_BoundingShape2D=newConfiner;
    }
}
