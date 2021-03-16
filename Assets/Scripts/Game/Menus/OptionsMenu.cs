using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]GameObject mainMenu;
    [SerializeField] Interactions menuFirst;
    public GameObject pauseMenu{get;set;}
    public bool fromMenuCalled { get; set; }
    public void Back()
    {
        if (fromMenuCalled)
        {
            mainMenu.SetActive(true);
            menuFirst.SetGameObjectToEventSystem(menuFirst.OptionMainMenu);
            Pause.onSlots=true;
        }
        else
        {
            pauseMenu.SetActive(true);
            menuFirst.SetGameObjectToEventSystem(menuFirst.PauseFirst);
        }
    }
}
