using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.UIElements;
using Unity.VisualScripting;
public class TowerInfoUI : MonoBehaviour
{
    [Header("스탯 표시 UI")]
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerDescriptionText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI ignoreDefText;
    [SerializeField] private TextMeshProUGUI critChanceText;
    [SerializeField] private TextMeshProUGUI critDamageText;

    [Header("UI 패널")]
    [SerializeField] private GameObject towerInfoPanel;

    private StateManager stateManager;

    public void Initialize(StateManager manager)
    {
        stateManager = manager;
        HideTowerInfo();

        UIEvents.OnStateChangeRequested += HandleStateChange;

        TowerBtnHover.OnTowerHover += HandleTowerHover;
        TowerBtnHover.OnTowerHoverExit += HandleTowerHoverExit;
    }

    void OnDestroy()
    {
        UIEvents.OnStateChangeRequested -= HandleStateChange;

        TowerBtnHover.OnTowerHover -= HandleTowerHover;
        TowerBtnHover.OnTowerHoverExit -= HandleTowerHoverExit;
    }

    void HandleTowerHover(TowerType towerType, AttackStats attackStats)
    {
        IClickable clickable = ClickManager.Instance.nowClickObject;
        if (clickable != null && clickable.currentState == StateType.TowerSpotSelect)
        {
            UpdateTowerInfo(towerType, attackStats);
        }
    }

    void HandleTowerHoverExit()
    {
        IClickable clickable = ClickManager.Instance.nowClickObject;
        if (clickable != null && clickable.currentState == StateType.TowerSpotSelect)
        {
            HideTowerInfo();
        }
    }

    void HandleStateChange(StateType newState)
    {
        if (newState == StateType.TowerSelect)
        {
            IClickable clickable = ClickManager.Instance.nowClickObject;
            if (clickable is BaseTower tower)
            {
                UpdateTowerInfo(tower.towerType, tower.currentAttackStats);
            }
        }
        else
        {
            HideTowerInfo();
        }
    }

    public void UpdateTowerInfo(TowerType towerType, AttackStats attackStats)
    {
        string towerDescription = GetTowerDescription(towerType);
        if (towerDescription != null)
            towerDescriptionText.text = towerDescription;

        if (towerNameText != null)
            towerNameText.text = towerType.ToString();

        if (damageText != null)
            damageText.text = attackStats.attackPower.ToString();

        if (rangeText != null)
            rangeText.text = attackStats.range.ToString("F1");

        if (attackSpeedText != null)
            attackSpeedText.text = attackStats.attackSpeed.ToString("F1") + "/s";

        if (ignoreDefText != null)
            ignoreDefText.text = attackStats.ignoreDefense.ToString();

        if (critChanceText != null)
            critChanceText.text = (attackStats.critChance * 100f).ToString("F0") + "%";

        if (critDamageText != null)
            critDamageText.text = attackStats.critDamage.ToString("F1") + "x";

        ShowTowerInfo();
    }

    private string GetTowerDescription(TowerType towerType)
    {
        return SOManager.Instance.GetTowerTech(towerType)?.description;
    }

    public void ShowTowerInfo()
    {
        if (towerInfoPanel != null)
            towerInfoPanel.SetActive(true);
    }

    public void HideTowerInfo()
    {
        if (towerInfoPanel != null)
            towerInfoPanel.SetActive(false);
    }
}