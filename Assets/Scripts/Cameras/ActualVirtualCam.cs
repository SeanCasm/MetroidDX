using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualVirtualCam : MonoBehaviour
{
    public static Cinemachine.CinemachineConfiner CMConfiner;
    private void Awake() {
        CMConfiner=GetComponent<Cinemachine.CinemachineConfiner>();
    }
}
