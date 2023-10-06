using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterStatTextElement : UIElement, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI _nameText;
    private TextMeshProUGUI _contentsText;

    private bool _isSetContents = false;
    private bool _isOpenTooltip = false;
    public bool isMouseOver { get; private set; }
    private float _mouseOverCount = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        isMouseOver = false;
        _nameText = GetComponent<TextMeshProUGUI>();
        _contentsText = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMouseOver && !_isOpenTooltip && _isSetContents) 
        {
            _mouseOverCount += Time.deltaTime;
            if (_mouseOverCount > 0.5f) 
            {
                _isOpenTooltip = true;
                UIManager.instance.characterUI.characterStatUI.OpenCharacterTooltip(this, _nameText.text, GetComponent<RectTransform>().position.y);
            }
        }
    }
    public void SetCharacterStatText(CharacterStatUIInfo info)
    {
        _nameText.text = info.statName;
        _contentsText.text = info.GetFinalStatValueString();
        _isSetContents = (_nameText.text != "");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _nameText.color = Color.yellow;
        _contentsText.color = Color.yellow;

        _isOpenTooltip = false;
        isMouseOver = true;
        _mouseOverCount = 0.0f;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _nameText.color = Color.white;
        _contentsText.color = Color.white;

        _isOpenTooltip = false;
        isMouseOver = false;
        _mouseOverCount = 0.0f;

        UIManager.instance.characterUI.characterStatUI._characterStatTooltip.GetComponent<CharacterTooltip>().CloseUI();
    }
}
