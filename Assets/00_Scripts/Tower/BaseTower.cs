using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;

public abstract class BaseTower : MonoBehaviour, IClickable
{
    public AttackStats baseAttackStats;
    public AttackStats currentAttackStats;

    public List<IndividualUpgrade> individualUpgrades = new List<IndividualUpgrade>();
    public TowerType towerType;
    public StateType currentState { get; private set; } = StateType.TowerSelect;

    public GameObject currentTarget { get; private set; }

    protected IAttackBehavior attackBehavior;
    private Coroutine attackCoroutine;
    public Projectile projectilePrefab;
    private PreviewRange previewRangeObject;

    protected virtual void Start()
    {
        StatInit();
        TowerManager.Instance.RegisterTower(this);
        attackBehavior = TowerAttackBehaviorFactory.Create(towerType);
        previewRangeObject = gameObject.GetComponentInChildren<PreviewRange>(true);
        OnSelect();
        StartAttackRoutine();
    }

    void StatInit()
    {
        AttackStats runtimeStat = SOManager.Instance.GetTowerDefaultStat(this.towerType);
        baseAttackStats = new AttackStats(runtimeStat);

        RefreshCurrentStats();
    }

    public void RefreshCurrentStats()
    {
        currentAttackStats = new AttackStats(baseAttackStats);
        ApplyIndividualUgrades();
    }

    void ApplyIndividualUgrades()
    {
        foreach (IndividualUpgrade upgrade in individualUpgrades)
        {
            if (upgrade.isApplied)
            {
                currentAttackStats.UpgradeStat(upgrade.upgradeType, upgrade.increaseAmount);
            }
        }
    }

    public void AddIndividualUpgrade(UpgradeType upgradeType, float increaseAmount, int cost, string upgradeName)
    {
        IndividualUpgrade newUpgrade = new IndividualUpgrade
        {
            upgradeType = upgradeType,
            increaseAmount = increaseAmount,
            cost = cost,
            upgradeName = upgradeName,
            isApplied = true
        };

        individualUpgrades.Add(newUpgrade);
        RefreshCurrentStats();

        if (upgradeType == UpgradeType.RangeUp)
        {
            previewRangeObject.SetRangeObjectState(currentAttackStats.range, true);
        }
    }

    protected void StartAttackRoutine()
    {
        if (attackCoroutine == null)
            attackCoroutine = StartCoroutine(AttackRoutine());
    }

    protected void StopAttackRoutine()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    protected virtual IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (HasTarget())
            {
                attackBehavior?.Attack(this);
                yield return new WaitForSeconds(1f / currentAttackStats.attackSpeed);
            }
            else
            {
                yield return null;
            }
        }
    }

    private GameObject FindTargetInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, currentAttackStats.range);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
                return hit.gameObject;
        }
        return null;
    }

    private bool HasTarget()
    {
        currentTarget = FindTargetInRange();
        return currentTarget != null;
    }

    public void OnSelect()
    {
        previewRangeObject.SetRangeObjectState(currentAttackStats.range, true);
    }

    public void OnDeselect()
    {
        previewRangeObject.SetRangeObjectState(0, false);
    }

    protected virtual void OnDisable()
    {
        StopAttackRoutine();
    }
}
