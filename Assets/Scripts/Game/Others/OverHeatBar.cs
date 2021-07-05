using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OverHeatBar : MonoBehaviour
{
    [SerializeField] GameObject overHeat;
    [SerializeField] float resizeHorBarOverTime, maxBarSize;
    private RectTransform progressHotBar;
    public static Action<float> SetFill;
    private float minHotProgress;
    private bool overHeated,sub;
     
    #region Unity methods
    void Awake()
    {
        progressHotBar = GetComponent<RectTransform>();
        minHotProgress = progressHotBar.sizeDelta.x;
    }
    private void OnEnable()
    {
        if(!sub)SetFill+=SetHot;
        sub=true;
        if (progressHotBar.sizeDelta.x > 0)
        {
            StartCoroutine(UnHotBar());
        }
    }
    private void OnDisable() {
        StopCoroutine(UnHotBar());
    }
    private void OnDestroy() {
        SetFill-=SetHot;
    }
    IEnumerator UnHotBar()
    {
        yield return new WaitForSeconds(1f);
        while (progressHotBar.sizeDelta.x > minHotProgress)
        {
            yield return new WaitForSeconds(0.1f);
            progressHotBar.sizeDelta = new Vector2(progressHotBar.sizeDelta.x - resizeHorBarOverTime, progressHotBar.sizeDelta.y);
        }
        progressHotBar.sizeDelta = new Vector2(minHotProgress, progressHotBar.sizeDelta.y);
        PlayerController.canInstantiate = true;
        overHeated = false;
        overHeat.SetActive(false);
    }
    #endregion
    
    /// <summary>
    /// Adds hot points to hot progress bar.
    /// </summary>
    /// <param name="hotPointsAmount">total hot points to add to the hot progress bar</param>
    public void SetHot(float hotPointsAmount)
    {
        if(!overHeated){
            progressHotBar.sizeDelta = new Vector2(progressHotBar.sizeDelta.x + hotPointsAmount, progressHotBar.sizeDelta.y);
            if (progressHotBar.sizeDelta.x >= maxBarSize)
            {
                overHeated = true;
                PlayerController.canInstantiate = false;
                progressHotBar.sizeDelta = new Vector2(maxBarSize, progressHotBar.sizeDelta.y);
                overHeat.SetActive(true);
                StartCoroutine(UnHotBar());
            }
        }
    }
    public void ResetHeatBar()
    {
        progressHotBar.sizeDelta = new Vector2(0, progressHotBar.sizeDelta.y);
    }
     
}