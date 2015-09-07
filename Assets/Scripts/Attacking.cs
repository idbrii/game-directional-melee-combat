using UnityEngine;
using System.Collections;

public class Attacking : MonoBehaviour
{

    public enum eAttackDirection
    {
        Right,
        Left,
        Back,
        NoAttack
    }

    enum eAttackWindow
    {
        Starting,
        Swinging,
        Landing,
        Done
    }

    [Header("Attack Windows")]
    [Tooltip("How far into attack until we are swinging.")]
    [Range(0, 1)]
    public float percentUntilSwinging = 0.25f;
    [Tooltip("How far into attack until we are landing.")]
    [Range(0, 1)]
    public float percentUntilLanding = 0.75f;
    private eAttackWindow currentAttackWindow = eAttackWindow.Done;

    private float secondsRemainingInAttack = 0;
    [Tooltip("The number of seconds an attack lasts.")]
    public float attackDurationSeconds = 1f;

    [Header("Attack objects")]
    [Tooltip("The left damager object we'll use for left attacks.")]
    public GameObject leftDamager;
    [Tooltip("The right damager object we'll use for right attacks.")]
    public GameObject rightDamager;
    [Tooltip("The back damager object we'll use for back attacks.")]
    public GameObject backDamager;
    
    private eAttackDirection nextAttackDirection = eAttackDirection.NoAttack;
    private eAttackDirection currentAttackDirection = eAttackDirection.NoAttack;

    [Tooltip("How much rotation to use for the attack animation.")]
    public float totalAttackAnimRotation = 90f;

    [Header("Blocking and stun")]
    [Tooltip("How long this object's stun lasts.")]
    public float secondsForStun = 3;
    private float secondsRemainingInStun = 0;
    [Tooltip("Whether this object is capable of blocking incoming attacks by starting an attack in the same direction.")]
    public bool canBlock = true;

    public bool IsAttackInProgress()
    {
        return nextAttackDirection == eAttackDirection.NoAttack;
    }

    float CalcAttackProgressPercent()
    {
        return secondsRemainingInAttack / attackDurationSeconds;
    }

    public bool IsStunned
    {
        get
        {
            return secondsRemainingInStun > 0;
        }
    }

    public void QueueAttack(eAttackDirection direction)
    {
        nextAttackDirection = direction;
    }

    void Update()
    {
        if (IsStunned)
        {
            secondsRemainingInStun -= Time.deltaTime;

            // Some kind of UI to show we're stunned.
            Debug.DrawLine(transform.position, transform.position + Vector3.up, Color.blue);
        }
        else if (currentAttackDirection == eAttackDirection.NoAttack)
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

    void UpdateAttackWindow(float delta_time)
    {
        currentAttackWindow = CalcAttackWindow();
        if (currentAttackWindow == eAttackWindow.Landing)
        {
            LandAttack();
        }
    }

    void LandAttack()
    {
        GameObject attack_target = DetermineAttackTarget(currentAttackDirection);
        if (attack_target == null)
        {
            return;
        }

        Attacking damagee = attack_target.GetComponentInChildren<Attacking>();
        eAttackResult result = damagee.CalcAttackResult(this);
        damagee.HandleAttack(result);
    }

    void HandleAttack(eAttackResult attack_result)
    {
        bool has_hit = false;

        if (canBlock)
        {
            has_hit = attack_result == eAttackResult.Hit;

            if (attack_result == eAttackResult.Parry)
            {
                Stun(this);
            }

            if (attack_result == eAttackResult.Block)
            {
                AbortAttack();
            }
        }
        else
        {
            has_hit = attack_result != eAttackResult.Block;
        }

        if (has_hit)
        {
            ApplyDamage(this);
        }
    }

    GameObject DetermineAttackTarget(eAttackDirection direction)
    {
        GameObject damager = GetTargetDamager(currentAttackDirection);
        if (damager == null)
        {
            return null;
        }

        var collisions = damager.GetComponent<ColliderCollector>();
        var found = collisions.GetFirstOverlappingObject();
        if (found != null)
        {
            found = found.transform.root.gameObject;
        }

        return found;
    }

    eAttackWindow CalcAttackWindow()
    {
        Dbg.Assert(percentUntilSwinging < percentUntilLanding, "Must swing before we can land");

        float progress = CalcAttackProgressPercent();
        if (progress < 0)
        {
            return eAttackWindow.Done;
        }
        else if (progress > percentUntilLanding)
        {
            return eAttackWindow.Landing;
        }
        else if (progress > percentUntilSwinging)
        {
            return eAttackWindow.Swinging;
        }
        else
        {
            return eAttackWindow.Starting;
        }
    }

    void ApplyDamage(Attacking damager)
    {
        Vitality health = GetComponentInChildren<Vitality>();
        health.TakeDamage(damager.gameObject, 10);
    }

    void Stun(Attacking damager)
    {
        secondsRemainingInStun = Mathf.Max(secondsRemainingInStun, secondsForStun);

        AbortAttack();
    }

    void AbortAttack()
    {
        currentAttackDirection = eAttackDirection.NoAttack;
    }

    enum eAttackResult
    {
        Hit,
        Block,
        Parry
    }

    eAttackResult CalcAttackResult(Attacking damager)
    {
        if (currentAttackDirection == eAttackDirection.NoAttack)
        {
            // Not attacking at all, so no chance for defence.
            return eAttackResult.Hit;
        }
        else if (currentAttackDirection == damager.currentAttackDirection)
        {
            // Matching attack directions, we have a chance for defence or
            // being vulnerable.
            switch (currentAttackWindow)
            {
                case eAttackWindow.Starting:
                    return eAttackResult.Parry;

                case eAttackWindow.Swinging:
                case eAttackWindow.Landing:
                    return eAttackResult.Block;

                case eAttackWindow.Done:
                    return eAttackResult.Hit;
            }

            Dbg.Assert(false, "Unhandled enum case: " + currentAttackWindow);
            return eAttackResult.Hit;
        }
        else
        {
            return eAttackResult.Hit;
        }
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
        float spin_progress = CalcAttackProgressPercent();

        float direction = 1f;
        if (currentAttackDirection == eAttackDirection.Right)
        {
            direction = -1f;
        }

        var v = target.transform.localEulerAngles;
        v.y = direction * totalAttackAnimRotation * spin_progress;
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
