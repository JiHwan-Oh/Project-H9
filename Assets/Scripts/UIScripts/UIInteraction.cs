using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���� �� Ŭ������ �̿��ؼ� UI �Է��� ó���� �ٸ� ��ȹ�� �־����� ���Ǿ ��ǻ� �ʿ���� �ڵ��Դϴ�.
//�׳��� UIManager�� �ϴ� ���� ���� ������ UIManager�Է¸� ���� ó���ϴ� �뵵? �����θ� �� �� ���� ��.
//�������� �Լ����� �ٸ� ��ġ�� �̰��ϰ� ������ ����.
/// <summary>
/// ���� UI ��ȣ�ۿ��� ó���ϴ� Ŭ����
/// </summary>
public class UIInteraction : Generic.Singleton<UIInteraction>
{

    private static UIManager _uiManager;

    private void Start()
    {
        _uiManager = UIManager.instance;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnCharacterBtnClick();
        }
    }
    public void OnCharacterBtnClick()
    {
        _uiManager.SetCharacterCanvasState(true);
        _uiManager.SetSkillCanvasState(false);
        _uiManager.SetPauseMenuCanvasState(false);
    }
    public void OnSkillBtnClick()
    {
        _uiManager.SetCharacterCanvasState(false);
        _uiManager.SetSkillCanvasState(true);
        _uiManager.SetPauseMenuCanvasState(false);
    }
    public void OnPauseMenuBtnClick()
    {
        _uiManager.SetCharacterCanvasState(false);
        _uiManager.SetSkillCanvasState(false);
        _uiManager.SetPauseMenuCanvasState(true);
    }
    public void OnBackgroundBtnClick()
    {
        _uiManager.SetCharacterCanvasState(false);
        _uiManager.SetSkillCanvasState(false);
        _uiManager.SetPauseMenuCanvasState(false);
    }
    public void OnSkillTooltipCloseBtnClick()
    {
        _uiManager.skillUI.CloseSkillTooltip();
    }
    public void OnSkillTooltipLearnBtnClick()
    {
        _uiManager.skillUI.ClickLearnSkill();
    }

    public void OnExitBtnClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnWeaponItemUIBtnClick()
    {
        _uiManager.characterUI.itemListUI.ChangeItemUIStatus(ItemInfo.ItemCategory.Weapon);
    }
    public void OnUsableItemUIBtnClick()
    {
        _uiManager.characterUI.itemListUI.ChangeItemUIStatus(ItemInfo.ItemCategory.Usable);
    }
    public void OnOtherItemUIBtnClick()
    {
        _uiManager.characterUI.itemListUI.ChangeItemUIStatus(ItemInfo.ItemCategory.Other);
    }
}
