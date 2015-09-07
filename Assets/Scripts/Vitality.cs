using UnityEngine;
using System.Collections;

public class Vitality : MonoBehaviour
{
    [Tooltip("The owner's starting health amount. Used to determine if we've taken damage.")]
    public float initialHealth = 100f;
    public float Health { get; private set; }

    [HideInInspector]
    public bool isDead = false;

    void Start()
    {
        Health = initialHealth;
    }

    void Update()
    {
        // Draw lofi health bar
        Vector3 start = transform.position + Vector3.up * 2.1f;
        Vector3 full_end = start + Vector3.right;
        Vector3 end = Vector3.MoveTowards(start, full_end, Health / initialHealth);
        Vector3 offset = Vector3.up * -0.05f;
		Debug.DrawLine(start + offset, full_end + offset, Color.white);
		Debug.DrawLine(start, end, Color.red);
    }


    public void TakeDamage(GameObject enemy, float damageAmount)
    {
        if (isDead)
        {
            // do nothing
        }
        else if (Health < damageAmount)
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
        Health -= damageAmount;
    }

    void Die(GameObject killer)
    {
        isDead = true;
        Health = 0f;
    }
}

