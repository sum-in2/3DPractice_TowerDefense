using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public List<GameObject> towerButtons;
    public List<GameObject> upgradeButtons;

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
        switch (newState)
        {
            case StateType.Tower:
                ToggleButtons(towerButtons, true);
                ToggleButtons(upgradeButtons, false);
                break;

            case StateType.Upgrade:
                ToggleButtons(towerButtons, false);
                ToggleButtons(upgradeButtons, true);
                break;

            default:
                ToggleButtons(towerButtons, false);
                ToggleButtons(upgradeButtons, false);
                break;
        }
    }

    private void ToggleButtons(List<GameObject> buttons, bool isActive)
    {
        if (buttons == null) return;
        foreach (var button in buttons)
        {
            if (button != null)
                button.SetActive(isActive);
        }
    }
}
