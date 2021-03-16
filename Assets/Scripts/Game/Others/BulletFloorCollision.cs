using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFloorCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag== "Suelo" || collision.tag== "EnemyBeam")
        {
        }
    }
}
