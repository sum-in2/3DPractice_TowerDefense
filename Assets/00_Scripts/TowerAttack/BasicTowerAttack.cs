public class BasicTowerAttack : IAttackBehavior
{
    public void Attack(BaseTower tower)
    {
        var basicTower = tower as BasicTower;
        if (basicTower == null)
            return;

        if (basicTower.currentTarget == null)
            return;

        Projectile proj = ObjectPoolManager.Instance.GetObject(basicTower.projectilePrefab);
        proj.transform.position = basicTower.firePoint.position;
        proj.SetTarget(basicTower.currentTarget);
        proj.SetDamage(basicTower.attackStats);
        proj.gameObject.SetActive(true);
    }
}