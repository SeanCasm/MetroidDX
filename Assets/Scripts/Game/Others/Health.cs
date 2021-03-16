using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health<T> : MonoBehaviour
{
    [SerializeField] protected T health;
    public T MyHealth{get{return health;}set{health=value;}}
    protected SpriteRenderer _renderer;
    protected Rigidbody2D rb2d;
    protected Animator anim; 
}
