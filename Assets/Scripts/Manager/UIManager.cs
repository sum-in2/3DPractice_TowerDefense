using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using Unity.VisualScripting;

public class UIManager : Singleton<UIManager>
{
    private PopupManager popupManager;
    private StateManager stateManager;

    protected override void Awake()
    {
        base.Awake();

        popupManager = GetComponentInChildren<PopupManager>();
        stateManager = GetComponentInChildren<StateManager>();
    }

    public void ShowPopup(PopupType popupType)
    {
        popupManager.ShowPopupUI(popupType);
    }

    public void ClosePopup(PopupType popup)
    {
        popupManager.ClosePopupUI(popup);
    }
}
