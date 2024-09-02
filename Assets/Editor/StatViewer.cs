using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(UnitStat))]
public class UnitStatPropertyDrawer : PropertyDrawer
{
    // �� �迭�� Foldout ���¸� �����ϱ� ���� bool ��
    private bool originalFoldout = true;
    private bool additionalFoldout = true;
    private bool multiplierFoldout = true;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // �⺻ �� ���̿� ���� ����
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float padding = EditorGUIUtility.standardVerticalSpacing;

        float yOffset = position.y;

        // original, _additional, _multiplier ������Ƽ�� ã���ϴ�.
        var originalProp = property.FindPropertyRelative("original");
        var additionalProp = property.FindPropertyRelative("_additional");
        var multiplierProp = property.FindPropertyRelative("_multiplier");

        // original �迭 ��Ӵٿ�
        yOffset = DrawStatArrayFoldout(ref yOffset, position, originalProp, "Original Stats", ref originalFoldout, lineHeight, padding);

        // _additional �迭 ��Ӵٿ�
        yOffset = DrawStatArrayFoldout(ref yOffset, position, additionalProp, "Additional Stats", ref additionalFoldout, lineHeight, padding);

        // _multiplier �迭 ��Ӵٿ�
        yOffset = DrawStatArrayFoldout(ref yOffset, position, multiplierProp, "Multiplier Stats", ref multiplierFoldout, lineHeight, padding);

        EditorGUI.EndProperty();
    }

    private float DrawStatArrayFoldout(ref float yOffset, Rect position, SerializedProperty arrayProp, string label, ref bool foldout, float lineHeight, float padding)
    {
        // Foldout�� �׸��� ���� Rect ����
        Rect foldoutRect = new Rect(position.x, yOffset, position.width, lineHeight);
        foldout = EditorGUI.Foldout(foldoutRect, foldout, label);

        if (foldout)
        {
            EditorGUI.indentLevel++;
            yOffset += lineHeight + padding;

            // �迭�� �� ��� ���
            for (int i = 1; i < arrayProp.arraySize; i++)
            {
                SerializedProperty elementProp = arrayProp.GetArrayElementAtIndex(i);

                Rect rect = new Rect(position.x, yOffset, position.width, lineHeight);
                EditorGUI.PropertyField(rect, elementProp, new GUIContent(((StatType)(i)).ToString()));
                yOffset += lineHeight + padding;
            }

            EditorGUI.indentLevel--;
        }
        else
        {
            yOffset += lineHeight + padding;
        }

        return yOffset;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float padding = EditorGUIUtility.standardVerticalSpacing;

        float totalHeight = 0f;

        // �� �迭 Foldout ���¿� ���� ���� ���
        totalHeight += GetArrayHeight(property.FindPropertyRelative("original"), originalFoldout, lineHeight, padding);
        totalHeight += GetArrayHeight(property.FindPropertyRelative("_additional"), additionalFoldout, lineHeight, padding);
        totalHeight += GetArrayHeight(property.FindPropertyRelative("_multiplier"), multiplierFoldout, lineHeight, padding);

        return totalHeight;
    }

    private float GetArrayHeight(SerializedProperty arrayProp, bool foldout, float lineHeight, float padding)
    {
        float height = lineHeight + padding;

        if (foldout)
        {
            height += (lineHeight + padding) * arrayProp.arraySize;
        }

        return height;
    }
}
