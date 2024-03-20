using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryInteractionUI : UIElement
{
    [SerializeField] private GameObject _useBtn;
    public bool isEquipable { get; private set; }

    private void Awake()
    {
        isEquipable = true;
        CloseUI();
    }

    public void SetInventoryInteractionUI(Item item, Vector3 pos) 
    {
        if (item is null) 
        {
            CloseUI();
            return;
        }

        _useBtn.SetActive(true);
        switch (item.GetData().itemType)
        {
            case ItemType.Revolver:
            case ItemType.Repeater:
            case ItemType.Shotgun:
                {
                    isEquipable = true;
                    _useBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Equip";
                    break;
                }
            case ItemType.Heal:
            case ItemType.Damage:
            case ItemType.Cleanse:
            case ItemType.Buff:
            case ItemType.Debuff:
                {
                    isEquipable = false;
                    _useBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Use";
                    if (GameManager.instance.CompareState(GameState.World) && item.GetData().itemType != ItemType.Heal)
                    {
                        _useBtn.SetActive(false);
                    }
                    break;
                }
            default: 
                {
                    break;
                }
        }

        GetComponent<RectTransform>().position = pos;
        UIManager.instance.currentLayer = 3;
        UIManager.instance.SetUILayer();
        OpenUI();
    }
}
