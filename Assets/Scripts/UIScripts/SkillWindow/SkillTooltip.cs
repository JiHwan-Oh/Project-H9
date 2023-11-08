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
    private bool _isInteractableButton;

    // Start is called before the first frame update
    void Start()
    {
        _skillManager = SkillManager.instance;
        _isInteractableButton = false;
    }

    public void SetSkillTooltip(Vector3 pos, int skillIndex)
    {
        _currentSkillIndex = skillIndex;
        _isInteractableButton = false;
        GetComponent<RectTransform>().position = pos;
        Skill currentSkill = _skillManager.GetSkill(skillIndex);
        if (currentSkill == null) return;

        _skillTooltipNameText.GetComponent<TextMeshProUGUI>().text = SkillManager.instance.GetSkillName(_currentSkillIndex);
        _skillTooltipDescriptionText.GetComponent<TextMeshProUGUI>().text = SkillManager.instance.GetSkillDescription(_currentSkillIndex);

        TextMeshProUGUI buttonText = _skillTooltipButtonText.GetComponent<TextMeshProUGUI>();
        if (GameManager.instance.CompareState(GameState.World))
        {
            if (currentSkill.isLearnable)
            {
                if (_skillManager.IsEnoughSkillPoint())
                {
                    _isInteractableButton = true;
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
        else 
        {
            buttonText.text = "���� �� ��ų ���� �Ұ�";
        }
    }

    /// <summary>
    /// �ش� ��ų�� �����ϴ� ���� �����ϴٸ� SkillManager�� ���� ��ų�� �����մϴ�.
    /// ��ų ����â�� ��ų�����ư�� Ŭ���� �� ����˴ϴ�.
    /// </summary>
    public void ClickLearnSkill()
    {
        if (_isInteractableButton && _skillManager.LearnSkill(_currentSkillIndex))
        {
            UIManager.instance.skillUI.UpdateAllSkillUINode();
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
