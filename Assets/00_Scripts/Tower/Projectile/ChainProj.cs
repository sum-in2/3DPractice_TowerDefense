using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VFX;
using UnityEngine.PlayerLoop;

public class ChainProjectile : Projectile
{
    public int maxChainCount = 3;
    public float chainRange = 5f;
    public float chainDamageReduction = 0.8f;

    public VisualEffect lightningVFX;

    private int currentChainCount = 0;
    private List<Enemy> hitEnemies = new List<Enemy>();

    void OnEnable()
    {
        currentChainCount = 0;
        hitEnemies.Clear();

        if (lightningVFX != null)
        {
            lightningVFX.Stop();
            lightningVFX.Reinit();
        }
    }

    void OnDisable()
    {
        if (lightningVFX != null)
            lightningVFX.Stop();
    }

    public override void SetTarget(GameObject target)
    {
        base.SetTarget(target);
        PlayLightningEffect(this.transform.position, target.transform.position);
    }

    protected override void HitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            hitEnemies.Add(enemy);

            float finalDamage = damage * Mathf.Pow(chainDamageReduction, currentChainCount);

            if (Random.value < criticalChance * 0.01f)
                finalDamage *= criticalDamage;

            enemy.TakeDamage(finalDamage);

            if (currentChainCount < maxChainCount)
            {
                Enemy nextTarget = FindNextTarget(enemy.transform.position);
                if (nextTarget != null)
                {
                    ChainToNextTarget(nextTarget);
                    return;
                }
            }
        }

        ObjectPoolManager.Instance.ReturnObject(this as Projectile);
    }

    private Enemy FindNextTarget(Vector3 currentPosition)
    {
        Collider[] colliders = Physics.OverlapSphere(currentPosition, chainRange);
        Enemy closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Collider col in colliders)
        {
            Enemy enemy = col.GetComponent<Enemy>();
            if (enemy != null && !hitEnemies.Contains(enemy))
            {
                float distance = Vector3.Distance(currentPosition, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;
    }

    private void ChainToNextTarget(Enemy nextTarget)
    {
        currentChainCount++;

        Vector3 currentPos = transform.position;
        Vector3 endPos = nextTarget.transform.position;

        PlayLightningEffect(currentPos, endPos);

        target = nextTarget.gameObject;

        Vector3 direction = (target.transform.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void PlayLightningEffect(Vector3 startPos, Vector3 endPos)
    {
        if (lightningVFX != null)
        {
            VFXEventAttribute eventAttribute = lightningVFX.CreateVFXEventAttribute();

            endPos.y = startPos.y;
            lightningVFX.SetVector3("StartPosition", startPos);
            lightningVFX.SetVector3("EndPosition", endPos);

            lightningVFX.SendEvent("OnPlay", eventAttribute);
        }
    }
}