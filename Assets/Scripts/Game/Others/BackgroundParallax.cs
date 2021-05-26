using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Put on backgrounds
/// </summary>
public class BackgroundParallax : MonoBehaviour
{
    private void Awake() {
        print("2");
        GameEvents.parallax.Invoke(transform);

    }
}
