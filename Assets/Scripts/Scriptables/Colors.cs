using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="new Colors",menuName ="ScriptableObjects/Colors")]
public class Colors : ScriptableObject
{
    public Color itemButtonSelect,itemButtonUnselect;
    public Color bossQuarterHealth, bossHalfHealth, bossLowHealth;
}
