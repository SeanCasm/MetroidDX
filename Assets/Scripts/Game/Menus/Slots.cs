using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class Slots : MonoBehaviour
{
    [SerializeField] GameObject[] buttonGames, energySlots, missileSlots, superMissileSlots, superBombSlots;
    [SerializeField]Transform spawnPoint;
    [SerializeField] RectTransform[] energyUISlots;
    [SerializeField]LoadScenes sceneLoader;
    [SerializeField]SaveAndLoad saveLoad;
    [SerializeField]PlayerInput playerInput;
    private GameData data;
    public GameObject player;
    void Awake()
    {
        Pause.onSlots=true;
    }
    // Start is called before the first frame update
    void Start()
    {
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
            missilesSlot[slotIndex].GetComponentInChildren<Text>().text = data.ammoMunition[0].ToString();
        }
        if (data.ammoMunition.ContainsKey(1))
        {
            superMissileSlot[slotIndex].SetActive(true);
            superMissileSlot[slotIndex].GetComponentInChildren<Text>().text= data.ammoMunition[1].ToString();
        }
        if (data.ammoMunition.ContainsKey(2))
        {
            superBombSlot[slotIndex].SetActive(true);
            superBombSlot[slotIndex].GetComponentInChildren<Text>().text = data.ammoMunition[2].ToString();
        }
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
        Text buttonText= buttonGames[slotIndex].GetComponentInChildren<Text>();
        if (SaveSystem.LoadPlayerSlot(slotIndex) != null)buttonText.text="Continue";
        else buttonText.text = "New Game";
    }
    private void NewGameData()
    {
        player.transform.position = new Vector3(spawnPoint.position.x, spawnPoint.position.y,0f);
        sceneLoader.UnloadCurrentScene();
    }
    public void SetData(int loadSlot)
    {
        Pause.onGame=true;
        SaveAndLoad.slot = loadSlot;
        for(int i=0;i<3;i++){
            if(loadSlot==i){
                if (SaveSystem.LoadPlayerSlot(i) != null) saveLoad.LoadPlayerSlot(i);
                else NewGameData();
            }
        }
    }
    void OnDisable()
    {
        Pause.onSlots = false;
    }
}