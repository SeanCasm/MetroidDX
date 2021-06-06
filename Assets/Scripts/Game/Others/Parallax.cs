using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private List<Transform> backgrounds=new List<Transform>();
    public float smoothing=1;
    private List<float> parallaxs=new List<float>();
    public static System.Action clearList;
    [SerializeField] Transform cam;
    private Vector2 camPos;
    private void OnEnable() {
        GameEvents.parallax+=PutBackgroundOnParallaxList;
        clearList+=ClearBackGroundsList;
    }
    private void OnDisable() {
        GameEvents.parallax -= PutBackgroundOnParallaxList;
        clearList -= ClearBackGroundsList;
    }
    private void PutBackgroundOnParallaxList(Transform bg)
    {
        backgrounds.Add(bg);
        camPos = cam.position;
        parallaxs.Add(backgrounds[backgrounds.Count-1].position.z*-1);
        print("3");
    }
    private void ClearBackGroundsList(){
        backgrounds.Clear();
        parallaxs.Clear();
    }
    private void Awake() {
        cam=Camera.main.transform;
    }
    
    private void FixedUpdate() {
        for (int i = 0; i < backgrounds.Count; i++)
        {
            float parallax = (camPos.x-cam.position.x) * parallaxs[i];
            float targetBackgroundX=backgrounds[i].position.x+parallax;
            Vector3 targetBackgroundP=new Vector3(targetBackgroundX,backgrounds[i].position.y,backgrounds[i].position.z);
            backgrounds[i].position=Vector3.Lerp(backgrounds[i].position,targetBackgroundP,smoothing*Time.deltaTime);
        }
        camPos=cam.position;
    }
    
}
