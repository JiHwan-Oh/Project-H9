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

    //�� ���⼭ �����ϰ� ���� ������ ����� �ȳ���? ��?��
    [SerializeField] private Color32[] _effectColor =
    {
        new Color32(0, 0, 0, 255),
        new Color32(255, 201, 18, 255),
        new Color32(72, 219, 18, 255)
    };
    [SerializeField] private Image _effectImage;
    [SerializeField] private Image _ButtonImage;
    [SerializeField] private Image _SkillImage;

    //1==����Ұ�, 2==���氡��, 3==����Ϸ� -> enum���� �����ؾ� �ϳ�?
    //SkillIcon �ڵ�� ��� ���� ����?
    [SerializeField] private Sprite[] _effect = new Sprite[3];
    private void Start()
    {
        for (int i = 0; i < _precedenceLine.Length; i++)
        {
            _precedenceLine[i].transform.GetChild(0).GetComponent<Image>().color = new Color32(199, 94, 8, 255);
        }
    }

    private void Update()
    {
        if (_effectImage.color == Color.white) Debug.LogError("?????");
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
        Color32[] effectColor =
        {
        new Color32(0, 0, 0, 255),
        new Color32(255, 201, 18, 255),
        new Color32(72, 219, 18, 255)
        };
        Debug.Log(effectColor[state]);
        _effectImage.color = effectColor[state];
    }
    /// <summary>
    /// ��ųƮ��UI�� ����� "�ش� ��ųƮ��UI�� ����Ű�� ��ų�� ����߸� Ȱ��ȭ�Ǵ� ���ེų����UI"���� ���¸� �����մϴ�.
    /// </summary>
    public void SetSkillArrow()
    {
        for (int i = 0; i < _precedenceLine.Length; i++)
        {
            _precedenceLine[i].transform.GetChild(0).GetComponent<Image>().color = _effectColor[1];
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
