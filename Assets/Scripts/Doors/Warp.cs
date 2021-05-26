using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
public class Warp : MonoBehaviour
{
    [SerializeField] bool unloadCurrentScene,onlyLoadSaveRoom;
    [SerializeField] int nextSceneIndex;
    [SerializeField] AssetReference nextzone;
    private float playerYPoint,exitXPoint;
    private GameObject currentZone,nextRoom;
    private PlayerController playerC;

    void Start()
    {
        exitXPoint = transform.GetChild(transform.childCount-1).position.x;
        if (!unloadCurrentScene)currentZone = transform.parent.gameObject;
        nextzone.LoadAssetAsync<GameObject>().Completed += OnLoadDone;
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
            GameEvents.doorTransition.Invoke(true);
            SetPause();
        }
    }
    void SetPause()
    {
        playerC.gameObject.transform.position = new Vector2(exitXPoint, playerYPoint);
        StartCoroutine("Resume");
        Pause.PausePlayer(playerC);
    }
    IEnumerator Resume()
    {
        if (unloadCurrentScene)
        {
            SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Additive);

        }else Instantiate(nextRoom);

        yield return new WaitForSecondsRealtime(1.5f);
        Parallax.clearList.Invoke();
        if(!unloadCurrentScene) Destroy(currentZone);
        GameEvents.doorTransition.Invoke(false);
        Pause.UnpausePlayer(playerC);
    }
}
