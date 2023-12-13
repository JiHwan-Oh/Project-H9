using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class SkillTooltip : UIElement, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _skillTooltipTexts;
    [SerializeField] private GameObject _skillTooltipNameText;
    [SerializeField] private GameObject _skillTooltipDescriptionText;
    [SerializeField] private GameObject _skillTooltipButtonText;
    [SerializeField] private GameObject _skillKeywordTooltipContainer;

    private SkillManager _skillManager;
    private int _currentSkillIndex;
    private bool _isInteractableButton;

    static private SkillKeywordPool _keywordTooltips = null;
    private List<SkillKeywordWrapper> _activeKeywordTooltips = new List<SkillKeywordWrapper>();

    // Start is called before the first frame update
    void Start()
    {
        _skillManager = SkillManager.instance;
        _currentSkillIndex = 0;
        _isInteractableButton = false;

        if (_keywordTooltips == null)
        {
            _keywordTooltips = new SkillKeywordPool();
            _keywordTooltips.Init("Prefab/Keyword Tooltip", _skillKeywordTooltipContainer.transform, 0);
        }
    }
    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        if (!_skillKeywordTooltipContainer.activeInHierarchy) return;
        if (_skillKeywordTooltipContainer.transform.childCount == 0) return;
        GameObject rootGameObject = _skillKeywordTooltipContainer.transform.GetChild(0).gameObject;
        if (rootGameObject is null) return;
        rootGameObject.GetComponent<VerticalLayoutGroup>().CalculateLayoutInputVertical();
        rootGameObject.GetComponent<VerticalLayoutGroup>().SetLayoutVertical();
    }

    public void SetSkillTooltip(Vector3 pos, int skillIndex)
    {
        OpenUI();
        if (_currentSkillIndex != skillIndex) 
        {
            _currentSkillIndex = skillIndex;
        }
        _isInteractableButton = false;
        GetComponent<RectTransform>().position = pos;
        //GetComponent<RectTransform>().position = Input.mousePosition;
        Skill currentSkill = _skillManager.GetSkill(skillIndex);
        if (currentSkill == null) return;

        _skillTooltipNameText.GetComponent<TextMeshProUGUI>().text = SkillManager.instance.GetSkillName(_currentSkillIndex);
        _skillTooltipDescriptionText.GetComponent<TextMeshProUGUI>().text = SkillManager.instance.GetSkillDescription(_currentSkillIndex);
        _skillTooltipDescriptionText.GetComponent<ContentSizeFitter>().SetLayoutVertical();
        _skillTooltipTexts.GetComponent<ContentSizeFitter>().SetLayoutVertical();

        TextMeshProUGUI buttonText = _skillTooltipButtonText.GetComponent<TextMeshProUGUI>();
        if (currentSkill.isLearnable)
        {
            if (_skillManager.IsEnoughSkillPoint())
            {
                if (GameManager.instance.CompareState(GameState.World))
                {
                    buttonText.text = "����";
                    _isInteractableButton = true;
                }
                else
                {
                    buttonText.text = "���� �� ��ų ���� �Ұ�";
                }
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
        if (_isInteractableButton && _skillManager.LearnSkill(_currentSkillIndex))
        {
            
            UIManager.instance.skillUI.UpdateRelatedSkillNodes(_currentSkillIndex);
        }
        SetSkillTooltip(GetComponent<RectTransform>().position, _currentSkillIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _skillKeywordTooltipContainer.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _skillKeywordTooltipContainer.SetActive(false);
        CloseUI();
    }
    public void SetKeywordTooltipContents(KeywordScript keyword)
    {
        if (keyword == null) return;
        var t = _keywordTooltips.Set();
        t.tooltip.SetSkillKeywordTooltip(keyword.name, keyword.GetDescription(), _activeKeywordTooltips.Count);
        _activeKeywordTooltips.Add(t);
    }
    public void ClearKeywordTooltips()
    {
        _keywordTooltips.Reset();
        _activeKeywordTooltips.Clear();
        _skillKeywordTooltipContainer.SetActive(false);
    }
    public override void CloseUI()
    {
        ClearKeywordTooltips();
        base.CloseUI();
    }
}
