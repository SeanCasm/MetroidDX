using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    [SerializeField] Collider2D detector;
    public bool detected{get;private set;}
    private GameObject player;
    private float playerPosX;
    public GameObject Player { get { return player; }set { player = value; } }
    public System.Action Detected,Out;
    public float PlayerPosX { get { return playerPosX; } }
    void OnTriggerEnter2D(Collider2D col) { 
        if (col.CompareTag("Player"))
        {
            Detected?.Invoke();
            detected = true;
            player = col.gameObject;
            playerPosX = player.transform.position.x;
        }
    } 
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {
            Out?.Invoke();
            detected = false;
            player = null;
        }
    }
}
