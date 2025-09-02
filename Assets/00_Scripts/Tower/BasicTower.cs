using UnityEngine;

public class BasicTower : BaseTower
{
    public Transform firePoint;

    void Awake()
    {
        ObjectPoolManager.Instance.CreatePool(projectilePrefab, 20);
    }
}