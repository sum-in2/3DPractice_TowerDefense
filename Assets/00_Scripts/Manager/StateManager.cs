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
    [SerializeField] private TowerInfoUI towerInfoUI;

    public void SetState(StateType state)
    {
        CurrentState = state;
    }

    private void Start()
    {
        OnStateChanged += HandleStateChanged;

        if (towerInfoUI != null)
            towerInfoUI.Initialize(this);
    }

    private void OnEnable()
    {
        TowerBtnHover.OnTowerHover += HandleTowerHover;
        TowerBtnHover.OnTowerHoverExit += HandleTowerHoverExit;

        SOManager.Instance.OnTowerUpgraded += HandleTowerUpgraded;
    }

    private void OnDisable()
    {
        TowerBtnHover.OnTowerHover -= HandleTowerHover;
        TowerBtnHover.OnTowerHoverExit -= HandleTowerHoverExit;

        SOManager.Instance.OnTowerUpgraded -= HandleTowerUpgraded;
    }

    private void HandleTowerUpgraded(TowerType towerType)
    {
        BaseTower clickable = ClickManager.Instance.nowClickObject as BaseTower;
        if (currentState == StateType.TowerSelect && clickable.towerType == towerType)
            RefreshSelectedTowerInfo();
    }

    private void HandleTowerHover(TowerType towerType)
    {
        if (CurrentState == StateType.TowerSpotSelect && towerInfoUI != null)
            towerInfoUI.UpdateTowerInfo(towerType);
    }

    private void HandleTowerHoverExit()
    {
        if (CurrentState == StateType.TowerSpotSelect && towerInfoUI != null)
            towerInfoUI.HideTowerInfo();
    }

    private void HandleStateChanged(StateType newState)
    {
        switch (newState)
        {
            case StateType.TowerSpotSelect:
                ToggleButtons(towerButtons, true);
                ToggleButtons(upgradeButtons, false);
                UpdateButtonLockState(towerButtons);
                break;

            case StateType.TowerSelect:
                ToggleButtons(towerButtons, false);
                ToggleButtons(upgradeButtons, true);
                UpdateButtonLockState(upgradeButtons);

                ShowSelectedTowerInfo();
                break;

            default:
                centerPanel.SetActive(false);
                rightPanel.SetActive(false);
                if (towerInfoUI != null)
                    towerInfoUI.HideTowerInfo();
                break;
        }
    }

    private void ShowSelectedTowerInfo()
    {
        IClickable clickable = ClickManager.Instance.nowClickObject;

        if (towerInfoUI != null && clickable is BaseTower tower)
            towerInfoUI.UpdateTowerInfo(tower.towerType);
    }

    private void ToggleButtons(List<GameObject> buttons, bool isActive)
    {
        if (!rightPanel.activeSelf) rightPanel.SetActive(true);
        if (buttons == null) return;

        foreach (var button in buttons)
        {
            if (button != null)
                button.SetActive(isActive);
        }
    }

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

    public void RefreshSelectedTowerInfo()
    {
        if (currentState == StateType.TowerSelect)
            ShowSelectedTowerInfo();
    }

    public void OnTowerPlaced(BaseTower tower)
    {
        if (tower != null)
            currentState = StateType.TowerSelect;
    }
}
