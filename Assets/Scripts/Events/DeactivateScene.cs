using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Deactivate the complete scene when player dies.
/// </summary>
public class DeactivateScene : MonoBehaviour
{
    private void OnEnable() {
        Retry.Start+=Deactivate;
    }
    private void OnDisable() {
        Retry.Start -= Deactivate;
    }
    private void Deactivate(){
        gameObject.SetActive(false);
    }
}
