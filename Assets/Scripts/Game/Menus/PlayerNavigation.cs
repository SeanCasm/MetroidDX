using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerNavigation : MonoBehaviour
{
    [SerializeField] Button[] playerMenuButtons;
    private void OnEnable()
    {
        StartCoroutine(CheckIfSelected());
    }
    private void OnDisable(){
        StopAllCoroutines();
    }
    private IEnumerator CheckIfSelected()
    {
        while (gameObject.activeSelf)
        {
            foreach (Button element in playerMenuButtons)
            {
                if (EventSystem.current.currentSelectedGameObject == element.gameObject)
                {
                    element.onClick.Invoke();
                }
                yield return new WaitForSecondsRealtime(0.05f);
            }
        }
    }
}