using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserAccount 
{
    public static ScriptLanguage Language;

    static UserAccount()
    {
        Debug.Log("�ӽ� UserAccount ������. �� �����ڴ� ���� �����͸� �� �ڴ�� �� �ٲ�Ӵϴ�. ����.");
        Language = ScriptLanguage.Korean;
    }

    //public static void Save()
}
