using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// ���� �� ���� ���۵Ǵ� ���� ������ ȭ�� �߾ӿ� �ؽ�Ʈ�� ǥ���ϴ� ����� �����ϴ� Ŭ����
/// </summary>
public class StartTurnTextUI : MonoBehaviour
{
    public TextMeshProUGUI turnText;
    // Start is called before the first frame update
    void Start()
    {
        InitTurnText();
    }

    private void InitTurnText()
    {
        turnText.text = "";
        turnText.enabled = false;
    }

    /// <summary>
    /// ���� ���۵Ǵ� ���� ������ Unit Ŭ������ �Է¹����� �׿� �´� �ؽ�Ʈ�� ȭ�鿡 ����մϴ�.
    /// ���� ���� ���� ���۵� ������ ����˴ϴ�.
    /// </summary>
    /// <param name="unit"> ���� ���۵Ǵ� �� ���� </param>
    public void SetStartTurnTextUI(Unit unit) 
    {
        if (!GameManager.instance.CompareState(GameState.Combat)) return;

        StopAllCoroutines();
        if (unit is Player)
        {
            StartCoroutine(ShowTurnText("Your Turn!", 2.0f));
            
        }
        else
        {
            StartCoroutine(ShowTurnText("Enemy Turn", 2.0f));
        }
    }

    IEnumerator ShowTurnText(string text, float time) 
    {
        while (true)
        {
            turnText.enabled = true;
            turnText.text = text;
            yield return new WaitForSeconds(time);
            InitTurnText();
            yield break;
        }
    }
}
