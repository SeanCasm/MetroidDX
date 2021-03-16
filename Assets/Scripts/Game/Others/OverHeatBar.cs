using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OverHeatBar : MonoBehaviour
{
    [SerializeField] GameObject overHeat;
    [SerializeField] float resizeHorBarOverTime, maxBarSize;
    [SerializeField] PlayerController playerC;
    private RectTransform progressHotBar;
    private float minHotProgress;
    private bool overHeated;
     
    #region Unity methods
    void Awake()
    {
        progressHotBar = GetComponent<RectTransform>();
        minHotProgress = progressHotBar.sizeDelta.x;
    }
    
    private void OnEnable()
    {
        GameEvents.overHeatAction +=SetHot;
        if (progressHotBar.sizeDelta.x > 0)
        {
            StartCoroutine(UnHotBar());
        }        
    }
    private void OnDisable() {
        GameEvents.overHeatAction-=SetHot;
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
        playerC.canInstantiate = true;
        overHeated = false;
        overHeat.SetActive(false);
    }
    #endregion
    /// <summary>
    /// Add hot points to hot progress bar when player shot.
    /// </summary>
    /// <param name="hotPointsAmount">total hot points to add to the hot progress bar</param>
    public void SetHot(float hotPointsAmount)
    {
        progressHotBar.sizeDelta = new Vector2(progressHotBar.sizeDelta.x + hotPointsAmount, progressHotBar.sizeDelta.y);
        if (progressHotBar.sizeDelta.x >= maxBarSize)
        {
            overHeated = true;
            playerC.canInstantiate = false;
            progressHotBar.sizeDelta = new Vector2(maxBarSize, progressHotBar.sizeDelta.y);
            overHeat.SetActive(true);
            StartCoroutine("UnHotBar");
        }
        else if (!overHeated)
        {
            StopAllCoroutines();
            StartCoroutine("UnHotBar");
        }
    }
    public void ResetHeatBar()
    {
        progressHotBar.sizeDelta = new Vector2(0, progressHotBar.sizeDelta.y);
    }
     
}