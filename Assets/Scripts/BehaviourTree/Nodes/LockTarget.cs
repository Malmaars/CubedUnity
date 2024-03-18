using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class LockTarget : Node
{
    Character owner;
    public LockTarget(Character _owner) { owner = _owner; }
    public override Result Run()
    {
        if (owner == null || owner.target == null)
            return Result.failed;

        owner.interacting = true;
        owner.target.sm.SetToWait();

        foreach (AnimatorControllerParameter parameter in owner.target.animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                owner.target.animator.SetBool(parameter.name, false);
        }

        return Result.success;
    }
}
