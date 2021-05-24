using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Touchpad : MonoBehaviour
{
    private void OnEnable() {
        Pause.touchpadPaused -= SetActive;
        Pause.touchpadPaused+=SetActive;
    }
    private void SetActive(bool disable){
        gameObject.SetActive(disable);
    }
}
