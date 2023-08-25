using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : Generic.Singleton<LoadingManager>
{
    private new void Awake()
    {
        base.Awake();
        
        canvas = GetComponentInChildren<Canvas>(includeInactive:true);
        progress = GetComponentInChildren<Slider>(includeInactive:true);
    }

    private void Start()
    {
        canvas.enabled = false;
    }

    public Canvas canvas;
    public Slider progress;

    public void LoadingScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    
    private IEnumerator LoadSceneCoroutine(string sceneName)
    {
        SceneManager.LoadScene("LoadingScene");
        
        yield return null;
        canvas.enabled = true;
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;

            if (progress.value < 1f)
            {
                
                progress.value = operation.progress;
            }

            operation.allowSceneActivation = true;
        }

        canvas.enabled = false;
    }
}
