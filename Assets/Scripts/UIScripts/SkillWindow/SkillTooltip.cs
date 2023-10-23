using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SkillTooltip : UIElement, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _skillTooltipNameText;
    [SerializeField] private GameObject _skillTooltipDescriptionText;
    [SerializeField] private GameObject _skillTooltipButtonText;
    [SerializeField] private GameObject _skillKeywordTooltips;

    private SkillManager _skillManager;
    private int _currentSkillIndex;

    // Start is called before the first frame update
    void Start()
    {
        _skillManager = SkillManager.instance;
    }

    public void SetSkillTooltip(Vector3 pos, int skillIndex) 
    {
        GetComponent<RectTransform>().position = pos;
        Skill currentSkill = _skillManager.GetSkill(skillIndex);

        _currentSkillIndex = skillIndex;
        _skillTooltipNameText.GetComponent<TextMeshProUGUI>().text = SkillManager.instance.GetSkillName(_currentSkillIndex);
        _skillTooltipDescriptionText.GetComponent<TextMeshProUGUI>().text = SkillManager.instance.GetSkillDescription(_currentSkillIndex);

        TextMeshProUGUI buttonText = _skillTooltipButtonText.GetComponent<TextMeshProUGUI>();
        if (currentSkill.isLearnable)
        {
            if (_skillManager.IsEnoughSkillPoint())
            {
                buttonText.text = "����";
            }
            else
            {
                buttonText.text = "��ų ����Ʈ ����";
            }
        }
        else
        {
            if (currentSkill.isLearned)
            {
                buttonText.text = "���� �Ϸ�";
            }
            else
            {
                buttonText.text = "���� �Ұ�";
            }
        }
    }

    /// <summary>
    /// �ش� ��ų�� �����ϴ� ���� �����ϴٸ� SkillManager�� ���� ��ų�� �����մϴ�.
    /// ��ų ����â�� ��ų�����ư�� Ŭ���� �� ����˴ϴ�.
    /// </summary>
    public void ClickLearnSkill()
    {
        if (_skillManager.LearnSkill(_currentSkillIndex))
        {
            UIManager.instance.skillUI.UpdateSkillUIImage();
        }
        SetSkillTooltip(GetComponent<RectTransform>().position, _currentSkillIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //��ȹ ���� Set �Լ� ȣ�� ��ġ üũ�ؾ� ��.
        _skillKeywordTooltips.SetActive(true);
        return;
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _skillKeywordTooltips.GetComponent<SkillKeywordTooltip>().CloseUI();
    }
}
