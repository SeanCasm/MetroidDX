using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAllPlayer : MonoBehaviour
{
    Behaviour[] behaviours;
    private Rigidbody2D rigid;
    // Start is called before the first frame update
    void Awake()
    {
        rigid =GetComponent<Rigidbody2D>();
        behaviours = GetComponents<Behaviour>();
    }

    public void EnablePlayer(){
        Utilities.SetBehaviours(behaviours,true);
        rigid.bodyType=RigidbodyType2D.Dynamic;
    }
}
