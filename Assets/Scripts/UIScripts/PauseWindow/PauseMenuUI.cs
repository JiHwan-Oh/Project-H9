using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// �޴� ��ư�� ������ �� ǥ���� ���� UI�� �����ϴ� Ŭ����
/// </summary>
public class PauseMenuUI : UISystem
{
    public OptionUI optionUI { get; private set; }
    [SerializeField] private LoadUI _loadPanel;

    private void Awake()
    {
        optionUI = GetComponent<OptionUI>();

        //uiSubsystems.Add(optionUI);
    }
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
    public void OnOptionBtnClick() 
    {
        optionUI.OpenUI();
    }
    public override void CloseUI()
    {
        base.CloseUI();
        optionUI.CloseUI();
        _loadPanel.CloseUI();
        if (optionUI.isOpened) base.CloseUI();
    }

    public void OnOpenLoadUIClick()
    {
        _loadPanel.OpenUI();
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
