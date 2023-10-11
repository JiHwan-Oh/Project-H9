using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatLevelInfo 
{
    public string statName { get; private set; }
    public int statIncreaseValue { get; private set; }
    public int statLevel { get; private set; }

    public PlayerStatLevelInfo(string name, int increse) 
    {
        statName = name;
        statIncreaseValue = increse;
        statLevel = 0;
    }

    public void LevelUpStat() 
    {
        if(IsLevelUpFully()) return;
        statLevel++;
        if (statName == "Concentration")
        {
            GameManager.instance.playerStat.concentration += statIncreaseValue;
        }
        else if (statName == "Sight Range")
        {
            GameManager.instance.playerStat.sightRange += statIncreaseValue;
        }
        else if (statName == "Speed")
        {
            GameManager.instance.playerStat.speed += statIncreaseValue;
        }
        UIManager.instance.onPlayerStatChanged.Invoke();
    }
    public bool IsLevelUpFully() 
    {
        return (statLevel >= 6);
    }
    public int GetPlayerCurrentValue()
    {
        UnitStat playerStat = GameManager.instance.playerStat;
        if (statName == "Concentration")
        {
            return playerStat.concentration;
        }
        else if (statName == "Sight Range")
        {
            return playerStat.sightRange;
        }
        else if (statName == "Speed")
        {
            return playerStat.speed;
        }

        Debug.LogError("유효하지 않은 statName입니다.");
        return -1;
    }
}

public class PlayerStatLevelUpUI : UISystem
{
    [SerializeField] private GameObject _statLevelUpWindow;
    [SerializeField] private GameObject _background;
    [SerializeField] private GameObject _statLevelUpTitleText;
    [SerializeField] private GameObject[] _statCardButtons;
    [SerializeField] private GameObject _statSelectButton;

    private bool isOpenUI = false;
    public PlayerStatLevelInfo[] statLevels { get; private set; }
    public int selectedCardNumber { get; private set; }
    public bool isSelectedSomeCard { get; private set; }


    private int _appearSpeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        statLevels = new PlayerStatLevelInfo[3];
        statLevels[0] = new PlayerStatLevelInfo("Concentration", 10);
        statLevels[1] = new PlayerStatLevelInfo("Sight Range", 1);
        statLevels[2] = new PlayerStatLevelInfo("Speed", 10);

