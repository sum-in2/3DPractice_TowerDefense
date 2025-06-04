using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseTower : MonoBehaviour
{
    public float attackPower;
    public float range;
    public float attackSpeed;
    public float ignoreDefense;
    public float criticalChance;
    public float criticalDamage;
    public TowerType towerType;

    private IAttackBehavior attackBehavior;

    public void SetAttackBehavior(IAttackBehavior behavior)
    {
        attackBehavior = behavior;
    }

    public virtual void Attack()
    {
        // 근데 이거 어케 만들어야 되지
        // 투사체가 목표물을 향해 계속 방향이 틀어지면 보이는게 좀 이상하지않나?
        if (attackBehavior != null)
            attackBehavior.Attack(this);
    }
}
