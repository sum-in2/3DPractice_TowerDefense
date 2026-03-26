using UnityEngine;

/// <summary>
/// 타워 수치 업그레이드 SO
/// </summary>

[CreateAssetMenu(menuName = "Game/Upgrade")]
public class Upgrade : ScriptableObject
{
    public TowerType towerType;
    public UpgradeType upgradeType;
    public float increaseAmount;
    public int cost;
    public int level;

    public bool TryUpgrade()
    {
        if (!PlayerManager.Instance.TrySpendGold(cost))
            return false;

        level++;
        SOManager.Instance.ApplyGlobalUpgrade(towerType, upgradeType, increaseAmount);
        return true;
    }
}
