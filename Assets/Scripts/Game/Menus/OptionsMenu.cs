using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField]GameObject mainMenu;
    [SerializeField] MenuPointer menuPointer;
    [SerializeField] Interactions menuFirst;
    public GameObject pauseMenu{get;set;}
    public bool fromMenuCalled { get; set; }
    public void Back()
    {
        if (fromMenuCalled)
        {
            mainMenu.SetActive(true);
            menuFirst.SetMainMenuSettingsFirst();
            menuPointer.SetCurrentMenu("main");
            menuPointer.SetCurrentPointerPosition(3);
            Pause.onSlots=true;
        }
        else
        {
            menuPointer.SetCurrentMenu("pause");
            menuPointer.SetCurrentPointerPosition(1);
            pauseMenu.SetActive(true);
            menuFirst.SetPauseSettingsFirst();
        }
    }
}
