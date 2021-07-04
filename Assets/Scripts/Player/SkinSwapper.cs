using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinSwapper : MonoBehaviour
{
    [Tooltip("Menu player suit in background image")]
    [SerializeField] Image suit;
    [SerializeField] Suit power,gravity,corrupt; 
    [SerializeField] Materials materials;
    private SpriteRenderer spriteRenderer;
    private bool gravityEquiped;
    public bool Gravity{get=>gravityEquiped;}
    public static System.Action<bool> OnLeft;
    private List<Sprite> suitLeft=new List<Sprite>();
    private List<Sprite> suitRight = new List<Sprite>();
    private List<Sprite> currentSide=new List<Sprite>();
    private void OnEnable() {
        GameEvents.EquipPower+=SetPowerSuit;
        GameEvents.EquipGravity+=SetGravitySuit;
        OnLeft+=SetSide;
    }
    private void OnDisable() {
        GameEvents.EquipPower -= SetPowerSuit;
        GameEvents.EquipGravity -= SetGravitySuit;
        OnLeft -= SetSide;
    }
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public void SetPowerSuit()
    {
        suit.sprite = power.portait;
        suitLeft = new List<Sprite>();
        suitRight = new List<Sprite>();
        foreach (Sprite element in power.suitRight)
        {
            suitRight.Add(element);
        }
        foreach (Sprite element in power.suitLeft)
        {
            suitLeft.Add(element);
        }
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
        suit.sprite = gravity.portait;
        suitLeft= new List<Sprite>();
        suitRight=new List<Sprite>();
        foreach(Sprite element in gravity.suitRight){
            suitRight.Add(element);
        }
        foreach(Sprite element in gravity.suitLeft){
            suitLeft.Add(element);
        }
        gravityEquiped = true;
    }
    private void SetSide(bool left){
        if(left)currentSide=new List<Sprite>(suitLeft);
        else currentSide = new List<Sprite>(suitRight);
    }
    void LateUpdate()
    {
        if(currentSide.Count>0){
            int index = int.Parse(spriteRenderer.sprite.name);
            spriteRenderer.sprite = currentSide[index];
        }
    }
}