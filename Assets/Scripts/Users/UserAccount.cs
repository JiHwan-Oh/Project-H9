using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserAccount 
{
    public static ScriptLanguage Language;

    static UserAccount()
    {
        Debug.Log("�ӽ� UserAccount ������. �� �����ڴ� ���� �����͸� �� �ڴ�� �� �ٲ�Ӵϴ�. ����.");
        Language = UIManager.instance.scriptLanguage;   //��ȯ �׽�Ʈ�ϴµ� �ڵ� �Ź� ���� �����ؼ� �׽�Ʈ������ �ھƵ�
    }

    //public static void Save()
}
