using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SOManager : Singleton<SOManager>
{
    // 만약에 SO가 늘면 늘수록 부담이 커질 것 같음
    // SO당 핸들러로 분할 후 매니저에선 핸들러를 종합해서 사용하는게 나을듯
    [SerializeField] List<Tech> towerTech;
    [SerializeField] List<Upgrade> towerUpgrade;
    private Dictionary<TowerType, Tech> towerDict;
    private Dictionary<TowerType, List<Upgrade>> upgradeDict;

    protected override void Awake()
    {
        base.Awake();
        upgradeDict = new Dictionary<TowerType, List<Upgrade>>();
        towerDict = towerTech.ToDictionary(t => t.techType);

        foreach (Upgrade upgrade in towerUpgrade)
        {
            if (!upgradeDict.ContainsKey(upgrade.towerType))
                upgradeDict[upgrade.towerType] = new List<Upgrade>();
            upgradeDict[upgrade.towerType].Add(upgrade);
        }
    }

    private int GetTowerLevel(TowerType type)
    {
        if (towerDict.ContainsKey(type))
            return towerDict[type].level;
        return -1;
    }
}
