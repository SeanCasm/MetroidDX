﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenes : MonoBehaviour
{
    [SerializeField]Room rooms;
    private bool check;
    public static int currentScene;
    public void StartLoadingScene(int sceneIndex){
        SceneManager.LoadSceneAsync(sceneIndex,LoadSceneMode.Single);
    }
    /// <summary>
    /// Load async the next scene, and when his loaded the current scene is unloaded.
    /// </summary>
    /// <param name="actualScene"></param>
    /// <param name="nextScene"></param>
    public void UnloadCurrentScene(int actualScene,int nextScene){
        AsyncOperation operation=SceneManager.LoadSceneAsync(nextScene,LoadSceneMode.Additive);
        StartCoroutine(CheckUnload(operation,actualScene,nextScene));
    }
    /// <summary>
    /// Load async the scene 1 and unload the scene 0, only when starts new game.
    /// </summary>
    public void UnloadCurrentScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        StartCoroutine(CheckUnload(operation));
    }
    /// <summary>
    /// Wait to next scene be loaded in background, when it happens the current scene is unloaded.
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="actualScene">index of actual scene</param>
    /// <param name="nextScene">index of next scene</param>
    /// <returns></returns>
    IEnumerator CheckUnload(AsyncOperation operation,int actualScene,int nextScene){
        while(!operation.isDone){
            yield return null;
        }
        SceneManager.UnloadSceneAsync(actualScene);
    }
    IEnumerator CheckUnload(AsyncOperation operation)
    {
        while (!operation.isDone)
        {
            yield return null;
        }
        AsyncOperation unloadOperation=SceneManager.UnloadSceneAsync(0);
        while (!unloadOperation.isDone)
        {
            yield return null;
        }
        GameObject room=rooms.LoadRoom("Begin");
        room.name="Begin";
        Instantiate(room);
    }
    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single);
        Time.timeScale = 1f;
    }
}
