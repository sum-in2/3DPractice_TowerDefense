using System.Runtime.Serialization;
using UnityEngine;

public class PlasmaChainTower : BaseTower
{
    public Transform firePoint;

    void Awake()
    {
        ObjectPoolManager.Instance.CreatePool(projectilePrefab, 20);

        ChainProj chainProj = projectilePrefab.GetComponent<ChainProj>();
        if (chainProj != null && chainProj.vfxPrefab != null)
            ObjectPoolManager.Instance.CreatePool(chainProj.vfxPrefab, 30);
    }
}