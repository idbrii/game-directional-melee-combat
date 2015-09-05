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

    [HideInInspector]
    public eAttackDirection nextAttackDirection = eAttackDirection.NoAttack;


    void Start()
    {
    }

    void Update()
    {
        if (nextAttackDirection != eAttackDirection.NoAttack)
        {
            Debug.Log("Attacking in " + nextAttackDirection.ToString(), this);

            nextAttackDirection = eAttackDirection.NoAttack;
        }
    }
}

