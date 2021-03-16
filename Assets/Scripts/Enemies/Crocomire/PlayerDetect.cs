using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetect : MonoBehaviour
{
    public bool detected;
    private GameObject player;
    private float playerPosX;
    public Collider2D detector{get;set;}
    public GameObject Player { get { return player; }set { player = value; } }
    private void Awake() {
        detector=GetComponent<Collider2D>();
    }
    public float PlayerPosX { get { return playerPosX; } }
    void OnTriggerEnter2D(Collider2D col) { 
        if (col.CompareTag("PlayerDetect"))
        {
            detected = true;
            player = col.gameObject;
            playerPosX = player.transform.position.x;
        }
    } 
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerDetect") )
        {
            detected = false;
            player = null;
        }
    }
}
