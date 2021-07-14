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
    protected void OnEnable() {
        PlayerInventory.GravitySetted+=SetSlow;
        PlayerInventory.GravityUnsetted+=UnsetSlow;
    }
    protected void OnDisable() {
        PlayerInventory.GravitySetted -= SetSlow;
        PlayerInventory.GravityUnsetted -= UnsetSlow;
    }
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && col.IsTouching(waterCollider))
        {
            pAnimator = col.GetComponentInParent<Animator>();
            pSkin=col.GetComponentInParent<SkinSwapper>();
            if (!pSkin.Gravity)
            {
                PlayerController.slow=1.75f;
                pAnimator.SetFloat("AnimSpeed", animationSpeed);
            }else bE2d.enabled = false;
        }
    }
    
    private void SetSlow(){
        waterCollider.enabled=bE2d.enabled = true;
    }
    private void UnsetSlow(){
        waterCollider.enabled=bE2d.enabled = false;
    }
    protected void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            pAnimator.SetFloat("AnimSpeed", 1f);
            if(!pSkin.Gravity){
                PlayerController.slow = 1;
            }
        }
    }
}
