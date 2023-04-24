using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class SkillRead : MonoBehaviour
{
	static string SPLIT_RE = @",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))"; //regular expression
	static string LINE_SPLIT_RE = @"\r\n|\n\r|\n|\r";
	static char[] TRIM_CHARS = { '\"' };

	public static List<List<string>> Read(string file)
	{
		TextAsset data = Resources.Load("files/" + file) as TextAsset;
        if (data == null)
        {
            Debug.Log("������ �������� �ʽ��ϴ�.");
            return null;
        }

        List<List<string>> rowList = new List<List<string>>();
		string[] lines = Regex.Split(data.text, LINE_SPLIT_RE);

        if (lines.Length <= 1)
        {
            Debug.Log("���� ������ �������� �ʽ��ϴ�.");
            return null;
        }

        string[] header = Regex.Split(lines[0], SPLIT_RE);
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = Regex.Split(lines[i], SPLIT_RE);
            if (values.Length == 0 || values[0] == "") continue;

            List<string> columnList = new List<string>();
            for (int j = 0; j < values.Length; j++)
            {
                string value = values[j];
                //value = value.TrimStart(TRIM_CHARS).TrimEnd(TRIM_CHARS).Replace("\\", "");
                string finalvalue = value;
                columnList.Add(finalvalue);
            }
            rowList.Add(columnList);
        }
        return rowList;
	}
}
