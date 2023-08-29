using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIElement : MonoBehaviour
{
    /// <summary>
    /// UI�� ������ �� �۵��մϴ�.
    /// </summary>
    public virtual void OpenUI()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// UI�� �ݾ��� �� �۵��մϴ�.
    /// </summary>
    public virtual void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
