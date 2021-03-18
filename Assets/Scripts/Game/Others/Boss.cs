using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] protected int iD;
    [SerializeField] protected Colors bossColors;
    public List<Sensor> roomDoors= new List<Sensor>();
    protected bool quarterReached, halfReached, lowReached;
    public int ID { get=>iD; }
    public static List<int> defeateds { get; set; } = new List<int>();
    protected void Start()
    {
        quarterReached =halfReached=lowReached = false;
    }
    public void SetDoors(){
        if(roomDoors.Count>0){
            foreach (Sensor element in roomDoors)
            {
                element.BossDoor = true;
            }
        }
    }
    public void OnDeath()
    {
        foreach (Sensor element in roomDoors)
        {
            element.BossDoor = false;
        }
        defeateds.Add(iD);
    }
}
