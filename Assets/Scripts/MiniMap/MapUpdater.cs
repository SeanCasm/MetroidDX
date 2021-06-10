using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
/// <summary>
/// Class attached to map revealer GameObjects in scene.
/// </summary>
public class MapUpdater : MonoBehaviour
{
    [SerializeField] List<GameObject> roomMaps = new List<GameObject>();
    [SerializeField] Collider2D col2D;
    [SerializeField] GameObject acqPanel;
    [SerializeField] Animator light;
    private int id;
    TextMeshProUGUI panelText;
    private bool active = true;
    private Animator animator, pAnim;
    private PlayerController pContr;
    private List<MiniMap> miniMapRef = new List<MiniMap>();
    public static List<int> mappers = new List<int>();
    private void Awake()
    {
        animator = GetComponent<Animator>();
        mappers.ForEach(item =>
        {
            if (item == id)
            {
                active = false;
                roomMaps.Clear();
                ClearReferences();
                return;
            }
        });
        if (active) GetMiniMaps();
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
    private void GetMiniMaps()
    {
        roomMaps.ForEach(item =>
        {
            GameObject[] childs = item.GetChilds();
            for (int i = 0; i < childs.Length; i++)
            {
                miniMapRef.Add(childs[i].GetComponent<MiniMap>());
            }
        });
        roomMaps.Clear();
    }
    private void SetPlayer()
    {
        pContr.ResetState();
        pContr.IsGrounded = true;
        pContr.canInstantiate = pContr.movement = false;
        pContr.rb.bodyType = RigidbodyType2D.Static;
    }
    private void PassMapData()
    {
        animator.SetTrigger("Detected");
        light.SetTrigger("Activated");
        var map = References.myMap;
        miniMapRef.ForEach(item =>
        {
            item.SetTile();
        });
        miniMapRef.Clear();

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
        pContr.canInstantiate = pContr.movement = true;
        pContr.rb.bodyType = RigidbodyType2D.Dynamic;
        pContr = null;
        pAnim = null;
        active = false;
        GameEvents.MinimapShortcout.Invoke();
    }
}
