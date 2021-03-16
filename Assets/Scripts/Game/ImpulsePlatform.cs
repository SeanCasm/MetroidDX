using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpulsePlatform : MonoBehaviour
{
    [SerializeField]PointEffector2D pointEffector2D;
    float yPosition;
    void Awake(){
        yPosition=transform.GetY();
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") && yPosition<other.transform.GetY()){
            pointEffector2D.enabled=true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")){
            pointEffector2D.enabled=false;
        }
    }
}
