using UnityEngine;
using UnityEngine.InputSystem;

public enum TowerType
{

}

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
        if (attackBehavior != null)
            attackBehavior.Attack(this);
    }
}
