using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseAndroid : MonoBehaviour
{
   /* public static bool gamePaused = false, onAnyMenu;
    public GameObject pauseMenuUi;
    public GameObject player;
    public GameObject globalGame;
    public GameObject HUD;
    public SaveAndLoad reload;
    public GameObject playerMenu, itemsMenu, settingsMenu, tripleMenu,androidControlScreen;
    private PlayerController playerC;
    public Retry retry;
    private bool acquiredPause;
    private PlayerHealth playerHealth;
    public bool AcquiredPause { set { acquiredPause = value; } }

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = player.GetComponentInChildren<PlayerHealth>();
        playerC = player.GetComponent<PlayerController>();
        acquiredPause = false;
    }
    public void Pause()
    {
        if (!acquiredPause && !playerHealth.DiePaused && player.activeSelf)
        {
            if (gamePaused && !itemsMenu.activeSelf)
            {
                Resume();
            }
            else
            {
                Pausee();
            }
        }
    }
    public void MenuPause()
    {
        if (!acquiredPause && !player.GetComponentInChildren<PlayerHealth>().DiePaused && player.activeSelf)
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                EnterPause();
            }
        }
    }
    public void Resume()
    {
        androidControlScreen.SetActive(true);
        HUD.gameObject.SetActive(true);
        playerMenu.SetActive(false);
        settingsMenu.SetActive(false);
        itemsMenu.SetActive(false);
        tripleMenu.SetActive(true);
        pauseMenuUi.SetActive(false);
        playerC.movement = true;
        Time.timeScale = 1f;
        gamePaused = false;
    }
    void GeneralPause()
    {
        androidControlScreen.SetActive(false);
        HUD.gameObject.SetActive(false);
        playerC.movement = false;
        playerC.rigidBody.velocity = Vector2.zero;
        Time.timeScale = 0f;
        gamePaused = true;
    }
    void EnterPause()
    {
        GeneralPause();
        playerMenu.SetActive(true);
    }
    void Pausee()
    {
        GeneralPause();
        pauseMenuUi.SetActive(true);
    }
    public void reloadState()
    {
        if (reload.getLoadSlot1())
        {
            reload.LoadPlayerSlot1();
        }
        else if (reload.getLoadSlot2())
        {
            reload.LoadPlayerSlot2();
        }
        else if (reload.getLoadSlot3())
        {
            reload.LoadPlayerSlot3();
        }
        Resume();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
   */
}
