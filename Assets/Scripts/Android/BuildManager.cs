using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BuildManager : MonoBehaviour
{
    [Header("Global config")]
    [SerializeField] GameObject touchpadReference;
    [SerializeField] List<GameObject> controlDeviceReference;
    [SerializeField] List<MonoBehaviour> classes;
    [SerializeField] List<GameObject> menuMessages;
    [SerializeField] GameObject eventSystemStandalone, eventSystemAndroid;
    [SerializeField] List<Image> raycasteableImages;
    [SerializeField]List<GameObject> mobileButtons;
    [Header("Player UI and inventory config")]
    [SerializeField]List<GameObject> ammoUI;
    [SerializeField]PlayerInventory playerInventory;
    private void Awake()
    {
#if UNITY_ANDROID
        raycasteableImages.ForEach(item =>
        {
            item.raycastTarget = true;
        });
        controlDeviceReference.ForEach(item =>
        {
            Destroy(item);
        });
        classes.ForEach(item =>
        {
            Destroy(item);
        });
        menuMessages.ForEach(item =>
        {
            Destroy(item);
        });
        ammoUI.ForEach(item=>{
            item.GetComponent<Button>().enabled=true;
        });
        touchpadReference.SetActive(true);
        eventSystemAndroid.SetActive(true);
        Destroy(eventSystemStandalone);
#endif
#if UNITY_STANDALONE
        Destroy(touchpadReference);
        eventSystemStandalone.SetActive(true);
        Destroy(eventSystemAndroid);
        mobileButtons.Foreach(item=>{
            Destroy(item);
        });
        raycasteableImages.ForEach(item=>{
            item.raycastTarget=false;
        });
        raycasteableTexts.ForEach(item=>{
            item.raycastTarget=false;
        });
#endif
    }
}
