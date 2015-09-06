using UnityEngine;
using System.Collections;

public class Strategizing : MonoBehaviour {

    enum eAttackWindow
    {
        Starting
            , Swinging
            , Landing
            , Done
    }

    private Attacking attackController;
    
    [Tooltip("Minimum time between attack starts.")]
    public float minSecondsBetweenAttacks;
    private float secondsToNextAttack = 0;

    [Tooltip("How far into attack until we are swinging.")]
    [Range(0,1)]
    public float percentUntilSwinging;
    [Tooltip("How far into attack until we are landing.")]
    [Range(0,1)]
    public float percentUntilLanding;

    eAttackWindow currentAttackWindow = eAttackWindow.Done;

    void Start()
    {
        attackController = GetComponent<Attacking>();
    }

    void Update()
    {
        float delta_time = Time.deltaTime;

		secondsToNextAttack -= delta_time;
		if (attackController.IsAttackInProgress() || secondsToNextAttack > 0)
		{
            UpdateAttack(delta_time);
		}
        else
        {
			InitiateAttack();
        }
    }

	void InitiateAttack()
	{
        attack.nextAttackDirection = SelectAttack();
	}

    Attacking.eAttackDirection SelectAttack()
    {
        // very random
        return Attacking.eAttackDirection.Left;
    }

    void UpdateAttack(float delta_time)
    {
        currentAttackWindow = CalcAttackWindow();
        if (currentAttackWindow == eAttackWindow.Landing)
        {
            attackController.CauseDamage();
        }
    }

    eAttackWindow CalcAttackWindow()
    {
        float progress = attackController.AttackProgressPercent;
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

}
