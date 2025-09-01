using UnityEngine;

public class BasicTower : BaseTower
{
    public Transform firePoint;

    void Awake()
    {
        attackStats = new AttackStats(5, 3, 1, 0, 20, 1.5f);
        ObjectPoolManager.Instance.CreatePool(projectilePrefab, 20);
    }
}