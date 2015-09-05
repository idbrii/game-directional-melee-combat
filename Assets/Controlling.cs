using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controlling : MonoBehaviour {

    [HideInInspector]
    private HeroicAttacking target;
    [HideInInspector]
    private Dictionary<string, HeroicAttacking.eAttackDirection> attackMap;
    

    void Start()
    {
        attackMap = new Dictionary<string, HeroicAttacking.eAttackDirection>();
            //"Jump"
        attackMap["RightAttack"] = HeroicAttacking.eAttackDirection.Right;
        attackMap["LeftAttack" ] = HeroicAttacking.eAttackDirection.Left;
        attackMap["BackAttack" ] = HeroicAttacking.eAttackDirection.Back;

        target = GetComponent<HeroicAttacking>();
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: how to handle multiple inputs on one frame? Right now we get
        // whichever comes last in map.
        foreach (string b in attackMap.Keys)
        {
            bool has_fired = Input.GetButton(b);
            if (has_fired)
            {
                Debug.Log(string.Format("Input {0}: {1}", b, has_fired), this);

                HeroicAttacking.eAttackDirection dir = attackMap[b];
                target.nextAttackDirection = dir;
            }
        }
    }
}
