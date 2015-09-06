using UnityEngine;
using System.Collections;

public class Attacking : MonoBehaviour {

    public enum eAttackDirection
    {
        Right
            , Left            
            , Back
            , NoAttack
    }

    [Tooltip("The left damager object we'll use for left attacks.")]
    public GameObject leftDamager;
    [Tooltip("The right damager object we'll use for right attacks.")]
    public GameObject rightDamager;
    [Tooltip("The back damager object we'll use for back attacks.")]
    public GameObject backDamager;
    

    [HideInInspector]
    public eAttackDirection nextAttackDirection = eAttackDirection.NoAttack;
    private eAttackDirection currentAttackDirection = eAttackDirection.NoAttack;

    private float secondsRemainingInAttack = 0;
    [Tooltip("The number of seconds an attack lasts.")]
    public float attackDurationSeconds = 1;

    public bool IsAttackInProgress()
    {
        return nextAttackDirection == eAttackDirection.NoAttack;
    }

    public float AttackProgressPercent
    {
        get
        {
            return secondsRemainingInAttack / attackDurationSeconds;
        }
    }

    public void CauseDamage()
    {
        var damagee = target.GetComponent<Attacking>();
        damagee.ReceiveDamage(gameObject);
    }

    protected abstract void ReceiveDamage(GameObject damager)
    {
    }

    void Start()
    {
    }

    void Update()
    {
        if (currentAttackDirection == eAttackDirection.NoAttack)
        {
            ProcessNextAttack();
        }
        else
        {
            UpdateAttack();
        }
    }

    void ProcessNextAttack()
    {
        if (IsAttackInProgress())
        {
            return;
        }

        currentAttackDirection = nextAttackDirection;
        nextAttackDirection = eAttackDirection.NoAttack;
        secondsRemainingInAttack = attackDurationSeconds;
    }

    void UpdateAttack()
    {
        secondsRemainingInAttack -= Time.deltaTime;
        if (secondsRemainingInAttack <= 0)
        {
            // Clear out both to ensure we're ignoring input entered while
            // attacking. We only pick up the next attack after the attack.
            currentAttackDirection = eAttackDirection.NoAttack;
            nextAttackDirection = eAttackDirection.NoAttack;

            // attack is now invalid, abort.
            return;
        }

        GameObject target = GetTargetDamager(currentAttackDirection);
        if (target == null)
        {
            return;
        }

        // TODO: For now, we just spin to show something is happening. Should
        // change this to translation so it looks more like a punch.
        float spin_progress = AttackProgressPercent;

        var v = target.transform.localEulerAngles;
        v.y = 360 * spin_progress;
        target.transform.localEulerAngles = v;
    }

    GameObject GetTargetDamager(eAttackDirection direction)
    {
        switch (currentAttackDirection)
        {
            case eAttackDirection.Right:
                return rightDamager;
        
            case eAttackDirection.Left:
                return leftDamager;
        
            case eAttackDirection.Back:
                return backDamager;

            case eAttackDirection.NoAttack:
                Dbg.Assert(false, "Should not UpdateAttack invalid attack.");
                break;
        }

        Dbg.Assert(false, "Invalid eAttackDirection: " + direction.ToString());
        return null;
    }
}

