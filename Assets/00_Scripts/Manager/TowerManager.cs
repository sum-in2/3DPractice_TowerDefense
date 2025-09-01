using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    private List<BaseTower> towerList = new List<BaseTower>();
    private List<Upgrade> upgradeSoList = new List<Upgrade>();

    public void RegisterTower(BaseTower tower)
    {
        if (!towerList.Contains(tower)) towerList.Add(tower);
    }

    public void UpgradeTowers(TowerType towerType, UpgradeType upgradeType)
    {
        if (upgradeSoList.Count == 0)
            upgradeSoList = SOManager.Instance.GetTowerUpgradeSOList(towerType);

        var upgradeTarget = upgradeSoList.FirstOrDefault(u => u.upgradeType == upgradeType);
        if (upgradeTarget == null) return;

        foreach (var tower in towerList.Where(t => t.towerType == towerType))
        {
            tower.attackStats.UpgradeStat(upgradeTarget.upgradeType, upgradeTarget.increaseAmount);
        }
    }
}