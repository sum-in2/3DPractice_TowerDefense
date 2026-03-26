using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VFX;
using System.Collections;

public class ChainProj : Projectile
{
    public int maxChainCount = 3;
    public float chainRange = 5f;
    public float chainDamageReduction = 0.8f;
    public float chainDelay = 0.01f;

    public ChainVFXEffect vfxPrefab;

    private int currentChainCount = 0;
    private List<Enemy> hitEnemies = new List<Enemy>();
    private Vector3 fixedNextTargetPos;

    void OnEnable()
    {
        currentChainCount = 0;
        hitEnemies.Clear();
    }

    public override void SetTarget(GameObject target)
    {
        this.target = target;

        PlayLightningEffect(transform.position, target.transform.position);

        transform.position = target.transform.position;
        HitTarget();
    }

    protected override void Update()
    {

    }

    protected override void HitTarget()
    {
        Enemy enemy = target.GetComponent<Enemy>();
        if (enemy != null)
        {
            hitEnemies.Add(enemy);

            float finalDamage = damage * Mathf.Pow(chainDamageReduction, currentChainCount);

            if (Random.value < criticalChance)
                finalDamage *= criticalDamage;

            enemy.TakeDamage(finalDamage);

            if (currentChainCount < maxChainCount)
            {
                Enemy nextTarget = FindNextTarget(enemy.transform.position);
                if (nextTarget != null)
                {
                    StartCoroutine(ChainToNextTargetWithDelay(nextTarget));
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
            if (enemy != null && enemy.gameObject.activeInHierarchy && !hitEnemies.Contains(enemy))
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

    private IEnumerator ChainToNextTargetWithDelay(Enemy nextTarget)
    {
        Vector3 currentPos = transform.position;
        Vector3 fixedNextPos = nextTarget.transform.position;

        PlayLightningEffect(currentPos, fixedNextPos);

        yield return new WaitForSeconds(chainDelay);

        if (!nextTarget.gameObject.activeInHierarchy)
        {
            ObjectPoolManager.Instance.ReturnObject(this as Projectile);
            yield break;
        }

        currentChainCount++;
        target = nextTarget.gameObject;
        transform.position = fixedNextPos;
        HitTarget();
    }

    private void PlayLightningEffect(Vector3 startPos, Vector3 endPos)
    {
        if (vfxPrefab != null)
        {
            var vfxEffect = ObjectPoolManager.Instance.GetObject(vfxPrefab);

            vfxEffect.transform.position = Vector3.zero;
            vfxEffect.PlayEffect(startPos, endPos);
        }
    }
}
