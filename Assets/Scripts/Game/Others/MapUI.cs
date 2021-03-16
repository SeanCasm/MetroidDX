using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    [SerializeField]GameObject mapUIPrefab;
    [SerializeField]Canvas canvas;
    GameObject mapUI;
    public void InstantiateAndPutOnCanvas(){
        mapUI=Instantiate(mapUIPrefab,canvas.transform.position,
        Quaternion.identity,canvas.transform);
    }
    public void CutMapUI(){
        Destroy(mapUI);
        mapUI=null;
    }
}