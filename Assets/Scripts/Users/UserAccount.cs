using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserAccount 
{
    public static ScriptLanguage Language;

    static UserAccount()
    {
        Debug.Log("�ӽ� UserAccount ������. �� �����ڴ� ���� �����͸� �� �ڴ�� �� �ٲ�Ӵϴ�. ����.");
        if (Language == ScriptLanguage.NULL) Language = UIManager.instance.scriptLanguage;
        else UIManager.instance.scriptLanguage = Language;
    }

    //public static void Save()
}
