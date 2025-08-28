using UnityEngine;
using System.Collections;

public abstract class BaseTower : MonoBehaviour, IClickable
{
    public AttackStats attackStats;
    public TowerType towerType;
    public StateType CurrentState { get; private set; } = StateType.Upgrade;

    protected IAttackBehavior attackBehavior;

    private Coroutine attackCoroutine;
    public Projectile projectilePrefab;

    protected virtual void Start()
    {
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

    protected abstract bool HasTarget();

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
