using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// �÷��̾��� ���� ���� źâ�� ǥ�����ִ� ����� �����ϴ� Ŭ����
/// </summary>
public class MagazineUI : UISystem
{
    [SerializeField] private GameObject _magazineText;

    private void Update()
    {
        //for test
        SetMagazineText();  //�д׾׼� ��� ���� ��� ���� ������δ� �Ѿ��� �ǽð����� �پ���� �ʱ� ������, ���� ������ �ذ�� �� ���� ����
    }
    public override void OpenUI()
    {
        base.OpenUI();
    }
    public override void CloseUI()
    {
        base.CloseUI();
    }

    /// <summary>
    /// źâ �� UI�� �����մϴ�.
    /// �׼��� ���۵� ��, ���� ��, ������ �� ����˴ϴ�.
    /// ���� �׽�Ʈ������ �� �����Ӹ��� ������Ʈ�ǰ� �ֽ��ϴ�.
    /// </summary>
    public void SetMagazineText() 
    {
        Weapon weapon = FieldSystem.unitSystem.GetPlayer().weapon;
        _magazineText.GetComponent<TextMeshProUGUI>().text = weapon.currentAmmo.ToString() + " / " + weapon.maxAmmo.ToString();
    }
}
