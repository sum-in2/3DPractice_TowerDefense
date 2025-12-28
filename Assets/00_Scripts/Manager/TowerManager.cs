using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    private List<BaseTower> towerList = new List<BaseTower>();
    private ITowerUpgradeNotifier towerNotifier;

    void InitializeNotifier()
    {
        if (towerNotifier == null)
            towerNotifier = SOManager.Instance as ITowerUpgradeNotifier;
    }


    void OnEnable()
    {
        InitializeNotifier();
        towerNotifier.OnTowerUpgraded += RefreshTowersOfType;
    }

    void OnDisable()
    {
        if (towerNotifier != null)
            towerNotifier.OnTowerUpgraded -= RefreshTowersOfType;
    }

    public void RegisterTower(BaseTower tower)
    {
        if (!towerList.Contains(tower))
            towerList.Add(tower);
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

    private void RefreshTowersOfType(TowerType towerType)
    {
        InitializeNotifier();
        List<BaseTower> towersOfType = GetTowersOfType(towerType);
        foreach (BaseTower tower in towersOfType)
        {
            tower.baseAttackStats = new AttackStats(SOManager.Instance.GetTowerRuntimeStat(towerType));
            tower.RefreshCurrentStats();
        }
    }
}