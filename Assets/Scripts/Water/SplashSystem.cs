using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashSystem : MonoBehaviour
{
    public GameObject splashSystem;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Destroy(Instantiate(splashSystem,col.transform.position,Quaternion.identity),4f);
        }
    }
}
