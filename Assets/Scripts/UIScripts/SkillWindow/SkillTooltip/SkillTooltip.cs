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
    [SerializeField] private GameObject _skillKeywordTooltipContainer;

    private SkillManager _skillManager;
    private int _currentSkillIndex;
    private bool _isInteractableButton;

    static private SkillKeywordPool _keywordTooltips = new SkillKeywordPool();
    private int _keywordTooltipCount = 0;
    private KeywordScript _keyword;

    // Start is called before the first frame update
    void Start()
    {
        _skillManager = SkillManager.instance;
        _currentSkillIndex = 0;
        _isInteractableButton = false;

        _keywordTooltips = new SkillKeywordPool();
            _keywordTooltips.Init("Prefab/Keyword Tooltip", _skillKeywordTooltipContainer.transform, 0);
    }

    public void SetSkillTooltip(Vector3 pos, int skillIndex)
    {
        OpenUI();
        if (_currentSkillIndex != skillIndex) 
        {
            _keyword = null;
            _currentSkillIndex = skillIndex;
        }
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
        _skillKeywordTooltipContainer.SetActive(_keyword != null);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _skillKeywordTooltipContainer.SetActive(false);
    }
    public void SetKeywordTooltipContents(KeywordScript kw)
    {
        _keyword = kw;
        SetKeywordTooltips();
    }
    private void SetKeywordTooltips()
    {
        if (_keyword == null) return;
        var t = _keywordTooltips.Set();
        t.tooltip.SetSkillKeywordTooltip(_keywordTooltipCount++, _keyword.name, _keyword.description);
    }
    public override void CloseUI()
    {
        _keywordTooltipCount = 0;
        _keywordTooltips.Reset();
        _skillKeywordTooltipContainer.SetActive(false);
        base.CloseUI();
    }
}
