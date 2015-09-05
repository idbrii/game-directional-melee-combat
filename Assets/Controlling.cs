using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controlling : MonoBehaviour {

    [HideInInspector]
    private Attacking target;
    [HideInInspector]
    private Dictionary<string, Attacking.eAttackDirection> attackMap;
    

    void Start()
    {
        attackMap = new Dictionary<string, Attacking.eAttackDirection>();
            //"Jump"
        attackMap["RightAttack"] = Attacking.eAttackDirection.Right;
        attackMap["LeftAttack" ] = Attacking.eAttackDirection.Left;
        attackMap["BackAttack" ] = Attacking.eAttackDirection.Back;

        target = GetComponent<Attacking>();
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

                Attacking.eAttackDirection dir = attackMap[b];
                target.nextAttackDirection = dir;
            }
        }
    }
}
