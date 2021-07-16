using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
public class Slots : MonoBehaviour
{
    [SerializeField] GameObject[] buttonGames, energySlots, missileSlots, superMissileSlots, superBombSlots;
    [SerializeField] RectTransform[] energyUISlots;
    [SerializeField]LoadScenes sceneLoader;
    [SerializeField]SaveAndLoad saveLoad;
    [SerializeField]PlayerInput playerInput;
    [Tooltip("Array of play time in order: slot-1,slot-2,slot-3")]
    [SerializeField] TextMeshProUGUI[] times;
    private GameData data;
    void Awake()
    {
        Pause.onSlots=true;
    }
    // Start is called before the first frame update
    void Start()
    {
        Pause.OnPause?.Invoke(true);
        Pause.onGame=false;
        string rebinds=PlayerPrefs.GetString("rebinds",string.Empty);
        if(!string.IsNullOrEmpty(rebinds)){
            playerInput.actions.LoadBindingOverridesFromJson(rebinds);
        }
        LoadDataToSlots();
    }
    public void LoadDataToSlots()
    {
        for(int i=0;i<3;i++){
            if(SaveSystem.LoadPlayerSlot(i)!=null){
                data = SaveSystem.LoadPlayerSlot(i);
                ShowItems(missileSlots, energySlots, energyUISlots, superMissileSlots, superBombSlots, i);
            }
        }
        for(int i=0;i<3;i++)ShowButtons(i);
    }
    void ShowItems(GameObject[] missilesSlot,GameObject[] energySlot,RectTransform[] energyUI,
        GameObject[] superMissileSlot,GameObject[] superBombSlot,int slotIndex)
    {
        int totalTanks = data.tanks;
        if (data.ammoMunition.ContainsKey(0))
        {
            missilesSlot[slotIndex].SetActive(true);
            missilesSlot[slotIndex].GetComponentInChildren<TextMeshProUGUI>().text = data.ammoMunition[0].ToString();
        }
        if (data.ammoMunition.ContainsKey(1))
        {
            superMissileSlot[slotIndex].SetActive(true);
            superMissileSlot[slotIndex].GetComponentInChildren<TextMeshProUGUI>().text= data.ammoMunition[1].ToString();
        }
        if (data.ammoMunition.ContainsKey(2))
        {
            superBombSlot[slotIndex].SetActive(true);
            superBombSlot[slotIndex].GetComponentInChildren<TextMeshProUGUI>().text = data.ammoMunition[2].ToString();
        }
        times[slotIndex].text = TimeCounter.TimeArrayIntToString(data.time);
        energySlot[slotIndex].SetActive(true);
        energyUI[slotIndex].sizeDelta = new Vector2(16f * totalTanks, 16f);
    }
    public void ErasePlayerSlot(int slotIndex)
    {
        SaveSystem.ErasePlayerSlot(slotIndex);
        energyUISlots[slotIndex].sizeDelta = new Vector2(0, 16f);
        energySlots[slotIndex].SetActive(false);
        ShowButtons(slotIndex);
    }
    public void ShowButtons(int slotIndex)
    {
        TextMeshProUGUI buttonText= buttonGames[slotIndex].GetComponentInChildren<TextMeshProUGUI>();
        if (SaveSystem.LoadPlayerSlot(slotIndex) != null)buttonText.text="Continue";
        else buttonText.text = "New Game";
    }
    private void NewGameData()
    {
        SaveAndLoad.newGame=true;
        sceneLoader.LoadStartScene();
    }
    public void SetData(int loadSlot)
    {
        Pause.onGame=true;
        SaveAndLoad.slot = loadSlot;
        for(int i=0;i<3;i++){
            if(loadSlot==i){
                if (SaveSystem.LoadPlayerSlot(i) != null){SaveAndLoad.newGame=false; saveLoad.LoadPlayerSlot(i);}
                else NewGameData();
            }
        }
        Pause.OnPause?.Invoke(false);
        GameEvents.timeCounter.Invoke(true);
    }
    void OnDisable()
    {
        Pause.onSlots = false;
    }
}