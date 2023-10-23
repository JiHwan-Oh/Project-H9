using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ų Ʈ�� �ȿ��� ������ Skill��Ҹ� �����ϴ� ��ųƮ��UI�� ����� ������ Ŭ����
/// </summary>
public class SkillTreeElement : UIElement
{
    //���� �ʿ�
    //����� �ν����� â���� ��ų �ε����� ���ེų����UI�� �ϳ��ϳ� �������־�� �ϴµ�, �� ���� ����� ã�Ƽ� �����ؾ� �� ��

    public int skillIndex;
    [SerializeField] private GameObject[] _precedenceLine;  //�̸��� ȥ���ȴ�. �ش� �ڵ忡�� ����Ű�� ��ų�� �������� ��쿡 Ȱ��ȭ�ؾ��ϴ� ���ེų����UI��.
                                                            //�ƿ� �̰� �� �ڵ�� �и��ؾ� ������...

    [SerializeField] private Image _effectImage;
    [SerializeField] private Image _ButtonImage;
    [SerializeField] private Image _SkillImage;

    private SkillIcon _skillIcon;
    private void Start()
    {
        _skillIcon = _SkillImage.GetComponent<SkillIcon>();
        _skillIcon.SetSkillIndex(skillIndex);
        for (int i = 0; i < _precedenceLine.Length; i++)
        {
            _precedenceLine[i].transform.GetChild(0).GetComponent<Image>().color = new Color32(199, 94, 8, 255);
        }
    }

    /// <summary>
    /// ��ųƮ��UI Ŭ�� �� ����˴ϴ�.
    /// skillUI���� ���� ��ųƮ��UI�� ����Ű�� ��ų ������ ���� ������ ����� ����� �����ϴ�.
    /// </summary>
    public void OnSkillUIBtnClick()
    {
        UIManager.instance.skillUI.ClickSkillUIButton(GetComponent<RectTransform>().position, skillIndex);
    }

    /// <summary>
    /// ��ųƮ��UI�� ����(���� ����, ���� �Ұ�, ���� �Ϸ�)�� �����մϴ�.
    /// skillUI���� ��ųƮ��UI���� ���¸� ������ �� ����˴ϴ�.
    /// </summary>
    /// <param name="state"> ��ųƮ��UI�� ���� </param>
    public void SetSkillButtonEffect(int state) 
    {
        Color32[] effectColor =
        {
            UICustomColor.SkillIconNotLearnedColor,
            UICustomColor.SkillIconLearnableColor,
            UICustomColor.SkillIconLearnedColor
        };
        _effectImage.color = effectColor[state];
    }
    /// <summary>
    /// ��ųƮ��UI�� ����� "�ش� ��ųƮ��UI�� ����Ű�� ��ų�� ����߸� Ȱ��ȭ�Ǵ� ���ེų����UI"���� ���¸� �����մϴ�.
    /// </summary>
    public void SetSkillArrow()
    {
        for (int i = 0; i < _precedenceLine.Length; i++)
        {
            _precedenceLine[i].transform.GetChild(0).GetComponent<Image>().color = UICustomColor.SkillIconLearnedColor;
        }
    }
    /// <summary>
    /// ��ųƮ���� ����Ű�� �ִ� ��ų�� ������ȣ�� ��ȯ�Ѵ�.
    /// </summary>
    /// <returns> ��ųƮ���� ����Ű�� �ִ� ��ų�� ������ȣ </returns>
    public int GetSkillUIIndex() 
    {
        return skillIndex;
    }
}
