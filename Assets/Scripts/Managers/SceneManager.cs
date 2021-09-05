using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Manager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : MonoBehaviour
{

    public void ReloadCurrentScene()
    {
        Manager.LoadScene(Manager.GetActiveScene().name);
    }

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
        Manager.LoadScene(sceneName);
    }

    public void LoadSceneAsyncro(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private void Update() {
        //TODO : swith input
        // for (int i = 0; i != 12; i++) {
        //     if (Input.GetKeyDown(KeyCode.F1 + i)) {
        //         LoadScene($"Level {i + 1}");
        //     }
        // }
    }
}
