using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBoss : MonoBehaviour
{
    [SerializeField] GameObject bossToSpawn;
    [SerializeField]Sensor[] doors;
    [SerializeField]PathCreation.PathCreator ridleyPath;
    void Awake()
    {
        Boss bossComponenet=bossToSpawn.GetComponent<Boss>();
        int iD = bossComponenet.ID;
        if (!Boss.defeateds.Contains(iD))
        {
            Instantiate(bossToSpawn,transform.position,Quaternion.identity);
            foreach(Sensor s in doors){
                bossComponenet.roomDoors.Add(s);
            }
            if(iD==1){//ridley
                PathCreation.Examples.PathFollower ridleyPathFollower=bossToSpawn.AddComponent<PathCreation.Examples.PathFollower>();
                ridleyPathFollower.speed=1.2f;
                ridleyPathFollower.endOfPathInstruction=PathCreation.EndOfPathInstruction.Loop;
                ridleyPathFollower.pathCreator=ridleyPath;
            }
        }
    }
 
}
