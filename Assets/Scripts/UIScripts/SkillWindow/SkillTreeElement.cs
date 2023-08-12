using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ��ų Ʈ�� �ȿ��� ������ Skill��Ҹ� �����ϴ� ��ųƮ��UI�� ����� ������ Ŭ����
/// </summary>
public class SkillTreeElement : Generic.Singleton<SkillTreeElement>
{
    //���� �ʿ�
    //����� �ν����� â���� ��ų �ε����� ���ེų����UI�� �ϳ��ϳ� �������־�� �ϴµ�, �� ���� ����� ã�Ƽ� �����ؾ� �� ��
    public int skillIndex;
    [SerializeField] private GameObject[] _precedenceLine;  //�̸��� ȥ���ȴ�. �ش� �ڵ忡�� ����Ű�� ��ų�� �������� ��쿡 Ȱ��ȭ�ؾ��ϴ� ���ེų����UI��.
    //�ƿ� �̰� �� �ڵ�� �и��ؾ� ������...

    //1==����Ұ�, 2==���氡��, 3==����Ϸ� -> enum���� �����ؾ� �ϳ�?
    //SkillIcon �ڵ�� ��� ���� ����?
    [SerializeField] private Sprite[] _effect = new Sprite[3];
    private void Start()
    {
        for (int i = 0; i < _precedenceLine.Length; i++)
        {
            _precedenceLine[i].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        }
    }

    /// <summary>
    /// ��ųƮ��UI Ŭ�� �� ����˴ϴ�.
    /// skillUI���� ���� ��ųƮ��UI�� ����Ű�� ��ų ������ ���� ������ ����� ����� �����ϴ�.
    /// </summary>
    public void OnSkillUIBtnClick()
    {
        UIManager.instance.skillUI.ClickSkillUIButton(this.gameObject.transform, skillIndex);
    }

    /// <summary>
    /// ��ųƮ��UI�� ����(���� ����, ���� �Ұ�, ���� �Ϸ�)�� �����մϴ�.
    /// skillUI���� ��ųƮ��UI���� ���¸� ������ �� ����˴ϴ�.
    /// </summary>
    /// <param name="state"> ��ųƮ��UI�� ���� </param>
    public void SetSkillButtonEffect(int state) 
    {
        this.GetComponent<Image>().sprite = _effect[state];
    }
    /// <summary>
    /// ��ųƮ��UI�� ����� "�ش� ��ųƮ��UI�� ����Ű�� ��ų�� ����߸� Ȱ��ȭ�Ǵ� ���ེų����UI"���� ���¸� �����մϴ�.
    /// </summary>
    public void SetSkillArrow()
    {
        for (int i = 0; i < _precedenceLine.Length; i++)
        {
            _precedenceLine[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
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
