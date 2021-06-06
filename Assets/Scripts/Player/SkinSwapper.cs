using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinSwapper : MonoBehaviour
{
    [Tooltip("Menu player suit in background image")]
    [SerializeField] Image suit;
    [SerializeField] Player.BaseData playerSuits;
    [SerializeField] Materials materials;
    private SpriteRenderer spriteRenderer;
    private bool gravityEquiped;
    public bool Gravity{get=>gravityEquiped;}
    private List<Sprite> sheetSuit=new List<Sprite>();
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetPowerSuit()
    {
        suit.sprite = playerSuits.powerSuit;
        gravityEquiped = false;
    }
    public void SetSpeedBooster(bool value)
    {
        if (value) spriteRenderer.material = materials.speedBooster;
        else spriteRenderer.material = materials.defaultMaterial;
    }
    public void SetScrewAttack(bool value)
    {
        if (value) spriteRenderer.material = materials.screwAttack;
        else spriteRenderer.material = materials.defaultMaterial;
    }
    public void SetGravitySuit()
    {
        suit.sprite = playerSuits.gravitySuit;
        sheetSuit= new List<Sprite>();
        foreach(Sprite element in playerSuits.gravityCompleteSheet){
            sheetSuit.Add(element);
        }
        gravityEquiped = true;
    }
    void LateUpdate()
    {
        if (gravityEquiped)
        {
            int sprite = int.Parse(spriteRenderer.sprite.name);
            spriteRenderer.sprite = sheetSuit[sprite];
        }
    }
}