using UnityEngine;
using System.Collections;

public abstract class BaseTower : MonoBehaviour, IClickable
{
    public AttackStats attackStats;
    public TowerType towerType;
    public StateType currentState { get; private set; } = StateType.Upgrade;

    public GameObject currentTarget { get; private set; }

    protected IAttackBehavior attackBehavior;

    private Coroutine attackCoroutine;
    public Projectile projectilePrefab;

    protected virtual void Start()
    {
        AttackStats originalStat = SOManager.Instance.GetTowerStat(this.towerType);
        attackStats = new AttackStats(originalStat);

        TowerManager.Instance.RegisterTower(this);
        attackBehavior = TowerAttackBehaviorFactory.Create(towerType);

        StartAttackRoutine();
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
                yield return new WaitForSeconds(1f / attackStats.attackSpeed);
            }
            else
            {
                yield return null;
            }
        }
    }

    private GameObject FindTargetInRange()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, attackStats.range);
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
    }

    public void OnDeselect()
    {
    }

    protected virtual void OnDisable()
    {
        StopAttackRoutine();
    }
}
