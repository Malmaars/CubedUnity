using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;


namespace BehaviourTree
{
    //A node that checks the condition that is the target of a character being able to interact with
    public class TargetCondition : ConditionalNode
    {
        Character owner;

        public TargetCondition(Character _owner, Node _child)
        {
            owner = _owner;
            child = _child;
        }

        public override Result Run()
        {
            if (owner.target == null)
                return Result.failed;

            return child.Run();
        }
    }
}