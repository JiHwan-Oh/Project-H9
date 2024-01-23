using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �÷��̾� Hp������UI ������ ����� ������ Ŭ����
/// </summary>
public class PlayerHpUIElement : UIElement
{
    [SerializeField] private Sprite _playerHpFillSprite;
    [SerializeField] private Sprite _playerHpEmptySprite;

    /// <summary>
    /// Hp������UI�� ä��ϴ�.
    /// </summary>
    public void FillUI()
    {
        GetComponent<Image>().color = new Color(1, 0, 0, 1);
    }

    /// <summary>
    /// Hp������UI�� ���ϴ�.
    /// </summary>
    public void EmptyUI()
    {
        GetComponent<Image>().color = new Color(1, 1, 1, 1);
    }
}
