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
           if(player.Screwing){
                anim.SetBool("Disable", true);
            }else if(player.HyperJumping){
                anim.SetBool("Disable", true);
            }
       }else{
           CheckCollisionTag(other);
       }
    }
}
