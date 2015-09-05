using UnityEngine;
using System.Collections;

public class HeroicAttacking : MonoBehaviour {

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
    

    [HideInInspector]
    public eAttackDirection nextAttackDirection = eAttackDirection.NoAttack;
    private eAttackDirection currentAttackDirection = eAttackDirection.NoAttack;

    private float secondsRemainingInAttack = 0;
    [Tooltip("The number of seconds an attack lasts.")]
    public float attackDurationSeconds = 1;


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
        if (nextAttackDirection == eAttackDirection.NoAttack)
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
        float spin_progress = secondsRemainingInAttack / attackDurationSeconds;

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
                Debug.Log("Back attack is unimplemented", this);
                return null;

            case eAttackDirection.NoAttack:
                Dbg.Assert(false, "Should not UpdateAttack invalid attack.");
                break;
        }

        Dbg.Assert(false, "Invalid eAttackDirection: " + direction.ToString());
        return null;
    }
}

