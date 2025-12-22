using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private float maxHP;
    [SerializeField] private float HP = 100f;
    private int baseReward;

    [SerializeField] private float damageFlashDuration = 0.3f;
    [SerializeField] private Color damageColor = Color.red;

    private Renderer enemyRenderer;
    private Material originalMaterial;
    private Color originalColor;
    private Coroutine flashCoroutine;

    void Awake()
    {
        enemyRenderer = GetComponent<Renderer>();
        if (enemyRenderer != null)
        {
            originalMaterial = enemyRenderer.material;
            originalColor = originalMaterial.color;
        }
    }

    public void Setup(int reward, float maxHp)
    {
        baseReward = reward;
        this.maxHP = maxHp;
        HP = maxHp;
        ResetColor();
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;

        StartDamageFlash();

        if (HP <= 0) Die();
    }

    public void Die()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
            ResetColor();
        }

        GameManager.Instance.AddGoldOnEnemyDie(baseReward); // 보상 지급
        ObjectPoolManager.Instance.ReturnObject(this);
    }

    public void ResetHP()
    {
        HP = maxHP;
        ResetColor();

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }
    }

    private void StartDamageFlash()
    {
        if (enemyRenderer == null) return;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(DamageFlashCoroutine());
    }

    private IEnumerator DamageFlashCoroutine()
    {
        enemyRenderer.material.color = damageColor;

        yield return new WaitForSeconds(damageFlashDuration);

        float fadeTime = 0.2f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            var lerpedColor = Color.Lerp(damageColor, originalColor, elapsedTime / fadeTime);
            enemyRenderer.material.color = lerpedColor;
            yield return null;
        }

        enemyRenderer.material.color = originalColor;
        flashCoroutine = null;
    }

    private void ResetColor()
    {
        if (enemyRenderer != null && originalMaterial != null)
        {
            enemyRenderer.material.color = originalColor;
        }
    }
}
