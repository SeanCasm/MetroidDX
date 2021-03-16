using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballEventAnim : MonoBehaviour
{
    [SerializeField] GameObject fireBall, firePoint, blackHole;
    private bool stop;
    public void Fireball()
    {
        if (!stop)
        {
            GameObject fire = Instantiate(fireBall, firePoint.transform.position, Quaternion.identity);
            fire.transform.eulerAngles = new Vector2(0, transform.eulerAngles.y);
            fire.transform.SetParent(null);
            Invoke("StopLaunching", 0.4f);
        }
    }
    void StopLaunching()
    {
        stop = true;
        Invoke("StartLaunching", 0.35f);
    }
    void StartLaunching()
    {
        stop = false;
    }
    public void Ultimate()
    {
        if (!stop)
        {
            GameObject hole = Instantiate(blackHole, firePoint.transform.position, Quaternion.identity);
            hole.transform.SetParent(null);
            Invoke("StopLaunching", 1f);
        } 
    }
}
