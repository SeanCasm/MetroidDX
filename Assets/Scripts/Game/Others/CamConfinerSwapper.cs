using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CamConfinerSwapper : MonoBehaviour
{
    [SerializeField]Collider2D newConfiner;
    private void OnEnable() {
       ActualVirtualCam.CMConfiner.m_BoundingShape2D=newConfiner;
    }
}
