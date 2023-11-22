using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class CharacterTooltipText : UIElement, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _subTooltip;
    [SerializeField] private GameObject _subTooltipText;

    private bool _isSetContents = false;
    private bool _isOpenTooltip = false;
    private bool isMouseOver = false;
    private float _mouseOverCount = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
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
                _subTooltip.SetActive(true);
            }
        }
    }

    public void SetCharacterTooltipText(string statName, UIStatType statType, float value, float xPosition) 
    {
        OpenUI();
        string valueStr = "";
        if (statType == UIStatType.Character) 
        {
            valueStr = value.ToString();
            GetComponent<TextMeshProUGUI>().color = UICustomColor.PlayerStatColor;
            _subTooltipText.GetComponent<TextMeshProUGUI>().color = UICustomColor.PlayerStatColor;
            string str = "ĳ���� ���ʽ�";
            if (statName == "Additional Hit Rate" || statName == "Critical Chance" || statName == "Critical Damage")
            {
                WeaponType playerWeaponType = FieldSystem.unitSystem.GetPlayer().weapon.GetWeaponType();
                if (playerWeaponType == WeaponType.Revolver)
                {
                    str += " (������)";
                }
                else if (playerWeaponType == WeaponType.Repeater)
                {
                    str += " (������)";
                }
                else if (playerWeaponType == WeaponType.Shotgun)
                {
                    str += " (����)";
                }
            }
            _subTooltipText.GetComponent<TextMeshProUGUI>().text = str;
        }
        else if (statType == UIStatType.Weapon)
        {
            valueStr = value.ToString();
            GetComponent<TextMeshProUGUI>().color = UICustomColor.WeaponStatColor;
            _subTooltipText.GetComponent<TextMeshProUGUI>().color = UICustomColor.WeaponStatColor;
            _subTooltipText.GetComponent<TextMeshProUGUI>().text = "���� ���� ���ʽ�";
        }
        else if (statType == UIStatType.SkillAdd)
        {
            valueStr = value.ToString();
            GetComponent<TextMeshProUGUI>().color = UICustomColor.SkillStatColor;
            _subTooltipText.GetComponent<TextMeshProUGUI>().color = UICustomColor.SkillStatColor;
            _subTooltipText.GetComponent<TextMeshProUGUI>().text = "��ų ���ʽ�";
        }
        else if (statType == UIStatType.PlusSign)
        {
            valueStr = "+";
            GetComponent<TextMeshProUGUI>().color = Color.white;
            _subTooltipText.GetComponent<TextMeshProUGUI>().text = "";
        }
        else if (statType == UIStatType.MultiSign)
        {
            valueStr = "*";
            GetComponent<TextMeshProUGUI>().color = Color.white;
            _subTooltipText.GetComponent<TextMeshProUGUI>().text = "";
        }
        _isSetContents = (valueStr != "+");
        GetComponent<TextMeshProUGUI>().text = valueStr;


        Vector2 size = GetComponent<RectTransform>().sizeDelta;
        size.x = 15;

        Vector3 pos = GetComponent<RectTransform>().localPosition;
        pos.x = xPosition * size.x;
        GetComponent<RectTransform>().localPosition = pos;

        size.x *= value.ToString().Length;
        GetComponent<RectTransform>().sizeDelta = size;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isOpenTooltip = false;
        isMouseOver = true;
        _mouseOverCount = 0.0f;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _isOpenTooltip = false;
        isMouseOver = false;
        _mouseOverCount = 0.0f;

        _subTooltip.SetActive(false);
    }
}
