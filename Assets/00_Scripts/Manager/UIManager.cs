using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using Unity.VisualScripting;
using System.Data;

public class UIManager : Singleton<UIManager>
{
    private PopupManager popupManager;
    private StateManager stateManager;

    protected override void Awake()
    {
        base.Awake();

        popupManager = GetComponentInChildren<PopupManager>();
        stateManager = GetComponentInChildren<StateManager>();
        UIEvents.OnStateChangeRequested += ChangeState;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        UIEvents.OnStateChangeRequested -= ChangeState;
    }

    public void ShowPopup(PopupType popupType)
    {
        popupManager.ShowPopupUI(popupType);
    }

    public void ClosePopup(PopupType popup)
    {
        popupManager.ClosePopupUI(popup);
    }

    public void ChangeState(StateType state)
    {
        stateManager.SetState(state);
    }
}
