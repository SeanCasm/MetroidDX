using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warp : MonoBehaviour
{
    [SerializeField] bool unloadCurrentScene,onlyLoadSaveRoom;
    [SerializeField] int nextSceneIndex;
    [SerializeField] GameObject nextzone;
    private float playerYPoint,exitXPoint;
    private GameObject currentZone;
    private PlayerController playerC;

    void Start()
    {
        exitXPoint = transform.GetChild(transform.childCount-1).position.x;
        if (!unloadCurrentScene)currentZone = transform.parent.gameObject;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerDetect")){
            playerC = other.GetComponentInParent<PlayerController>();
            playerYPoint=playerC.transform.position.y;
            DoorTransition.Transition=true;
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

        }else Instantiate(nextzone);

        yield return new WaitForSecondsRealtime(1.5f);
         
        if(!unloadCurrentScene) Destroy(currentZone);
        DoorTransition.Transition = false;
        Pause.UnpausePlayer(playerC);
    }
}
