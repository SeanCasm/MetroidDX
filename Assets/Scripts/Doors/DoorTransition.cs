using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class DoorTransition : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] Animator camAnimator;
    private SpriteRenderer spriteRenderer;
    private Transform player;
    private float ortSizeWidth;
    CameraTransition cameraTransition;
    private Animator animator;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        transform.localScale = new Vector3(100, 100, 0);
    }
    private void Start()
    {
        player = References.Player.transform;
    }
    private void OnEnable()
    {
        GameEvents.DoorTransition += StartTransition;
    }
    private void OnDisable()
    {
        GameEvents.DoorTransition -= StartTransition;
    }
    private void StartTransition(CameraTransition camTransition)
    {
        transform.position = player.position;
        virtualCamera.gameObject.SetActive(false);
        virtualCamera.Follow = null;
        ActualVirtualCam.CMConfiner.m_BoundingShape2D = null;
        cameraTransition = camTransition;
        animator.SetTrigger("Start");
        StartCoroutine(Lerp());
    }
    public void StopCoroutine()
    {
        StopAllCoroutines();
        virtualCamera.Follow = References.Player.transform;
        virtualCamera.transform.position=camAnimator.gameObject.transform.position;
        virtualCamera.gameObject.SetActive(true);
    }
    IEnumerator Lerp()
    {
        yield return new WaitForSecondsRealtime(.5f);
        switch (cameraTransition)
        {
            case CameraTransition.Left:
                camAnimator.SetTrigger("Left");
                break;
            case CameraTransition.Right:
                camAnimator.SetTrigger("Right");
                break;
            case CameraTransition.Up:
                camAnimator.SetTrigger("Up");
                break;
            case CameraTransition.Down:
                camAnimator.SetTrigger("Down");
                break;
        }
    }
}
