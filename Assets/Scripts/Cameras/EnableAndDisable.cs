using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnableAndDisable : MonoBehaviour
{
    Animator animator;
    void Awake()
    {
        animator=GetComponent<Animator>();
    }
    void OnBecameInvisible()
    {
        if(animator)animator.enabled=false;
    }
    void OnBecameVisible()
    {
        if (animator)animator.enabled=true;
    }
}
