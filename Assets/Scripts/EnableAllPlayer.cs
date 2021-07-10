using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAllPlayer : MonoBehaviour
{
    Behaviour[] behaviours;
    private Rigidbody2D rigid;
    
    // Start is called before the first frame update
    void Start()
    {
        rigid =GetComponent<Rigidbody2D>();
        behaviours = GetComponents<Behaviour>();
    }
    private void OnEnable() {
        GameEvents.enablePlayer+=Enable;
    }
    private void Enable(){
        Utilities.SetBehaviours(behaviours,true);
        rigid.bodyType=RigidbodyType2D.Dynamic;
    }
    private void OnDestroy() {
        GameEvents.enablePlayer -= Enable;

    }
}
