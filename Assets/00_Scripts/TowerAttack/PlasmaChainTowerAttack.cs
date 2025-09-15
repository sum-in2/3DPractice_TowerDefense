public class PlasmaChainTowerAttack : IAttackBehavior
{
    public void Attack(BaseTower tower)
    {
        var plasmaTower = tower as PlasmaChainTower;
        if (plasmaTower == null)
            return;

        if (plasmaTower.currentTarget == null)
            return;

        Projectile proj = ObjectPoolManager.Instance.GetObject(plasmaTower.projectilePrefab);
        proj.transform.position = plasmaTower.firePoint.position + new UnityEngine.Vector3(0, 0.5f, 0);
        proj.SetTarget(plasmaTower.currentTarget);
        proj.SetDamage(plasmaTower.currentAttackStats);
        proj.gameObject.SetActive(true);
    }
}