using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TowerInfoUI : MonoBehaviour
{
    [Header("스탯 표시 UI 요소들")]
    [SerializeField] private TextMeshProUGUI towerNameText;
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
    }

    public void UpdateTowerInfo(TowerType towerType, AttackStats attackStats)
    {
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