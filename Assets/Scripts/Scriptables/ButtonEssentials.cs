using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName="new ButtonEssentials",menuName="ScriptableObjects/ButtonEssentials")]
public class ButtonEssentials : ScriptableObject
{
    public Color selected, unselected;
    public Sprite check, uncheck;
}
