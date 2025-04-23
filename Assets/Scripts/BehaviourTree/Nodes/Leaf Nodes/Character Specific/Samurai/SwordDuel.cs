using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class SwordDuel : InvokeNode
    {
        bool running, finishing;
        float timer;
        SwordSlash mySlash;
        public SwordDuel(Character c) { owner = c; mySlash = new SwordSlash(c); }
        public override Result Run()
        {
            if (owner.target == null)
                return Result.failed;

            if (!running && !finishing)
            {
                if (child == null)
                    child = new SwordReaction(owner.target);
                else
                    child.AssignNewOwner(owner.target);

                owner.target.sm.InvokeReaction(child, owner.target, NeedType.social, 15);

                running = true;
            }
            else if(running && !finishing)
            {
                Result result = mySlash.Run();
                if (result == Result.success || result == Result.failed)
                {
                    //decide who wins the duel, for now the samurai always wins
                    owner.animator.Play("Celebrate");
                    owner.target.animator.Play("Defeated");
                    owner.characterRelations[owner.target].AddFriendshipAmount(5f);
                    owner.target.characterRelations[owner].AddFriendshipAmount(-5f);

                    timer = 5;
                    finishing = true;
                }
            }

            else if(running && finishing)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    owner.interacting = false;
                    //the animation has finished, return the character to the default state
                    owner.animator.SetTrigger("Return");
                    child.done = true;

                    return Result.success;
                }
            }


            UpdateTracking(owner);

            return Result.running;
        }
    }

    public class SwordReaction : ReactionNode
    {
        SwordSlash mySlash;
        bool running;
        public SwordReaction(Character c)
        {
            character = c;
            mySlash = new SwordSlash(c);
            running = true;
        }

        public override Result Run()
        {
            UpdateTracking(character);
            if (running)
            {
                Result result = mySlash.Run();
                if(result == Result.success || result == Result.failed)
                {
                    running = false;
                }
            }

            if (done)
            {
                character.animator.SetTrigger("Return");
                ExitNode();
                return Result.success;
            }
            return Result.running;
        }

        public override void AssignNewOwner(Character c)
        {
            base.AssignNewOwner(c);
            mySlash.ReAssign(c);
            running = true;
        }
    }
}