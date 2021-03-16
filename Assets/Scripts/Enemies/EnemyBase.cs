using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy
{
    public class EnemyBase : MonoBehaviour
    {
        [Header("Enemy settings")]
        [SerializeField] protected float speed;
        [SerializeField] protected bool detectPlayer;
        protected EnemyHealth eh;
        protected Animator anim;
        protected Rigidbody2D rigid;
        protected PlayerDetect pDetect;
        protected void Awake()
        {
            if(detectPlayer)pDetect = GetComponentInChildren<PlayerDetect>();
            eh = GetComponentInChildren<EnemyHealth>();
            anim = GetComponent<Animator>();
            rigid = GetComponent<Rigidbody2D>();
        }
    }
}
 
