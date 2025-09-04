using UnityEngine;

public class PlasmaChainTower : BaseTower
{
    public Transform firePoint;

    void Awake()
    {
        ObjectPoolManager.Instance.CreatePool(projectilePrefab, 20);
    }
}