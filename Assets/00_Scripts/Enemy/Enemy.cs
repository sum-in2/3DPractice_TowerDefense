using UnityEngine;

public class Enemy : MonoBehaviour
{

    private float maxHP = 100f;
    private float HP = 100f;

    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + "의 HP : " + HP + " > " + (HP - damage));
        HP -= damage;

        if (HP < 0) Die();
    }
    public void Die()
    {
        ObjectPoolManager.Instance.ReturnObject(this);
    }
    public void ResetHP()
    {
        HP = maxHP;
    }
}
