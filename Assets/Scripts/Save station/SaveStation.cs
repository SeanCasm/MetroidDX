using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class SaveStation : MonoBehaviour
{
    private SaveAndLoad saveLoad;
    private PlayerController playerController;
    private Animator anim;
    private int gameSlot;
    private GameObject player;
    private Rigidbody2D rb2d;
    public string actualSectorLoad;
    public static bool loaded;
    private void OnDisable() {
        loaded=false;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player") && !loaded)
        {
            Pause.onSave = Pause.onAnyMenu = true;
            GameEvents.save.Invoke(this);
            player = References.Player;
            rb2d = col.GetComponentInParent<Rigidbody2D>();
            playerController = col.GetComponentInParent<PlayerController>();
            saveLoad = col.GetComponentInParent<SaveAndLoad>();
            anim = col.GetComponentInParent<Animator>();
            playerController.ResetState();
            playerController.isGrounded=true;
            loaded=true;
            for(int i=0;i<3;i++){
                if(i==SaveAndLoad.slot)gameSlot=i;
            }
            OnStation();
        }
    }
    public void saveGame(bool optionSelect)
    {
        if (optionSelect)
        {
            saveLoad.SetPositions(transform.position.x, transform.position.y, transform.position.z);
            saveLoad.sectorName = actualSectorLoad;
            saveLoad.SavePlayerSlot(gameSlot);
            PlayerAnimatorUpdate(false, true);
            Invoke("stopSavingAnim",4f);
        }
        else
        {
            anim.SetBool("Saved", false);
            Pause.UnpausePlayer(playerController);
            unFreezeMoves();
        }
    }
    #region Private methods
    /// <summary>
    /// Set the player to the save station position, change his animation state and disable all possible movements.
    /// </summary>
    void OnStation()
    {
        anim.SetBool("Saved", true);
        player.transform.position = transform.position;
        player.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        PlayerController.canInstantiate = playerController.movement = false;
    }
    void stopSavingAnim()
    {
        GameEvents.saveMessage.Invoke();
        unFreezeMoves();
        PlayerAnimatorUpdate(false, false);
        PlayerController.canInstantiate = playerController.movement = true;
    }
    void unFreezeMoves()
    {
        rb2d.constraints = RigidbodyConstraints2D.None;
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        Pause.onAnyMenu = false;
    }
    void PlayerAnimatorUpdate(bool saved, bool isSaving)
    {
        anim.SetBool("Saved", saved);
        anim.SetBool("IsSaving", isSaving);
    }
    #endregion
}