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
        proj.transform.position = basicTower.firePoint.position + new UnityEngine.Vector3(0, 0.5f, 0);
        proj.SetTarget(basicTower.currentTarget);
        proj.SetDamage(basicTower.currentAttackStats);
        proj.gameObject.SetActive(true);
    }
}