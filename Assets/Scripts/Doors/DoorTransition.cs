using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorTransition : MonoBehaviour
{
    [SerializeField] GameObject doorTransition;
    private void OnEnable() {
        GameEvents.doorTransition+=SetTransition;
    }
    private void SetTransition(bool active){
        doorTransition.SetActive(active);
    }
}
