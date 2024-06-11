using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ų �������� ǥ���ϴ� UI ����� ������ Ŭ����
/// ��ųâ UI �� ĳ����â UI ��ο� ����Ͽ�,
/// ��ųâ UI������ SkillTreeElement�� ���� Ŭ�����μ� ����ϰ�
/// ĳ����â UI������ �ܼ��� �̹��� ǥ�� ��ɸ��� �����Ѵ�.
/// </summary>
public class SkillIcon : UIElement
{
    private int _skillIndex;
    public Sprite defaultImage;

    /// <summary>
    /// ��ų �������� � ��ų�� ǥ���� �� �����մϴ�.
    /// </summary>
    /// <param name="index"> ��ų ������ȣ </param>
    public void SetSkillIndex(int index) 
    {
        _skillIndex = index;
        FindIconImage();
    }

    private void FindIconImage()
    {
        SkillManager skillManager = SkillManager.instance;
        Skill skill = skillManager.GetSkill(_skillIndex);
        if (skill == null) return;
        //Sprite sprite = Resources.Load("SkillIcon/" + skill.skillInfo.icon) as Sprite;
        Sprite sprite = skill.skillInfo.icon;
        if (sprite == null)
        {
            Debug.LogError(_skillIndex + "�� ��ų�� ��������Ʈ " + skill.skillInfo.icon + "�� ã�� �� �����ϴ�.");
            sprite = defaultImage;
        }
        this.GetComponent<Image>().sprite = sprite;
    }
}
