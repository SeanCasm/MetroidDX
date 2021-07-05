using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// Class attached to map revealer GameObjects in scene.
/// </summary>
public class MapUpdater : MonoBehaviour
{
    [SerializeField] MinimapTilesMapUpdater minimapTiles;
    [SerializeField] Collider2D col2D;
    [SerializeField] GameObject acqPanel;
    [SerializeField] Animator light;
    private int id;
    TextMeshProUGUI panelText;
    private bool active = true;
    private Animator animator, pAnim;
    private PlayerController pContr;
    public static List<int> mappers = new List<int>();
    private void Awake()
    {
        animator = GetComponent<Animator>();
        mappers.ForEach(item =>
        {
            if (item == id)
            {
                active = false;
                ClearReferences();
                return;
            }
        });
        if(!active)minimapTiles=null;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && active && other.IsTouching(col2D))
        {
            pAnim = other.GetComponentInParent<Animator>();
            pContr = other.GetComponentInParent<PlayerController>();
            SetPlayer();
            if (other.transform.position.x > transform.position.x) pContr.leftLook = true;
            else pContr.leftLook = false;
            PassMapData();
        }
    }

    private void SetPlayer()
    {
        pContr.ResetState();
        pContr.IsGrounded = true;
        PlayerController.canInstantiate = pContr.movement = false;
        pContr.rb.bodyType = RigidbodyType2D.Static;
    }
    private void PassMapData()
    {
        animator.SetTrigger("Detected");
        light.SetTrigger("Activated");
        var map = References.myMap;
        minimapTiles.minimapScripts.ForEach(item =>
        {
            item.SetTile();
        });
        minimapTiles=null;
    }
    /// <summary>
    /// Called in animation event.
    /// </summary>
    private void Completed()
    {
        var obj = Instantiate(acqPanel, References.Canvas.position, Quaternion.identity, References.Canvas);
        panelText = obj.GetChild(0).GetComponent<TextMeshProUGUI>();
        panelText.text = "Map update completed";
        Destroy(obj, 1f);
        Invoke("TransitionToMinimap", 1f);
    }
    private void ClearReferences()
    {
        panelText = null;
        acqPanel = null;
    }
    private void TransitionToMinimap(){
        float time=GameEvents.StartTransition.Invoke();
        Invoke("EnablePlayer",time/2);
    }
    private void EnablePlayer()
    {
        panelText.text = "";
        ClearReferences();
        PlayerController.canInstantiate = pContr.movement = true;
        pContr.rb.bodyType = RigidbodyType2D.Dynamic;
        pContr = null;
        pAnim = null;
        active = false;
        GameEvents.MinimapShortcout.Invoke();
    }
}
