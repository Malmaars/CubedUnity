using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class ResetTarget : Node
    {
        Character owner;
        public ResetTarget(Character c) { owner = c; }
        public override Result Run()
        {
            owner.RemoveTarget();
            return Result.success;
        }
    }
}