using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �޴� ��ư�� ������ �� ǥ���� ���� UI�� �����ϴ� Ŭ����
/// </summary>
public class PauseMenuUI : UISystem
{
    public void OnResumeBtnClick()
    {
        UIManager.instance.SetUILayer(1);
    }
    
    public void BackToTitle()
    {
        //Find all GameObjects with DontDestroyOnLoad
        foreach (GameObject obj in FindObjectsOfType<GameObject>())
        {
            Destroy(obj);
        }
        
        SceneManager.LoadScene($"TitleScene");
    }
    
    public void ExitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        
        #else
        Application.Quit();
        
        #endif
    }
}
