using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionSubscriber : MonoBehaviour
{
    [Range(0.1f,3f)]
    [SerializeField]float animationTime;
    private Animator animator;
    private void Awake() {
        animator=GetComponent<Animator>();
    }
    private void OnEnable() {
        GameEvents.StartTransition+=StartTrantision;
    }
    private void OnDisable() {
        GameEvents.StartTransition -=StartTrantision;
    }
    private float StartTrantision(){
        animator.SetTrigger("Start");
        return animationTime;
    }
}
