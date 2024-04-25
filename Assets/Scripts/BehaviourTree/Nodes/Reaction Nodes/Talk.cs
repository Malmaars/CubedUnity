using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class Talk : InvokeNode
    {
        ParticleSystem talkingParticles;
        GameObject particlesObject;

        //We will keep this saved, to prevent having to make a new one constantly
        TalkReaction tReaction;

        float timer;
        bool talking;

        public Talk(Character _owner)
        {
            owner = _owner;
            particlesObject = Object.Instantiate(Resources.Load("Particles/TalkParticles") as GameObject);
            talkingParticles = particlesObject.GetComponent<ParticleSystem>();
            timer = 5;
        }

        public override Result Run()
        {
            UpdateTracking(owner);
            //look for a character to talk to
            if (talking == false)
            {

                if (SearchForCharacter() == Result.failed)
                    return Result.failed;
               

                if (tReaction == null)
                    tReaction = new TalkReaction(owner.target);

                else
                    tReaction.AssignNewOwner(owner.target);

                owner.target.sm.InvokeReaction(tReaction);
                child = tReaction;

                timer = 5;
                particlesObject.transform.position = owner.currentRoom.visual.transform.position;
                particlesObject.transform.parent = owner.currentRoom.visual.transform;
                talkingParticles.Play();
                owner.animator.SetBool("Talk", true);
                talking = true;
            }

            //if it succeeded, that means the target is known, and the target is in the same room
            //invoke a reaction from the target, and start talking

            //Debug.Log("Talking...");
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                owner.animator.SetBool("Talk", false);
                talkingParticles.Stop();
                EndReaction();
                talking = false;

                return Result.success;
            }

            return Result.running;
        }


        public Result SearchForCharacter()
        {
            //we check all the characters that are in the same room as the owner of this node
            foreach (Character inhabitant in owner.currentRoom.currentInhabitants)
            {
                if (inhabitant == owner.target)
                {
                    return Result.success;
                }
            }

            return Result.failed;
        }
    }

    public class TalkReaction : ReactionNode
    {
        public Character character;
        public TalkReaction(Character c) { character = c; }
        public override Result Run()
        {
            UpdateTracking(character);
            if (done)
            {
                character.animator.SetBool("Talk", false);
                ExitNode();
                return Result.success;
            }

            //play an animation
            character.animator.SetBool("Talk", true);

            return Result.running;
        }

        public void AssignNewOwner(Character c) { character = c; }
    }
}