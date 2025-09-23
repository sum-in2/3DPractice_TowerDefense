using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VFX;
using System.Collections;

public class ChainProj : Projectile
{
    public int maxChainCount = 3;
    public float chainRange = 5f;
    public float chainDamageReduction = 0.8f;
    public VisualEffect lightningVFX;
    public float chainDelay = 0.1f;

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
        this.target = target;
        Vector3 temp = new Vector3(transform.position.x, target.transform.position.y, transform.position.z);
        PlayLightningEffect(temp, target.transform.position);

        DebugEx.Log(temp + " / " + target.transform.position);
        transform.position = target.transform.position;
        HitTarget();
    }

    protected override void Update()
    {
        if (target != null && !target.activeSelf)
            ObjectPoolManager.Instance.ReturnObject(this);
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
        Vector3 currentPos = target.transform.position;
        Vector3 endPos = nextTarget.transform.position;

        PlayLightningEffect(currentPos, endPos);

        yield return new WaitForSeconds(chainDelay);

        currentChainCount++;
        target = nextTarget.gameObject;

        transform.position = nextTarget.transform.position;
        HitTarget();
    }

    private void PlayLightningEffect(Vector3 startPos, Vector3 endPos)
    {
        if (lightningVFX != null)
        {
            lightningVFX.SetVector3("StartPosition", startPos);
            lightningVFX.SetVector3("EndPosition", endPos);
            lightningVFX.SetFloat("LifeTime", 1.5f);

            lightningVFX.Play();
        }
    }
}
