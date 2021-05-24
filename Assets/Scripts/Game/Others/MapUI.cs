using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    [SerializeField]GameObject mapUIPrefab,playerMenu;
    [SerializeField]Canvas canvas;
    GameObject mapUI;
    public void InstantiateAndPutOnCanvas(){
        mapUI=Instantiate(mapUIPrefab,canvas.transform.position,
        Quaternion.identity,canvas.transform);
        #if UNITY_ANDROID
        mapUI.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
        {
            CutMapUI();
            playerMenu.SetActive(true);
            Interactions.BackMap.Invoke();
        });
        Destroy(mapUI.GetChild(1).GetChild(0));//destroys a gameobject with attached text component
         
        #endif
    }
    public void CutMapUI(){
        Destroy(mapUI);
        mapUI=null;
    }
}