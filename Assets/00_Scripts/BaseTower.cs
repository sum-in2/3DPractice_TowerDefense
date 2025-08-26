using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseTower : MonoBehaviour, IClickable
{
    public float attackPower;
    public float range;
    public float attackSpeed;
    public float ignoreDefense;
    public float criticalChance;
    public float criticalDamage;
    public TowerType towerType;
    public StateType CurrentState { get; private set; } = StateType.Upgrade;

    private IAttackBehavior attackBehavior;

    public void SetAttackBehavior(IAttackBehavior behavior)
    {
        attackBehavior = behavior;
    }

    public virtual void Attack()
    {
        if (attackBehavior != null)
            attackBehavior.Attack(this);
    }

    public void OnSelect()
    {
    }

    public void OnDeselect()
    {
    }
}
