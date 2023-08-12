using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ų �������� ǥ���ϴ� UI ����� ������ Ŭ����
/// ��ųâ UI �� ĳ����â UI ��ο� ����Ͽ�,
/// ��ųâ UI������ SkillTreeElement�� ���� Ŭ�����μ� ����ϰ�
/// ĳ����â UI������ �ܼ��� �̹��� ǥ�� ��ɸ��� �����Ѵ�.
/// ���� �̱���.
/// </summary>
public class SkillIcon : MonoBehaviour
{
    private int _skillIndex;

    /// <summary>
    /// ��ų �������� � ��ų�� ǥ���� �� �����մϴ�.
    /// </summary>
    /// <param name="index"> ��ų ������ȣ </param>
    public void GetSkillIndex(int index) 
    {
        _skillIndex = index;
        //FindIconImage();
    }

    private void FindIconImage()
    {
        SkillManager skillManager = SkillManager.instance;
        Sprite sprite = Resources.Load("Images/" + skillManager.GetSkill(_skillIndex).skillInfo.iconNumber) as Sprite;
        this.GetComponent<Image>().sprite = sprite;
    }
    /// <summary>
    /// ��ų �������� ���콺������ �� ����˴ϴ�.
    /// �ൿ���ù�ư ������ �� ��� ��� Ȱ���ؼ� �ٽ� �ۼ��� ����.
    /// </summary>
    public void OnSkillUIButtonOver()
    {
        //_uiManager._skillUI.ClickSkillUIButton(this.gameObject.transform, skillIndex);
    }
}
