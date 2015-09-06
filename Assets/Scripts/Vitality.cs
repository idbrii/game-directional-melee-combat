using UnityEngine;
using System.Collections;

public class Vitality : MonoBehaviour
{
    [Tooltip("The owner's starting health amount. Used to determine if we've taken damage.")]
    public float initialHealth = 100f;
    [HideInInspector]
    public float health;
    [HideInInspector]
    public bool isDead = false;

    void Update()
    {
        Vector3 start = transform.position;
        Vector3 full_end = start + Vector3.right;
        Vector3 end = Vector3.MoveTowards(start, full_end, health / initialHealth);
		Debug.DrawLine(start, end);
    }


    public void TakeDamage(GameObject enemy, float damageAmount)
    {
        if (isDead)
        {
            // do nothing
        }
        else if (health < damageAmount)
        {
            Die(enemy);
        }
        else
        {
            HandleDamage(enemy, damageAmount);
        }
    }

    void HandleDamage(GameObject enemy, float damageAmount)
    {
        health -= damageAmount;
    }

    void Die(GameObject killer)
    {
        isDead = true;
        health = 0f;
    }
}

