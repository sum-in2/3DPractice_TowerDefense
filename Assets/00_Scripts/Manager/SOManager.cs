using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SOManager : Singleton<SOManager>
{
    [SerializeField] List<Tech> towerTech;
    [SerializeField] List<Upgrade> towerUpgrade;

    private Dictionary<TowerType, Tech> towerDict; // Tech가 단순 해금이 아닌 추후에 타워 업그레이드 트리가 생길 수도? 있으? 니까
    private Dictionary<TowerType, List<Upgrade>> upgradeDict;
    private Dictionary<TowerType, bool> lockStates;

    protected override void Awake()
    {
        base.Awake();

        upgradeDict = new Dictionary<TowerType, List<Upgrade>>();
        towerDict = towerTech.ToDictionary(t => t.techType);
        lockStates = new Dictionary<TowerType, bool>();

        foreach (Upgrade upgrade in towerUpgrade)
        {
            if (!upgradeDict.ContainsKey(upgrade.towerType))
                upgradeDict[upgrade.towerType] = new List<Upgrade>();
            upgradeDict[upgrade.towerType].Add(upgrade);
        }

        foreach (var pair in towerDict)
        {
            lockStates[pair.Key] = (pair.Value.level == 0);
        }
    }

    public bool IsLocked(TowerType type)
    {
        return lockStates.ContainsKey(type) && lockStates[type];
    }

    public void SetTowerLevel(TowerType type, int level)
    {
        if (!towerDict.ContainsKey(type)) return;

        towerDict[type].level = level;
        lockStates[type] = (level == 0);
    }
    public void TowerLevelAdder(TowerType type)
    {
        if (!towerDict.ContainsKey(type)) return;

        towerDict[type].level += 1;
        if (!lockStates[type]) lockStates[type] = true;
    }

    private int GetTowerLevel(TowerType type)
    {
        if (towerDict.ContainsKey(type))
            return towerDict[type].level;
        return -1;
    }
}
