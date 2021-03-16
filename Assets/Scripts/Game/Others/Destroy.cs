using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] float destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<AudioSource>() != null) Destroy(gameObject, GetComponent<AudioSource>().clip.length);
        else Destroy(gameObject, destroyTime);
    }
    public void DestroyPrefab()
    {
        Destroy(gameObject);
    }
}
