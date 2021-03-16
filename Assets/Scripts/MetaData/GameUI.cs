using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameUI : MonoBehaviour
{
    public class AmmoUI
    {
        private int iD { get; set; }
        private GameObject ammoGeneralUI, ammoImageObjectUI;
        private Text ammoText;
        public AmmoUI(int iD,GameObject ammoGeneralUI)
        {
            this.iD = iD;this.ammoGeneralUI = ammoGeneralUI;
            ammoText = ammoGeneralUI.transform.GetChild(0).GetComponent<Text>();
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
    private List<AmmoUI> ammoUI;
    [SerializeField] Text health;
    [SerializeField] RectTransform healthImage;
    #endregion
    private void OnEnable() {
        GameEvents.playerHealth += UpdateHealth;
    }
    private void OnDisable() {
        GameEvents.playerHealth -= UpdateHealth;
    }
    private void Start()
    {
        ///Creates a ammo collection UI (missiles UI,super missiles UI...)
        ammoUI = new List<AmmoUI>();
        for(int i = 0; i < pInventory.limitedAmmo.Count; i++)
        {
            ammoUI.Add(new AmmoUI(i, transform.GetChild(i).gameObject));
            if (pInventory.limitedAmmo[i] != null)
            {
                pInventory.limitedAmmo[i].ammoText += ammoUI[i].UpdateText;
                pInventory.limitedAmmo[i].toggleUI += ammoUI[i].ToggleSelection;
                pInventory.limitedAmmo[i].enableUI += ammoUI[i].EnableUI;
                pInventory.limitedAmmo[i].StablishUI();
            }
        }
        GameEvents.playerHealth.Invoke(pHealth.MyHealth,pHealth.Tanks);
    }
    public void AddAndSubscribe(int iD)
    {
        ammoUI.Add(new AmmoUI(iD, transform.GetChild(iD).gameObject));
        pInventory.limitedAmmo[iD].ammoText += ammoUI[iD].UpdateText;
        pInventory.limitedAmmo[iD].toggleUI += ammoUI[iD].ToggleSelection;
        pInventory.limitedAmmo[iD].enableUI += ammoUI[iD].EnableUI;
        pInventory.limitedAmmo[iD].StablishUI();
    }
    private void UpdateHealth(int amount,int tanks){
        health.text="Energy: "+amount.ToString();
        healthImage.sizeDelta = new Vector2(16f *tanks, 16f);
    }
}