        selectedCardNumber = -1;
        isSelectedSomeCard = false;
        ClosePlayerStatLevelUpUI();
    }

    // Update is called once per frame
    void Update()
    {

        float[] appearTargetValue = { 380, 0, 192 / 255.0f };
        float[] disappearTargetValue = 
        {
            Camera.main.pixelHeight / 2  + _statLevelUpTitleText.GetComponent<RectTransform>().sizeDelta.y / 2, 
            -(Camera.main.pixelHeight / 2 + _statCardButtons[0].GetComponent<RectTransform>().sizeDelta.y / 2),
            0
        };
        Vector3 pos;
        Color color;
        float alpha;
        float[] targetValue;
        if (isOpenUI)
        {
            targetValue = appearTargetValue;
        }
        else
        {
            targetValue = disappearTargetValue;
        }
        pos = _statLevelUpTitleText.GetComponent<RectTransform>().localPosition;
        pos.y = CalculationLerpValue(pos.y, targetValue[0]);
        _statLevelUpTitleText.GetComponent<RectTransform>().localPosition = pos;
        if (pos.y == disappearTargetValue[0]) _statLevelUpTitleText.SetActive(false);

            for (int i = 0; i < _statCardButtons.Length; i++)
        {
            pos = _statCardButtons[i].GetComponent<RectTransform>().localPosition;
            if (selectedCardNumber != i || isOpenUI)
            {
                pos.y = CalculationLerpValue(pos.y, targetValue[1]);
                _statCardButtons[i].GetComponent<RectTransform>().localPosition = pos;
            }

            alpha = _statCardButtons[i].GetComponent<CanvasGroup>().alpha;
            if (!isOpenUI)
            {
                alpha = CalculationLerpValue(alpha, 0);
                _statCardButtons[i].GetComponent<CanvasGroup>().alpha = alpha;
            }

            if (pos.y == disappearTargetValue[1] || alpha == 0) 
            {
                _statCardButtons[i].SetActive(false);
            }
        }

        color = _background.GetComponent<Image>().color;
        color.a = CalculationLerpValue(color.a, targetValue[2]);
        _background.GetComponent<Image>().color = color;
        if (color.a == disappearTargetValue[2]) _background.SetActive(false);

    }
    private float CalculationLerpValue(float a, float b)
    {
        float threshold = 0.01f;
        if (Mathf.Abs(a - b) < threshold)
        {
            a = b;
        }
        else
        {
            a = Mathf.Lerp(a, b, Time.deltaTime * _appearSpeed);
        }
        return a;
    }

    public void OpenPlayerStatLevelUpUI() 
    {
        if (isOpenUI) return;
        int cnt = 0;
        for (int i = 0; i < _statCardButtons.Length; i++)
        {
            if (statLevels[i].IsLevelUpFully())
            {
                cnt++;
            }
        }
        Debug.Log(cnt);
        if (cnt >= _statCardButtons.Length) return;

        OpenUI();
        isOpenUI = true;
        selectedCardNumber = -1;
        isSelectedSomeCard = false;

        //Set Title Position
        _statLevelUpTitleText.SetActive(true);
        Vector3 pos = _statLevelUpTitleText.GetComponent<RectTransform>().localPosition;
        pos.y = Camera.main.pixelHeight / 2  + _statLevelUpTitleText.GetComponent<RectTransform>().sizeDelta.y / 2;
        _statLevelUpTitleText.GetComponent<RectTransform>().localPosition = pos;

        //Set Card Position
        for (int i = 0; i < _statCardButtons.Length; i++) 
        {
            pos = _statCardButtons[i].GetComponent<RectTransform>().localPosition;
            pos.y = -(Camera.main.pixelHeight / 2 + _statCardButtons[i].GetComponent<RectTransform>().sizeDelta.y / 2 * (i + 1));
            _statCardButtons[i].GetComponent<RectTransform>().localPosition = pos;
            _statCardButtons[i].GetComponent<PlayerStatLevelUpElement>().SetPlayerStatLevelUpCard(statLevels[i]);
            _statCardButtons[i].GetComponent<CanvasGroup>().alpha = 1;
        }
        _statSelectButton.GetComponent<PlayerStatLevelUpSelectButton>().InitPlayerStatLevelUpSelectButton();

        //Set Background Color
        _background.SetActive(true);
        Color color = _background.GetComponent<Image>().color;
        color.a = 0;
        _background.GetComponent<Image>().color = color;

        _statSelectButton.SetActive(false);

        _statLevelUpWindow.SetActive(true);
    }
    private void ClosePlayerStatLevelUpUI() 
    {
        isOpenUI = false;
        for (int i = 0; i < _statCardButtons.Length; i++)
        {
            _statCardButtons[i].GetComponent<PlayerStatLevelUpElement>().CloseUI();
        }
        _statSelectButton.SetActive(false);

        CloseUI();
    }

    public void ClickStatCard(int cardNumber, bool isSelected) 
    {
        if (isSelectedSomeCard && selectedCardNumber != cardNumber) return;
        isSelectedSomeCard = isSelected;
        if (isSelected)
        {
            selectedCardNumber = cardNumber;
        }
        else
        {
            selectedCardNumber = -1;
        }
        _statSelectButton.SetActive(isSelected);
    }
    public void ClickSelectButton() 
    {
        if (!isSelectedSomeCard) return;

        string levelUpStatName = _statCardButtons[selectedCardNumber].GetComponent<PlayerStatLevelUpElement>().statLevelInfo.statName;
        for (int i = 0; i < statLevels.Length; i++) 
        {
            if (levelUpStatName == statLevels[i].statName)
            {
                statLevels[i].LevelUpStat();
            }
        }

        ClosePlayerStatLevelUpUI();
    }
}
