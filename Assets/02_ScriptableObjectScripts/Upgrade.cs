using UnityEngine;

/// <summary>
/// 타워 수치 업그레이드 SO
/// </summary>

[CreateAssetMenu(menuName = "Game/Upgrade")]
public class Upgrade : ScriptableObject
{
    [SerializeField] private TowerType towerType;
    [SerializeField] private UpgradeType upgradeType;
    [SerializeField] private float increaseAmount;
    [SerializeField] private int cost;
    [SerializeField] private int level;

    public TowerType TowerType => towerType;
    public UpgradeType UpgradeType => upgradeType;
    public float IncreaseAmount => increaseAmount;
    public int Cost => cost;
    public int Level => level;

    public void ResetLevel() => level = 0;

    public bool TryUpgrade()
    {
        if (!PlayerManager.Instance.TrySpendGold(cost))
            return false;

        level++;
        SOManager.Instance.ApplyGlobalUpgrade(towerType, upgradeType, increaseAmount);
        return true;
    }
}
