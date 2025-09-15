using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SOManager : Singleton<SOManager>
{
    [SerializeField] List<Tech> towerTech;
    [SerializeField] List<Upgrade> towerUpgrade;
    [SerializeField] List<TowerStat> towerDefaultStats;
    [SerializeField] List<TowerStat> towerRunTimeStats;

    private Dictionary<TowerType, Tech> towerTechDict; // Tech가 단순 해금이 아닌 추후에 타워 업그레이드 트리가 생길 수도? 있으? 니까
    private Dictionary<TowerType, List<Upgrade>> upgradeSODict;
    private Dictionary<TowerType, bool> lockStates;
    private Dictionary<TowerType, AttackStats> defaultStatDict;
    private Dictionary<TowerType, AttackStats> runtimeStatDict;

    public System.Action<TowerType> OnTowerUpgraded;

    protected override void Awake()
    {
        base.Awake();

        upgradeSODict = new Dictionary<TowerType, List<Upgrade>>();
        defaultStatDict = towerDefaultStats.ToDictionary(t => t.towerType, t => t.attackStats);
        runtimeStatDict = towerRunTimeStats.ToDictionary(t => t.towerType, t => t.attackStats);
        towerTechDict = towerTech.ToDictionary(t => t.techType);
        lockStates = new Dictionary<TowerType, bool>();

        foreach (Upgrade upgrade in towerUpgrade)
        {
            if (!upgradeSODict.ContainsKey(upgrade.towerType))
                upgradeSODict[upgrade.towerType] = new List<Upgrade>();
            upgradeSODict[upgrade.towerType].Add(upgrade);
        }

        foreach (var pair in towerTechDict)
        {
            lockStates[pair.Key] = (pair.Value.level == 0);
        }

        UpgradeSOLevelInit();
        CopyAllDefaultToRuntime();
    }

    public void CopyAllDefaultToRuntime()
    {
        foreach (KeyValuePair<TowerType, AttackStats> kvp in defaultStatDict)
        {
            runtimeStatDict[kvp.Key] = new AttackStats(kvp.Value);
        }
    }

    public AttackStats GetTowerDefaultStat(TowerType towerType)
    {
        return defaultStatDict[towerType];
    }

    public AttackStats GetTowerRuntimeStat(TowerType towerType)
    {
        return runtimeStatDict[towerType];
    }

    private void UpgradeSOLevelInit()
    {
        // TODO: 후에 세이브/로드 시에 SO 데이터 로드해서 적용

        foreach (Upgrade upgrade in towerUpgrade)
        {
            upgrade.level = 0;
        }
    }

    public bool IsLocked(TowerType type)
    {
        return lockStates.ContainsKey(type) && lockStates[type];
    }

    public void SetTowerLevel(TowerType type, int level)
    {
        if (!towerTechDict.ContainsKey(type)) return;

        towerTechDict[type].level = level;
        lockStates[type] = (level == 0);
    }
    public void TowerLevelAdder(TowerType type)
    {
        if (!towerTechDict.ContainsKey(type)) return;

        towerTechDict[type].level += 1;
        if (!lockStates[type]) lockStates[type] = true;
    }

    private int GetTowerLevel(TowerType type)
    {
        if (towerTechDict.ContainsKey(type))
            return towerTechDict[type].level;
        return -1;
    }

    public List<Upgrade> GetTowerUpgradeSOList(TowerType towerType)
    {
        if (upgradeSODict.ContainsKey(towerType))
            return upgradeSODict[towerType];

        return null;
    }

    public void ApplyGlobalUpgrade(TowerType towerType, UpgradeType upgradeType, float increaseAmount)
    {
        runtimeStatDict[towerType].UpgradeStat(upgradeType, increaseAmount);
        RefreshTowersOfType(towerType);

        OnTowerUpgraded?.Invoke(towerType);
    }

    private void RefreshTowersOfType(TowerType towerType)
    {
        List<BaseTower> towersOfType = TowerManager.Instance.GetTowersOfType(towerType);
        foreach (BaseTower tower in towersOfType)
        {
            tower.baseAttackStats = new AttackStats(runtimeStatDict[towerType]);
            tower.RefreshCurrentStats();
        }
    }

    public Tech GetTowerTech(TowerType towerType)
    {
        return towerTechDict.ContainsKey(towerType) ? towerTechDict[towerType] : null;
    }
}
