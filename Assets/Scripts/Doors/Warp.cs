using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Warp : MonoBehaviour
{
    [SerializeField] AssetReference nextzone;
    private CameraTransition cameraTransition;
    private float playerYPoint;
    private Vector2 exit;
    private GameObject currentZone,nextRoom;
    public static System.Action OnWarp;
    private PlayerController playerC;

    void Start()
    {
        exit = transform.GetChild(transform.childCount-1).position;
        currentZone = transform.parent.gameObject;
        nextzone.LoadAssetAsync<GameObject>().Completed += OnLoadDone;
        if(transform.eulerAngles.z!=0){
            if(transform.eulerAngles.z<0)cameraTransition=CameraTransition.Up;
            else cameraTransition=CameraTransition.Down;
        }else{
            if(transform.localScale.x>0)cameraTransition=CameraTransition.Left;
            else cameraTransition=CameraTransition.Right;
        }
    }
    private void OnLoadDone(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<GameObject> obj)
    {
        nextRoom = obj.Result;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDetect")){
            playerC = other.GetComponentInParent<PlayerController>();
            playerYPoint=playerC.transform.position.y;
            GameEvents.DoorTransition.Invoke(cameraTransition);
            StartCoroutine("Resume");
            Pause.PausePlayer(playerC);
        }
    }
    IEnumerator Resume()
    {
        yield return new WaitForSecondsRealtime(.5f);
        playerC.gameObject.transform.position = new Vector2(exit.x, playerYPoint);
        transform.SetParent(null);
        currentZone.SetActive(false);
        Instantiate(nextRoom);
        yield return new WaitForSecondsRealtime(1f);
        OnWarp?.Invoke();
        Parallax.clearList.Invoke();
        Pause.UnpausePlayer(playerC);
        Destroy(currentZone);
        Destroy(gameObject);
    }
}
