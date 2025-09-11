using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    private List<BaseTower> towerList = new List<BaseTower>();

    public void RegisterTower(BaseTower tower)
    {
        if (!towerList.Contains(tower))
            towerList.Add(tower);
    }

    public void UpgradeTowers(TowerType towerType, UpgradeType upgradeType, float increaseAmount)
    {
        SOManager.Instance.ApplyGlobalUpgrade(towerType, upgradeType, increaseAmount);
    }

    public List<BaseTower> GetTowersOfType(TowerType towerType)
    {
        return towerList.Where(t => t.towerType == towerType).ToList();
    }

    public void RemoveTower(BaseTower tower)
    {
        if (towerList.Contains(tower))
            towerList.Remove(tower);
    }
}