using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallWalking : MonoBehaviour
{
    #region Properties
    [SerializeField] float speed;
    [SerializeField] float floorAware;
    [SerializeField] LayerMask groundLayer;
    private Transform floorCorner;
    private Rigidbody2D _rigidbody;
    private Vector2 velocity;
    #endregion
    #region Unity Methods
    void Awake()
    {
        floorCorner = transform.GetChild(0);
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Physics2D.Raycast(transform.position, transform.right, floorAware, groundLayer))
        {
            Rotate(90f);
        }else 
        if (!Physics2D.Raycast(transform.position, -transform.up, floorAware, groundLayer))
        {
            transform.position = floorCorner.position;
            Rotate(-90f);
        }
    }
    private void FixedUpdate()
    {
        _rigidbody.velocity = transform.right*Time.deltaTime*speed;
    }
    #endregion
    #region Private Methods
    private void Rotate(float zDegrees)
    {
        transform.Rotate(0, 0, zDegrees);
        
        switch (transform.eulerAngles.z)
        {
            case -90: transform.eulerAngles=new Vector3(0,0,270);
                break;
            case -180: transform.eulerAngles = new Vector3(0, 0,180);
                break;
            case -270: transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case -360: transform.eulerAngles = new Vector3(0, 0, 0);
                break;
        }
    }
    #endregion
}
