using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UISystem : Generic.Singleton<UISystem>
{
    /// <summary>
    /// UI�� ������ �� �۵��մϴ�.
    /// </summary>
    public virtual void OpenUI() 
    {
    }
    /// <summary>
    /// UI�� �ݾ��� �� �۵��մϴ�.
    /// </summary>
    public virtual void CloseUI()
    {
    }

    /// <summary>
    /// ���� �ý��ۿ��� UI �˾�â�� ������ �� �۵��մϴ�.
    /// </summary>
    public virtual void OpenPopupWindow()
    {
        //UIManager.instance.previousLayer = CurrentLayerNumber;
        //OpenWindow();
    }
    /// <summary>
    /// ���� �ý��ۿ��� UI �˾�â�� �ݾ��� �� �۵��մϴ�.
    /// </summary>
    public virtual void ClosePopupWindow()
    {
        //UIManager.instance.previousLayer = LowLayerNumber;
        //CloseWindow();
    }
}
