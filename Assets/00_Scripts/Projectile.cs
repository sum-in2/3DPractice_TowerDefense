using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject target;
    private float speed = 10f;

    private float damage;
    private float criticalChance;
    private float criticalDamage;

    public void SetTarget(GameObject target)
    {
        this.target = target;
        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(90, 0, 0);
    }


    public void SetDamage(float damage, float criticalChance, float criticalDamage)
    {
        // TODO : 방어력 관통
        this.damage = damage;
        this.criticalChance = criticalChance;
        this.criticalDamage = criticalDamage;
    }

    void Update()
    {
        if (target == null)
        {
            ObjectPoolManager.Instance.ReturnObject(this);
            return;
        }

        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, target.transform.position) < 0.5f)
            HitTarget();
    }

    private void HitTarget()
    {
        var enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            float finalDamage = damage;
            if (Random.value < criticalChance * 0.01f)
                finalDamage *= criticalDamage;
            enemy.TakeDamage(finalDamage);
        }

        ObjectPoolManager.Instance.ReturnObject(this);
    }
}