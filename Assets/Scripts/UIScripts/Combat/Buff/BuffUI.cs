using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUI : UISystem
{
    [SerializeField] private GameObject _BuffUI;
    [SerializeField] private GameObject _DebuffUI;
    [SerializeField] private PassiveDatabase passiveDB;

    private List<IDisplayableEffect> _currentBuffs = new List<IDisplayableEffect>();
    private List<IDisplayableEffect> _currentDebuffs = new List<IDisplayableEffect>();

    private new void Awake()
    {
        base.Awake();
        _BuffUI.SetActive(false);
        _DebuffUI.SetActive(false);

        UIManager.instance.onPlayerStatChanged.AddListener(SetBuffDebuffUI);
        UIManager.instance.onActionChanged.AddListener(SetBuffDebuffUI);
        UIManager.instance.onTurnChanged.AddListener(SetBuffDebuffUI);
    }
    public override void OpenUI()
    {
        base.OpenUI();
        SetBuffDebuffUI();
    }

    public void SetBuffDebuffUI()
    {
        Player player = FieldSystem.unitSystem.GetPlayer();
        if (player == null) return;
        IDisplayableEffect[] playerBuffs = player.GetDisplayableEffects();
        _currentBuffs.Clear();
        _currentDebuffs.Clear();
        foreach (IDisplayableEffect effect in playerBuffs) 
        {
            if (effect is StatusEffect) 
            {
                _currentDebuffs.Add(effect);
            }
            else
            {
                _currentBuffs.Add(effect);
            }
        }
        SetBuffUI(_BuffUI, _currentBuffs, true);
        SetBuffUI(_DebuffUI, _currentDebuffs, false);
    }
    private void SetBuffUI(GameObject UI, List<IDisplayableEffect> currentState, bool isBuff)
    {
        UI.SetActive(true);
        for (int i = 0; i < UI.transform.childCount; i++)
        {
            UI.transform.GetChild(i).GetComponent<BuffUIElement>().CloseUI();
        }
        int buffCount = 0;

        foreach (IDisplayableEffect effect in currentState)
        {
            Debug.Log(effect.GetIndex() +" / "+effect.CanDisplay());
            if (effect.CanDisplay())
            {
                UI.transform.GetChild(buffCount++).GetComponent<BuffUIElement>().SetBuffUIElement(effect, isBuff);
            }
        }

        if (buffCount == 0)
        {
            UI.SetActive(false);
        }
    }
}
