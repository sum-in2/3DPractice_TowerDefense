using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateManager : MonoBehaviour
{
    public List<GameObject> towerButtons;
    public List<GameObject> upgradeButtons;
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
        switch (newState)
        {
            case StateType.Tower:
                ToggleButtons(towerButtons, true);
                ToggleButtons(upgradeButtons, false);
                UpdateButtonLockState(towerButtons);
                break;

            case StateType.Upgrade:
                ToggleButtons(towerButtons, false);
                ToggleButtons(upgradeButtons, true);
                UpdateButtonLockState(upgradeButtons);
                break;

            default:
                centerPanel.SetActive(false);
                rightPanel.SetActive(false);
                break;
        }
    }

    private void ToggleButtons(List<GameObject> buttons, bool isActive)
    {
        if (!centerPanel.activeSelf) centerPanel.SetActive(true);
        if (!rightPanel.activeSelf) rightPanel.SetActive(true);
        if (buttons == null) return;

        foreach (var button in buttons)
        {
            if (button != null)
                button.SetActive(isActive);
        }
    }

    // 새로 추가한 버튼 잠금 상태 갱신 메서드
    public void UpdateButtonLockState(List<GameObject> buttons)
    {
        if (buttons == null) return;

        foreach (GameObject button in buttons)
        {
            if (button == null) continue;

            Button btnComponent = button.GetComponent<Button>();
            Image btnImage = button.GetComponent<Image>();
            GameObject lockIcon = button.transform.Find("LockIcon")?.gameObject;

            if (btnComponent == null || btnImage == null) continue;

            if (System.Enum.TryParse(button.name, out TowerType towerType))
            {
                bool isLocked = SOManager.Instance.IsLocked(towerType);

                btnComponent.interactable = !isLocked;
                btnImage.color = isLocked ? new Color(0.5f, 0.5f, 0.5f, 1f) : Color.white;

                if (lockIcon != null)
                    lockIcon.SetActive(isLocked);
            }
        }
    }
}
