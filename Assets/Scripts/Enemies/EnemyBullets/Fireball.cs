﻿using System.Collections;
using System.Collections.Generic;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Enemy.Weapons;
public class Fireball : Weapon
{
    [SerializeField]private GameObject groundFireBall;
    new void Start()
    {
        base.Start();
    }
    new void Awake()
    {
        base.OnEnable();
        player=References.Player.transform;
        transform.SetParent(null);
        base.SetDirectionAndRotation();
    }
    
    new void FixedUpdate()
    {
        base.FixedUpdate();
    }
    new void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Suelo"))
        {
            GameObject gF=Instantiate(groundFireBall, transform.GetChild(0).transform.position, Quaternion.identity,null);
            Destroy(gameObject);
        }
    }

}
