using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class attached to map revealer GameObjects in scene.
/// </summary>
public class MapRevealer : MonoBehaviour
{
    [SerializeField]List<GameObject> roomMaps=new List<GameObject>();
    [SerializeField]Collider2D col2D;
    private int id;
    private bool active=true;
    private Animator animator,pAnim;
    private PlayerController pContr;
    private List<MiniMap> roomMap=new List<MiniMap>();
    public static List<int> mappers=new List<int>();
    private void Awake() {
        animator=GetComponent<Animator>();
        mappers.ForEach(item=>{
            if(item==id)active=false;
        });
        if(active)GetMiniMaps();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && active && other.IsTouching(col2D)){
            pAnim=other.GetComponentInParent<Animator>();
            pContr=other.GetComponentInParent<PlayerController>();
            SetPlayer();
            if(other.transform.position.x>transform.position.x)pContr.leftLook=true;
            else pContr.leftLook=false;
            PassMapData();
        }
    }
    private void GetMiniMaps(){
        roomMaps.ForEach(item=>{
            GameObject[] childs=item.GetChilds();
            for(int i=0;i<childs.Length;i++){
                roomMap.Add(childs[i].GetComponent<MiniMap>());
            }
        });
    }
    private void SetPlayer(){
        pContr.ResetState();
        pContr.IsGrounded=true;
        pContr.canInstantiate = pContr.movement=false;
        pContr.rb.bodyType=RigidbodyType2D.Static;
    }
    private void PassMapData(){
        animator.SetTrigger("Detected");
        var map=References.myMap;
        roomMap.ForEach(item=>{
            item.SetTile();
        });
    }
    /// <summary>
    /// Called in animation event.
    /// </summary>
    private void Completed(){
        pContr.canInstantiate = pContr.movement = true;
        pContr.rb.bodyType = RigidbodyType2D.Dynamic;
        pContr=null;
        pAnim=null;
        active=false;
    }
}
