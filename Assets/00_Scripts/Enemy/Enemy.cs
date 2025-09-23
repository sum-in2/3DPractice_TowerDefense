using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private float maxHP = 100f;
    private float HP = 100f;

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
            return;
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
