using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 플레이어의 스텟 중 체력, 집중력, 액션 포인트를 표시하는 UI의 기능을 수행하는 클래스
/// </summary>
public class PlayerSummaryStatUI : UISystem
{
    private UnitStat _playerStat;
    private Unit _player;

    [SerializeField] private GameObject _healthPointUI;
    [SerializeField] private GameObject _ConcentrationUI;
    [SerializeField] private GameObject _actionPointUI;

    // Start is called before the first frame update
    void Start()
    {
        UIManager.instance.onPlayerStatChanged.AddListener(() => SetCurrentStatusUI());
        UIManager.instance.onTurnChanged.AddListener(() => SetCurrentStatusUI());
        UIManager.instance.onActionChanged.AddListener(() => SetCurrentStatusUI());
        SetCurrentStatusUI();
    }
    private void Update()
    {
        //for test
        //SetCurrentStatusUI();
        //유닛의 스텟 변화 시점을 감지할 방법이...? Unit의 stat변수는 protected이고... UnityEvent는 마땅한 것이 없다.
        //stat 변수의 set을 이용해서 해도 되긴 하는데 내가 구현한 게 아니라서 조심스럽다. 향후 리펙토링 시 고려.
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
    /// 상태 UI 정보(플레이어 체력, 집중력, 액션 포인트)를 설정합니다.
    /// 현재는 매 프레임마다 호출됩니다.
    /// </summary>
    public void SetCurrentStatusUI()
    {
        _player = FieldSystem.unitSystem.GetPlayer();
        if (_player is null) return;
        
        _playerStat = _player.stat;

        _healthPointUI.GetComponent<PlayerSummaryStatElement>().SetPlayerSummaryUI("Health Point", _playerStat.curHp.ToString() + " / " + _playerStat.GetStat(StatType.MaxHp).ToString());
        _ConcentrationUI.GetComponent<PlayerSummaryStatElement>().SetPlayerSummaryUI("Concentration", _playerStat.concentration.ToString());
        _actionPointUI.GetComponent<PlayerSummaryStatElement>().SetPlayerSummaryUI("Action Point", _playerStat.GetStat(StatType.CurActionPoint).ToString() + " / " + _playerStat.GetStat(StatType.MaxActionPoint).ToString());

        GetComponent<PlayerHpUI>().SetHpUI();
    }
}
