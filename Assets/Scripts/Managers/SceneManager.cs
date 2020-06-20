﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Manager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = Manager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }
}
