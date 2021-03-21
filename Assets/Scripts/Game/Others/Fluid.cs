using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fluid : MonoBehaviour
{
    [SerializeField] BuoyancyEffector2D bE2d;
    [SerializeField] protected BoxCollider2D waterCollider;
    [SerializeField] float animationSpeed;
    protected SkinSwapper pSkin;
    protected PlayerHealth playerH;
    private Animator pAnimator;
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && col.IsTouching(waterCollider))
        {
            pAnimator = col.GetComponentInParent<Animator>();
            pSkin=col.GetComponentInParent<SkinSwapper>();
            if (!pSkin.Gravity)
            {
                pAnimator.SetFloat("AnimSpeed", animationSpeed);
            }
            else bE2d.enabled = false;
        }
    }
    protected void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player") && col.IsTouching(waterCollider))
        {
            if (!pSkin.Gravity){bE2d.enabled = true;pAnimator.SetFloat("AnimSpeed", animationSpeed); }
            else{ bE2d.enabled = false;pAnimator.SetFloat("AnimSpeed",1f);}
        }
    }
    protected void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            pAnimator.SetFloat("AnimSpeed", 1f);
        }
    }
}
