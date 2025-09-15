public class EnergyTowerAttack : IAttackBehavior
{
    public void Attack(BaseTower tower)
    {
        var energyTower = tower as PlasmaChainTower;
        if (energyTower == null)
            return;

        if (energyTower.currentTarget == null)
            return;

        Projectile proj = ObjectPoolManager.Instance.GetObject(energyTower.projectilePrefab);
        proj.transform.position = energyTower.firePoint.position + new UnityEngine.Vector3(0, 0.5f, 0);
        proj.SetTarget(energyTower.currentTarget);
        proj.SetDamage(energyTower.currentAttackStats);
        proj.gameObject.SetActive(true);
    }
}