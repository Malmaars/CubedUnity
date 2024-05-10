using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class ZeroGravTennis : InvokeNode
    {
        bool running;
        public ZeroGravTennis(Character c)
        {
            owner = c;
        }
        public override Result Run()
        {
            if (owner.target == null)
                return Result.failed;
            if (!running)
            {
                if (child == null)
                    child = new ZeroGravTennisReaction(owner.target);
                else
                    child.AssignNewOwner(owner.target);
                
                running = true;    
            }



            return Result.running;
        }
    }

    public class ZeroGravTennisReaction : ReactionNode
    {
        public ZeroGravTennisReaction(Character c) { character = c; }
        public override Result Run()
        {
            throw new System.NotImplementedException();
        }
    }
}