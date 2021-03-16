using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorTransition : MonoBehaviour
{
    [SerializeField] GameObject doorTransition;
    private static GameObject transitionReference;
    private void Awake() {
        transitionReference=doorTransition;   
    }
    public static bool Transition { set { transitionReference.SetActive(value) ; } }

}
