using UnityEngine;
using System.Collections;

public class Vitality : MonoBehaviour
{
    [Tooltip("The owner's starting health amount. Used to determine if we've taken damage.")]
    public float initialHealth = 100f;
    public float Health { get; private set; }

    [HideInInspector]
    public bool isDead = false;

    [Tooltip("An object we can scale to communicate the current health.")]
    public Transform healthIndicator;
    Vector3 baseScale; // the original scale of the object

    void Start()
    {
        Health = initialHealth;

        baseScale = healthIndicator.localScale;
    }

    void Update()
    {
        // Draw lofi health bar

        float health_pct = Health / initialHealth;
        healthIndicator.localScale = baseScale * health_pct;
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

