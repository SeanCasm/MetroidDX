using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolMovement : MonoBehaviour
{
    GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, 1f*Time.deltaTime);
    }
}
