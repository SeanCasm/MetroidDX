using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPrefabs : MonoBehaviour
{
    public GameObject soundClip;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(soundClip);
    }
 
}
