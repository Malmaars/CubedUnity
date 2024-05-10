using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class PaintSomeone : InvokeNode
    {
        bool running;
        Paint paintNode;
        public PaintSomeone(Character c)
        {
            owner = c;
            paintNode = new Paint(c);
        }
        public override Result Run()
        {
            if (owner.target == null)
                return Result.failed;
            if (!running)
            {
                if (child == null)
                    child = new BePainted(owner.target);
                else
                    child.AssignNewOwner(owner.target);

                owner.target.sm.InvokeReaction(child, owner.target, NeedType.social, 15);
                running = true;
            }

            Result result = paintNode.Run();
            UpdateTracking(owner);

            if (result == Result.success || result == Result.failed)
            {
                //end the service, we're done painting
                running = false;
                EndReaction();
                return result;
            }

            return Result.running;
        }
    }

    public class BePainted : ReactionNode
    {
        bool running;
        public BePainted(Character c) { character = c; }
        public override Result Run()
        {
            if (!running)
            {
                character.animator.Play("StrikePoses");
            }

            if (done)
            {
                character.animator.SetTrigger("Return");
                running = false;
                ExitNode();
                return Result.success;
            }

            return Result.running;
        }
    }
}