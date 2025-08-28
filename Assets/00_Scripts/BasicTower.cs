using UnityEngine;

public class BasicTower : BaseTower
{
    public Transform firePoint;
    public GameObject currentTarget { get; private set; }

    void Awake()
    {
        attackStats = new AttackStats(5, 3, 1, 0, 20, 1.5f);
        ObjectPoolManager.Instance.CreatePool(projectilePrefab, 20);
    }

    protected override bool HasTarget()
    {
        currentTarget = FindTargetInRange();
        return currentTarget != null;
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

}
