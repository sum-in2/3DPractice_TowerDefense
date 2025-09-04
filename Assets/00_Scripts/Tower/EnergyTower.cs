using UnityEngine;

public class EnergyTower : BaseTower
{
    public Transform firePoint;

    void Awake()
    {
        ObjectPoolManager.Instance.CreatePool(projectilePrefab, 20);
    }
}