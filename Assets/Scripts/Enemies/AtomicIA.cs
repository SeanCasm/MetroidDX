using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enemy;
public class AtomicIA : EnemyBase
{
    [Tooltip("Distance between player and this enemy, to go towards the player")]
    [SerializeField] float distance;
    GameObject player;
    private void Start() {
        player=References.Player;
    }
    private void Update() {
        if(Vector2.Distance(transform.position,player.transform.position)<=distance){
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed*Time.deltaTime);
        }
    }
}
