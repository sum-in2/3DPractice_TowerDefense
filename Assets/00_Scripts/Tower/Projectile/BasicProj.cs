using UnityEngine;

public class BasicProj : Projectile
{
    protected override void HitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            float finalDamage = damage;
            if (Random.value < criticalChance)
                finalDamage *= criticalDamage;
            enemy.TakeDamage(finalDamage);
        }
        ObjectPoolManager.Instance.ReturnObject(this as Projectile);
    }
}