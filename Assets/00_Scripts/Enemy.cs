using UnityEngine;

public class Enemy : MonoBehaviour
{

    private float HP = 100;

    public void TakeDamage(float damage)
    {
        Debug.Log(gameObject.name + "의 HP : " + HP + " > " + (HP - damage));
        HP -= damage;

        if (HP < 0) Die();
    }
    public void Die()
    {
        // 풀 반환
    }
}
