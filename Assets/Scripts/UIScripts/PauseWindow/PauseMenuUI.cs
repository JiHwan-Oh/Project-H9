using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �޴� ��ư�� ������ �� ǥ���� ���� UI�� �����ϴ� Ŭ����
/// </summary>
public class PauseMenuUI : UISystem
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnResumeBtnClick()
    {
        UIManager.instance.currentLayer = 1;
        UIManager.instance.SetCharacterCanvasState(false);
        UIManager.instance.SetSkillCanvasState(false);
        UIManager.instance.SetPauseMenuCanvasState(false);
    }
}
