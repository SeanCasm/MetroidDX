using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Blocks;
public class HiteableBlock : DestroyableBlock
{
    new  void Start() {
        base.Start();
    }
    new void Awake() {
       base.Awake(); 
    }
    new void OnTriggerEnter2D(Collider2D other) {
       if(other.CompareTag("Player")){
           PlayerController player=other.GetComponentInParent<PlayerController>();
           if((player.screwSelected && player.OnRoll) && blockType==BlockType.screwAttack){
                anim.SetTrigger("Destroy");
            }else if(player.HyperJumping && blockType==BlockType.speedBooster){
                anim.SetTrigger("Destroy");
            }
       }else{
           base.CheckCollisionTag(other);
       }
    }
}
