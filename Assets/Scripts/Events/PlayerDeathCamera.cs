using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathCamera : MonoBehaviour
{
    private Cinemachine.CinemachineVirtualCamera cam;
    private void Start() {
        cam=GetComponent<Cinemachine.CinemachineVirtualCamera>();
    }
    private void OnEnable() {
        Retry.Completed+=OnPlayerDeath;
    }
    private void OnDisable() {
        Retry.Completed -= OnPlayerDeath;
    }
    private void OnPlayerDeath(){
        cam.LookAt=References.Player.transform;
        ActualVirtualCam.CMConfiner.m_BoundingShape2D=null;
    }
}
