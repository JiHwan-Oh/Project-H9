using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserAccount 
{
    public static ScriptLanguage Language;

    static UserAccount()
    {
        Debug.Log("임시 UserAccount 생성자. 이 생성자는 유저 데이터를 지 멋대로 쳐 바꿔둡니다. 유의.");
        Language = UIManager.instance.scriptLanguage;   //변환 테스트하는데 코드 매번 열기 불편해서 테스트용으로 박아둠
    }

    //public static void Save()
}
