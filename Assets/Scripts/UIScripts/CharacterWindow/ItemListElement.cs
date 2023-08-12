using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ĳ���� ���� â���� �κ��丮�� ��ġ�� ������ ������ ������ Ŭ����
/// </summary>
public class ItemListElement : MonoBehaviour
{
    private Image _ItemIcon;
    private TextMeshProUGUI _ItemName;
    private int _itemIndex;

    // Start is called before the first frame update
    void Awake()
    {
        _ItemIcon = this.transform.GetChild(0).GetComponent<Image>();
        _ItemName = this.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// ������ ����Ʈ UI�� �� ������ UI ������ �����մϴ�.
    /// itemListUI�� ������ ����Ʈ ��ü�� ������ �� ����˴ϴ�.
    /// </summary>
    /// <param name="item"> ������ UI�� ǥ���� ������ </param>
    public void SetItemListElement(Item item)
    {
        _itemIndex = item.itemInfo.index;
        /*
        Sprite sprite = Resources.Load("Images/" + item.itemInfo.iconNumber) as Sprite;
        ItemIcon.sprite = sprite;
        */
        _ItemName.text = item.itemInfo.name;
    }

    /// <summary>
    /// ������ UI�� Ŭ������ �� �˾�â�� ����� ����� itemListUI���� �����ϴ�.
    /// </summary>
    public void OnItemUIBtnClick()
    {
        UIManager.instance.characterUI.itemListUI.ClickItemUIButton(_itemIndex);
    }
}
