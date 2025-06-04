using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;

public class UIManager : Singleton<UIManager>
{
    public T ShowPopup<T>(PopupType popupType) where T : UIPopup
    {
        return PopupManager.Instance.ShowPopupUI<T>(popupType);
    }

    public void ClosePopup(UIPopup popup)
    {
        PopupManager.Instance.ClosePopupUI(popup);
    }
}
