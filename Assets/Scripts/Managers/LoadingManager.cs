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
        UIManager.instance.ChangeScene(SceneNameToGameState(sceneName));
        canvas.enabled = false;
    }

    /// <summary>
    /// ���� �̸��� ������ �׿� �����ϴ� GameState�� ��ȯ�Ͽ� ��ȯ���ݴϴ�.
    /// </summary>
    /// <param name="sceneName"> �� �̸� </param>
    /// <returns> �Է��� �� �̸��� �����ϴ� GameState </returns>
    public GameState SceneNameToGameState(string sceneName)
    {
        switch (sceneName)
        {
            case "WorldScene":
            case "UITestScene":
                {
                    return GameState.World;
                }
            case "CombatScene":
                {
                    return GameState.Combat;
                }
        }

        Debug.LogError(sceneName + "�̶�� Scene�� ã�� �� �����ϴ�.");
        return GameState.World;
    }
    /// <summary>
    /// GameState�� �Է��ϸ� �׿� �����ϴ� ���� �̸��� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="gameState"> GameState </param>
    /// <returns> �Է��� GameState�� �����ϴ� �� �̸� </returns>
    public string GameStateToSceneName(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.World:
                {
                    return "WorldScene";
                }
            case GameState.Combat:
                {
                    return "CombatScene";
                }
        }
        return null;
    }
}
