using UnityEngine;
using System;

/// <summary>
/// 인게임 중앙 하단 상태 UI를 관리합니다.
/// </summary>
public class StateManager : MonoBehaviour
{
    public GameObject centerPanel;
    public GameObject rightPanel;

    public delegate void StateChangedHandler(StateType newState);
    public event StateChangedHandler OnStateChanged;

    private StateType currentState = StateType.None;

    public StateType CurrentState
    {
        get => currentState;
        private set
        {
            if (currentState != value)
            {
                currentState = value;
                OnStateChanged?.Invoke(currentState);
            }
        }
    }

    public void SetState(StateType state)
    {
        CurrentState = state;
    }

    private void Start()
    {
        OnStateChanged += HandleStateChanged;
    }

    private void HandleStateChanged(StateType newState)
    {
        Debug.Log("상태 변경됨: " + newState);
        switch (newState)
        {
            case StateType.None:
                centerPanel.SetActive(false);
                rightPanel.SetActive(false);
                break;
            default:
                centerPanel.SetActive(true);
                rightPanel.SetActive(true);
                break;
        }
    }
}
