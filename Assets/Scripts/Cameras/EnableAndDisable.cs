using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnableAndDisable : MonoBehaviour
{
    [SerializeField]bool freezeRigidBody;
    RigidbodyType2D type2D;
    Animator animator;
    Rigidbody2D rigid;

    void Awake()
    {
        TryGetComponent<Rigidbody2D>(out rigid);
        TryGetComponent<Animator>(out animator);
        if(rigid)type2D=rigid.bodyType;
    }
    void OnBecameInvisible()
    {
        if(freezeRigidBody)rigid.bodyType=RigidbodyType2D.Static;
        if(animator)animator.enabled=false;
    }
    void OnBecameVisible()
    {
        if (freezeRigidBody) rigid.bodyType =type2D;
        if (animator)animator.enabled=true;
    }
}
