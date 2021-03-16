using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] protected int iD;
    [SerializeField] protected Colors bossColors;
    public List<Sensor> roomDoors;
    protected bool quarterReached, halfReached, lowReached;
    public int ID { get; }
    public static List<int> defeateds { get; set; } = new List<int>();
    protected void Start()
    {
        quarterReached =halfReached=lowReached = false;
        foreach (Sensor element in roomDoors)
        {
            element.BossDoor = true;
        }
         
    }
    protected void OnDestroy()
    {
        foreach (Sensor element in roomDoors)
        {
            element.BossDoor = false;
        }
        defeateds.Add(iD);
    }
}
