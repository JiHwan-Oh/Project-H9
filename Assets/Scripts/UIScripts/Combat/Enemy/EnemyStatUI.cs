using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// ���� �� ���� ������ Ȯ���� �� �ִ� �� ����â UI�� ǥ���ϴ� ����� �����ϴ� Ŭ����
/// </summary>
public class EnemyStatUI : UISystem
{
    [SerializeField] private GameObject _enemyStatWindow;

    [SerializeField] private GameObject _enemyStatText;
    [SerializeField] private GameObject _weaponStatText1Contents;
    [SerializeField] private GameObject _weaponStatText2Contents;
    [SerializeField] private GameObject _weaponStatText3Name;
    [SerializeField] private GameObject _weaponStatText3Contents;

    private const float WINDOW_X_POSITION_CORRECTION = 300;

    private bool _isOpenedUI = false;
    private Enemy _enemy;

    public override void CloseUI()
    {
        base.CloseUI();
        CloseEnemyStatUI();
    }

    public override void OpenPopupWindow()
    {
        _isOpenedUI = true;
        UIManager.instance.previousLayer = 3;
        _enemyStatWindow.SetActive(true);
    }
    public override void ClosePopupWindow()
    {
        _isOpenedUI = false;
        UIManager.instance.previousLayer = 1;
        CloseEnemyStatUI();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Player player = FieldSystem.unitSystem.GetPlayer();
            if (player is null || player.GetSelectedAction().GetActionType() is not ActionType.Idle) return;
            Vector3Int enemyPos;
            if (IsMouseClickedEnemy(out enemyPos))
            {
                Debug.Log("Click");
                SetEnemyStatUI((Enemy)FieldSystem.unitSystem.GetUnit(enemyPos));
            }
        }
        if (_isOpenedUI)
        {
            if (_enemy is null) 
            {
                ClosePopupWindow();
                return;
            }
            SetEnemyStatUIPosition(_enemy.transform.position);
        }
    }
    /// <summary>
    /// �� ����â�� �����մϴ�.
    /// �÷��̾ Idle ������ �� �� ������ Ŭ���ϸ� �ش� �� ���� ���� ���� ����â UI�� �����˴ϴ�.
    /// </summary>
    /// <param name="enemy"> Ŭ���� �� ���� </param>
    public void SetEnemyStatUI(Enemy enemy)
    {
        OpenPopupWindow();
        _enemy = enemy;
        SetEnemyStatUIPosition(enemy.transform.position);
        SetCharacterStatText(enemy.stat);
        SetWeaponStatText(enemy.weapon);
    }
    private void SetEnemyStatUIPosition(Vector3 pos)
    {
        //Select UI Right or Left side
        Player player = FieldSystem.unitSystem.GetPlayer();
        int sign = (int)(pos.x - player.transform.position.x);
        if (sign == 0)
        {
            sign = -1;
        }
        sign = sign / Mathf.Abs(sign);

        Vector3 enemyPositionHeightCorrection = pos;
        enemyPositionHeightCorrection.y += 1.8f;

        Vector2 enemyScreenPos = Camera.main.WorldToScreenPoint(enemyPositionHeightCorrection);
        Vector2 windowSetPos = enemyScreenPos;
        windowSetPos.x += sign * WINDOW_X_POSITION_CORRECTION;

        //Set UI Position
        _enemyStatWindow.GetComponent<RectTransform>().position = windowSetPos;

        //Screen Range Correction
        Vector2 enemyScreenLocalPos = _enemyStatWindow.GetComponent<RectTransform>().localPosition;

        Vector2 canvasSize = GetComponent<RectTransform>().sizeDelta;
        Vector2 windowSize = _enemyStatWindow.GetComponent<RectTransform>().sizeDelta;
        float xPosLimit = (canvasSize.x / 2 - windowSize.x / 2) - 5;
        float yPosLimit = (canvasSize.y / 2 - windowSize.y / 2) - 5;

        if (enemyScreenLocalPos.x > xPosLimit) enemyScreenLocalPos.x = xPosLimit;
        if (enemyScreenLocalPos.x < -xPosLimit) enemyScreenLocalPos.x = -xPosLimit;
        if (enemyScreenLocalPos.y > yPosLimit) enemyScreenLocalPos.y = yPosLimit;
        if (enemyScreenLocalPos.y < -yPosLimit) enemyScreenLocalPos.y = -yPosLimit;

        _enemyStatWindow.GetComponent<RectTransform>().localPosition = enemyScreenLocalPos;

        //Alpha setting
        Vector2 windowScreenPos = _enemyStatWindow.GetComponent<RectTransform>().position;
        bool isEnemyInWindowXpos = (windowScreenPos.x - windowSize.x / 2 < enemyScreenPos.x && enemyScreenPos.x < windowScreenPos.x + windowSize.x / 2);
        bool isEnemyInWindowYpos = (windowScreenPos.y - windowSize.y / 2 < enemyScreenPos.y && enemyScreenPos.y < windowScreenPos.y + windowSize.y / 2);
        float alpha = 0;
        if (isEnemyInWindowXpos && isEnemyInWindowYpos)
        {
            alpha = 0.5f;
        }
        else 
        {
            alpha = 1;
        }
        Color c = _enemyStatWindow.GetComponent<Image>().color;
        float threshold = 0.01f;
        if (Mathf.Abs(c.a - alpha) > threshold)
        {
            c.a = Mathf.Lerp(c.a, alpha, Time.deltaTime * 2);
        }
        else
        {
            c.a = alpha;
        }
        _enemyStatWindow.GetComponent<Image>().color = c;

    }
    private void SetCharacterStatText(UnitStat enemyStat)
    {
        string text = enemyStat.maxHp.ToString() + '\n' +
                      enemyStat.concentration.ToString() + '\n' +
                      enemyStat.sightRange.ToString() + '\n' +
                      enemyStat.speed.ToString() + '\n' +
                      enemyStat.maxActionPoint.ToString() + '\n' +
                      enemyStat.additionalHitRate.ToString() + '\n' +
                      enemyStat.criticalChance.ToString();

        _enemyStatText.GetComponent<TextMeshProUGUI>().text = text;
    }
    private void SetWeaponStatText(Weapon weapon)
    {
        string text1 = /*weapon.weaponName +*/'\n' +
                       weapon.weaponDamage.ToString();
        _weaponStatText1Contents.GetComponent<TextMeshProUGUI>().text = text1;

        string text2 = weapon.currentAmmo.ToString() + " / " + weapon.maxAmmo.ToString() + '\n' +
                       weapon.weaponRange.ToString();
        _weaponStatText2Contents.GetComponent<TextMeshProUGUI>().text = text2;



        string text3 = "", text4 = "";
        float hitRate = weapon.hitRate;
        float criChance = weapon.criticalChance;
        float criDamage = weapon.criticalDamage;
        if (hitRate != 0)
        {
            text3 += "Hit Rate:" + '\n';
            text4 += hitRate.ToString() + '\n';
        }
        if (criChance != 0)
        {
            text3 += "Critical Chance:" + '\n';
            text4 += criChance.ToString() + '\n';
        }
        if (criDamage != 0)
        {
            text3 += "Critical Damage:" + '\n';
            text4 += criDamage.ToString() + '\n';
        }
        _weaponStatText3Name.GetComponent<TextMeshProUGUI>().text = text3;
        _weaponStatText3Contents.GetComponent<TextMeshProUGUI>().text = text4;
    }

    /// <summary>
    /// �� ����â UI�� �ݽ��ϴ�.
    /// </summary>
    public void CloseEnemyStatUI()
    {
        _enemyStatWindow.SetActive(false);
    }
    private static bool IsMouseClickedEnemy(out Vector3Int pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool isSuccessRaycast = Physics.Raycast(ray, out var hit, float.MaxValue, layerMask: LayerMask.GetMask("Enemy"));
        if (isSuccessRaycast)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            pos = enemy.hexPosition;
            return true;
        }

        pos = Vector3Int.zero;
        return false;
    }
}
