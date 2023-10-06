using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UIElement : MonoBehaviour
{
    protected bool isOpenUI;
    /// <summary>
    /// UI�� ������ �� �۵��մϴ�.
    /// </summary>
    public virtual void OpenUI()
    {
        gameObject.SetActive(true);
        isOpenUI = true;
    }
    /// <summary>
    /// UI�� �ݾ��� �� �۵��մϴ�.
    /// </summary>
    public virtual void CloseUI()
    {
        isOpenUI = false;
        gameObject.SetActive(false);
    }

    public virtual bool IsInteractable() 
    {
        return true;
    }
}
