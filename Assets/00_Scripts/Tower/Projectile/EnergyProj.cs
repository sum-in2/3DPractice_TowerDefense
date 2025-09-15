using UnityEngine;

public class EnergyProj : Projectile
{
    private float splashRadius = 1.5f;
    public ParticleSystem splash;

    protected override void HitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            float finalDamage = damage;
            if (Random.value < criticalChance)
                finalDamage *= criticalDamage;
            enemy.TakeDamage(finalDamage);
            SplashDamage(enemy, finalDamage * 0.7f);
        }

        // TODO : 스플래시 이펙트
        ObjectPoolManager.Instance.ReturnObject(this as Projectile);
    }
    void OnDrawGizmos()
    {
        //TODO : 테스트 끝나면 없앨 것
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.transform.position, splashRadius);
    }

    void SplashDamage(Enemy targetEnemy, float finalDamage)
    {
        Vector3 targetPosition = targetEnemy.gameObject.transform.position;
        Collider[] enemies = Physics.OverlapSphere(targetPosition, splashRadius);

        SpawnSplashParticle(targetPosition);

        foreach (Collider col in enemies)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy == null) continue;

            enemy.TakeDamage(finalDamage);

        }

        void SpawnSplashParticle(Vector3 position)
        {
            ParticleSystem ps = Instantiate(splash, position, Quaternion.identity);

            if (ps != null)
            {
                Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
            }
            else
            {
                Destroy(ps.gameObject, 2f);
            }
        }

    }
}