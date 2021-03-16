using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blocks;

public class DestroyableBlock : MonoBehaviour
{
    #region Properties
    [SerializeField] string[] canShowingTheBlock;
    [SerializeField] protected BlockType blockType;
    private SpriteRenderer spriteRenderer, childRenderer;
    protected Animator anim;
    private string noIgnore;
    #endregion
    #region Unity methods
    protected void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        childRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    protected void Start()
    {
        SetNoIgnoreTag();
    }
    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(noIgnore))anim.SetBool("Disable", true);
        else CheckCollisionTag(col);
    }
    protected void CheckCollisionTag(Collider2D col){
        foreach (string element in canShowingTheBlock)
        {
            if (col.CompareTag(element)) anim.SetBool("Show", true);
        }
    }
    #endregion
    #region Private methods
    void SetNoIgnoreTag()
    {
        switch (blockType)
        {
            case BlockType.beam:
                noIgnore = "Beam";
                break;
            case BlockType.missile:
                noIgnore = "Missile";
                break;
            case BlockType.bomb:
                noIgnore = "Bomb";
                break;
            case BlockType.superBomb:
                noIgnore = "SuperBomb";
                break;
            case BlockType.superMissile:
                noIgnore = "SuperMissile";
                break;
        }
    }
    /** Desactivates the gameObject from animation event*/
    private void ShowHiddenBlock()
    {
        spriteRenderer.enabled = false;
    }
    private void DeactivatingBlock()
    {
        Destroy(gameObject);
    }
    #endregion
}