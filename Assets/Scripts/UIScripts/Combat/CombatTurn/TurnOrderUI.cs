using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOrderUI : UISystem
{
    [SerializeField] private GameObject _turnOrderUI;
    [SerializeField] private GameObject _turnOrderUIContainer;
    [SerializeField] private GameObject _turnOrderUIPrefab;

    private const int TURN_ORDER_UI_LENGTH = 12;

    // Start is called before the first frame update
    void Start()
    {
        TurnOrderUIObjectPooling(30);
        UIManager.instance.onTSceneChanged.AddListener((gs) => { if (gs == GameState.World) CloseUI(); });
    }
    public override void CloseUI()
    {
        while (_turnOrderUI.transform.childCount > 0)
        {
            DeleteTurnOrderUI(_turnOrderUI.transform.GetChild(0).gameObject);
        }
        base.CloseUI();
    }
    /// <summary>
    /// �� ���� UI�� �����մϴ�.
    /// turnSystem���� ����� �� ������ �Է¹޾� UI�� ǥ���մϴ�.
    /// ���� ������ ó�� ����� ��� InitTurnOrderUI�Լ��� ���� UI���� �ʱ� ��ġ�� �����ϰ�,
    /// �� ���Ŀ� ����� ��� ChangeTurnOrderUI�Լ��� ���� ���� UI���� ��ġ�� �̵����Ѽ� ǥ���մϴ�.
    /// </summary>
    /// <param name="turnOrder"> �� ���� ����Ʈ </param>
    public void SetTurnOrderUI(List<Unit> turnOrder)
    {
        if (_turnOrderUI.transform.childCount <= 0)
        {
            InitTurnOrderUI(turnOrder);
        }
        ChangeTurnOrderUI(turnOrder);
    }
    private void AddTurnOrderUI(Unit unit, int order)
    {
        if (_turnOrderUIContainer.transform.childCount <= 0)
        {
            TurnOrderUIObjectPooling(15);
        }
        GameObject ui = _turnOrderUIContainer.transform.GetChild(0).gameObject;
        ui.transform.SetParent(_turnOrderUI.transform, false);
        ui.GetComponent<TurnOrderUIElement>().InitTurnOrderUIElement(unit, 1 + TURN_ORDER_UI_LENGTH);
        ui.SetActive(true);
    }
    private void DeleteTurnOrderUI(GameObject ui)
    {
        ui.transform.SetParent(_turnOrderUIContainer.transform, false);
        ui.SetActive(false);
    }
    public void DeleteDeadUnitTurnOrderUI(Unit unit) 
    {
        for (int i = transform.childCount; i >= 0; i--)
        {
            if (_turnOrderUI.transform.GetChild(i).GetComponent<TurnOrderUIElement>().unit == unit)
            {
                DeleteTurnOrderUI(_turnOrderUI.transform.GetChild(i).gameObject);
            }
        }
    }
    private void InitTurnOrderUI(List<Unit> turnOrder)
    {
        int order = 0;
        foreach (Unit unit in turnOrder)
        {
            AddTurnOrderUI(unit, order++);
        }
    }
    private void ChangeTurnOrderUI(List<Unit> turnOrder)
    {
        //���� �ϼ����� �纻
        List<GameObject> currentTurnOrderUI = new List<GameObject>();
        for (int i = 0; i < _turnOrderUI.transform.childCount; i++)
        {
            currentTurnOrderUI.Add(_turnOrderUI.transform.GetChild(i).gameObject);
        }

        //currentTurnOrderUI�� �ش� ��Ҹ� üũ�ߴ��� ����
        bool[] check = new bool[currentTurnOrderUI.Count];
        for (int i = 0; i < check.Length; i++) check[i] = false;

        //����� ������ ��ҵ��� ���� currentTurnOrderUI�� �ε����� ǥ���� ������� ����
        List<int> changeOrder = new List<int>();

        //�Էµ� turnOrder�� �°� changeOrder�� ����.
        //turnOrder������� currentTurnOrderUI���� �ش� ��Ҹ� �˻��� ��,
        //check�� true�� ������ �� �ش� ����� �ε����� changeOrder�� ����.
        //���� turnOrder�� ���� ��Ұ� currentTurnOrderUI�� �������� �ʴ´ٸ�, currentTurnOrderUI�� �ƴ϶� ���� _turnOrderUI�� ���� �ش� ��Ҹ� ����.
        foreach (Unit unit in turnOrder)
        {
            bool isFound = false;
            for (int i = 1; i < currentTurnOrderUI.Count; i++)
            {
                bool isSameTypeObject = (currentTurnOrderUI[i].GetComponent<TurnOrderUIElement>().unit.GetType() == unit.GetType());
                bool isAlreadyCheck = check[i];
                if (isSameTypeObject && !isAlreadyCheck)
                {
                    changeOrder.Add(i);
                    check[i] = true;
                    isFound = true;
                    break;
                }
            }
            if (!isFound)
            {
                AddTurnOrderUI(unit, TURN_ORDER_UI_LENGTH);
                changeOrder.Add(_turnOrderUI.transform.childCount - 1);
            }
        }

        //������ ���� ��Ҹ� ������ _turnOrderUI�� ���� currentTurnOrderUI�� �ٽ� ����
        currentTurnOrderUI = new List<GameObject>();
        for (int i = 0; i < _turnOrderUI.transform.childCount; i++)
        {
            currentTurnOrderUI.Add(_turnOrderUI.transform.GetChild(i).gameObject);
        }
        //ǥ�� ������ ��Ű�� ���� ���� ǥ������ UI���� �ӽ÷� ���� ������Ʈ Ǯ�� ��� ��ҷ� �̵�.(���� ������ currentTurnOrderUI�� ���)
        while (_turnOrderUI.transform.childCount > 0)
        {
            _turnOrderUI.transform.GetChild(0).SetParent(_turnOrderUIContainer.transform, false);
        }

        //������ changeOrder�� ���� currentTurnOrderUI���� ������� ��Ҹ� ������ _turnOrderUI�� �ٽ� �̵��ϰ� changeOrder�Լ��� ������ UI ��ġ�� �̵��ϵ��� ���
        for (int i = 0; i < changeOrder.Count; i++)
        {
            //Debug.Log(currentTurnOrderUI[changeOrder[i]].GetComponent<TurnOrderUIElement>()._unit + "index: " + changeOrder[i]);
            currentTurnOrderUI[changeOrder[i]].transform.SetParent(_turnOrderUI.transform, false);
            currentTurnOrderUI[changeOrder[i]].GetComponent<TurnOrderUIElement>().ChangeOrder(i);
        }

        //�Էµ� turnOrder�� ���� ��ҵ��� �״�� ��Ȱ��ȭ.
        for (int i = 0; i < check.Length; i++)
        {
            if (check[i] == false)
            {
                DeleteTurnOrderUI(currentTurnOrderUI[i].gameObject);
            }
        }
    }

    private void TurnOrderUIObjectPooling(int length)
    {
        for (int i = 0; i < length; i++)
        {
            GameObject ui = Instantiate(_turnOrderUIPrefab, Vector3.zero, Quaternion.identity, _turnOrderUIContainer.transform);
            ui.SetActive(false);
        }
    }

    public void EffectMouseOverEnemy(Enemy enemy)
    {
        for (int i = 0; i < _turnOrderUI.transform.childCount; i++)
        {
            if (_turnOrderUI.transform.GetChild(i).GetComponent<TurnOrderUIElement>().unit is Player) continue;

            bool isEffectOn = (enemy != null && _turnOrderUI.transform.GetChild(i).GetComponent<TurnOrderUIElement>()?.unit == (Unit)enemy);
            _turnOrderUI.transform.GetChild(i).GetComponent<TurnOrderUIElement>().EffectTurnOrderUIElement(isEffectOn);
        }
    }
}
