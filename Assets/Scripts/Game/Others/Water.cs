using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : Fluid
{
   // private AudioSource audioPlayer;
    //public float damageCall;
    new void OnTriggerEnter2D(Collider2D col)
    {
         base.OnTriggerEnter2D(col);
    }
    new void OnTriggerStay2D(Collider2D other) {
       base.OnTriggerStay2D(other); 
    }
    new void OnTriggerExit2D(Collider2D col){
        base.OnTriggerExit2D(col);
    }
}
