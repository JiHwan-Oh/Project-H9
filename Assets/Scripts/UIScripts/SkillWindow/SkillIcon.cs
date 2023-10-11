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
        Sprite sprite = Resources.Load("Images/" + skillManager.GetSkill(_skillIndex).skillInfo.iconIndex) as Sprite;
        if (sprite == null) 
        {
            sprite = defaultImage;
        }
        this.GetComponent<Image>().sprite = sprite;
    }
}
