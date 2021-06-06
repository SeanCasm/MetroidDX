using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameUI : MonoBehaviour
{
    public class AmmoUI
    {
        private int iD { get; set; }
        private GameObject ammoGeneralUI, ammoImageObjectUI;
        private TextMeshProUGUI ammoText;
        public AmmoUI(int iD,GameObject ammoGeneralUI)
        {
            this.iD = iD;this.ammoGeneralUI = ammoGeneralUI;
            ammoText = ammoGeneralUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            ammoImageObjectUI= ammoGeneralUI.transform.GetChild(1).gameObject;

        }
        public void UpdateText(int id, int amount)
        {
            if (iD == this.iD) ammoText.text = amount.ToString();
        }
        public void ToggleSelection(int iD, bool select)
        {
            if (iD == this.iD) ammoImageObjectUI.SetActive(select);
        }
        public void EnableUI(int iD)
        {
            if (iD == this.iD) ammoGeneralUI.SetActive(true);
        }
    }
    #region Properties
    [SerializeField] PlayerInventory pInventory;
    [SerializeField]PlayerHealth pHealth;
    private Dictionary<int,AmmoUI> ammoUI;
    [SerializeField] TextMeshProUGUI health;
    [SerializeField] RectTransform healthImage;
    #endregion
    private void OnEnable() {
        GameEvents.playerHealth += UpdateHealth;
    }
    private void OnDestroy() {
        GameEvents.playerHealth -= UpdateHealth;

    }
    private void Start()
    {
        ///Creates a ammo collection UI (missiles UI,super missiles UI...)
        ammoUI = new Dictionary<int, AmmoUI>();
        var ammo=pInventory.limitedAmmo;
        for(int i = 0; i < ammo.Count; i++)
        {
            ammoUI.Add(ammo[i].iD,new AmmoUI(ammo[i].iD, transform.GetChild(ammo[i].iD).gameObject));
            if (pInventory.limitedAmmo[i] != null)
            {
                ammo[i].ammoText += ammoUI[i].UpdateText;
                ammo[i].toggleUI += ammoUI[i].ToggleSelection;
                ammo[i].enableUI += ammoUI[i].EnableUI;
                ammo[i].StablishUI();
            }
        }
        GameEvents.playerHealth.Invoke(pHealth.MyHealth,pHealth.Tanks);
    }
    public void AddAndSubscribe(int iD)
    {
        var ammo=pInventory.limitedAmmoSearch;
        ammoUI.Add(iD,new AmmoUI(iD, transform.GetChild(iD).gameObject));
        ammo[iD].ammoText += ammoUI[iD].UpdateText;
        ammo[iD].toggleUI += ammoUI[iD].ToggleSelection;
        ammo[iD].enableUI += ammoUI[iD].EnableUI;
        ammo[iD].StablishUI();
         
    }
    private void UpdateHealth(int amount,int tanks){
        health.text="Energy: "+amount.ToString();
        healthImage.sizeDelta = new Vector2(16f *tanks, 16f);
    }
}