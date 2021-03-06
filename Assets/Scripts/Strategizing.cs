﻿using UnityEngine;
using System.Collections;

public class Strategizing : MonoBehaviour {

    private Attacking attackController;
    
    [Header("Attack planning")]
    [Tooltip("Minimum time between attack starts.")]
    [Range(0.01f,100f)]
    public float minSecondsBetweenAttacks = 3f;
    private float secondsToNextAttack = 0f;

    void Start()
    {
        attackController = GetComponentInChildren<Attacking>();
    }

    void Update()
    {
        float delta_time = Time.deltaTime;
		secondsToNextAttack -= delta_time;

		if (attackController.IsAttackInProgress())
        {
            secondsToNextAttack = minSecondsBetweenAttacks;
        }

		if (secondsToNextAttack <= 0)
        {
			InitiateAttack();
        }
    }

	void InitiateAttack()
	{
        attackController.QueueAttack(SelectAttack());
	}

    protected virtual Attacking.eAttackDirection SelectAttack()
    {
        // very random
        return Attacking.eAttackDirection.Left;
    }
}
