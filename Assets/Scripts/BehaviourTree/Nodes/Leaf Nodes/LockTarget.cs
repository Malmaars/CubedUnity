using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class LockTarget : Node
    {
        Character owner;
        public LockTarget(Character _owner) { owner = _owner; }
        public override Result Run()
        {
            UpdateTracking(owner);

            if (owner == null || owner.target == null)
                return Result.failed;

            owner.interacting = true;
            owner.target.sm.SetToWait();

            //this might not be the best way to cancel the current animation
            foreach (AnimatorControllerParameter parameter in owner.target.animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Bool)
                    owner.target.animator.SetBool(parameter.name, false);
            }

            return Result.success;
        }
    }
}