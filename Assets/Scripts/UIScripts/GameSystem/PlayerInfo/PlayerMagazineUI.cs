using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMagazineUI : UIElement, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _magazineTooltip;

    [SerializeField] private PlayerMagazineUIElement _revolvers;
    [SerializeField] private PlayerMagazineUIElement _shotguns;
    [SerializeField] private PlayerMagazineUIElement _repeater;


    private int MAGAZINE_SIZE = 6;
    private int MAGAZINE_COUNT = 7;
    void Start()
    {
        if (_magazineTooltip == null) return;
        _magazineTooltip?.SetActive(false);
    }
    public void SetMagazineUI(bool isOnlyDisplayMaxMagazine)
    {
        int maxAmmo;
        Magazine magazine;
        int flickerCnt;

        if (isOnlyDisplayMaxMagazine)
        {
            maxAmmo = GameManager.instance.itemDatabase.GetItemData(GameManager.instance.playerWeaponIndex).weaponAmmo;
            magazine = new Magazine(maxAmmo);
            flickerCnt = 0;
        }
        else
        {
            maxAmmo = FieldSystem.unitSystem.GetPlayer().weapon.maxAmmo;
            magazine = FieldSystem.unitSystem.GetPlayer().weapon.magazine;
            flickerCnt = UIManager.instance.gameSystemUI.playerInfoUI.summaryStatusUI.expectedMagUsage;
        }

        if (_revolvers.gameObject.activeSelf == true)
            _revolvers.gameObject.SetActive(false);
        if (_shotguns.gameObject.activeSelf == true)
            _shotguns.gameObject.SetActive(false);
        if (_repeater.gameObject.activeSelf == true)
            _repeater.gameObject.SetActive(false);

        var weaponType = FieldSystem.unitSystem.GetPlayer().weapon.GetWeaponType();
        if (weaponType == ItemType.Revolver)
        {
            _revolvers.gameObject.SetActive(true);
            _revolvers.Reload(maxAmmo, magazine, flickerCnt);
        }
        if (weaponType == ItemType.Shotgun)
        {
            _shotguns.gameObject.SetActive(true);
            _shotguns.Reload(maxAmmo, magazine, flickerCnt);
        }
        if (weaponType == ItemType.Repeater)
        {
            _repeater.gameObject.SetActive(true);
            _repeater.Reload(maxAmmo, magazine, flickerCnt);
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_magazineTooltip == null) return;
        _magazineTooltip.GetComponent<PlayerMainStatTooltip>().SetPlayerMainStatTooltip(gameObject);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_magazineTooltip == null) return;
        _magazineTooltip?.SetActive(false);
    }
}
