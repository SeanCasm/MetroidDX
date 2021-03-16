using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Enemy{
public class WallDetector : MonoBehaviour
{
    [SerializeField] LayerMask wallLayer;
    [SerializeField] float wallAware;
    private GroundChecker enemyFloorDetector;
    private void Awake() {
            enemyFloorDetector=GetComponentInChildren<GroundChecker>();
    }
    // Update is called once per frame
    void Update()
        {
            if (Physics2D.Raycast(transform.position, transform.right, wallAware, wallLayer))
            {
                enemyFloorDetector.Flip();
            }
        }
    }
}
 
