using System;
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

        emotion mainEmotion, reactionEmotion;

        public Talk(Character _owner)
        {
            owner = _owner;
            particlesObject = UnityEngine.Object.Instantiate(Resources.Load("Particles/TalkParticles") as GameObject);
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

                owner.target.sm.InvokeReaction(tReaction, owner.target, NeedType.social, 15);
                child = tReaction;

                timer = 5;
                particlesObject.transform.position = owner.currentRoom.visual.transform.position;
                particlesObject.transform.parent = owner.currentRoom.visual.transform;
                talkingParticles.Play();
                owner.animator.SetBool("Talk", true);
                talking = true;

                //decide what the emotions of each character is, and how it influences their relationship with each other
                mainEmotion = (emotion)UnityEngine.Random.Range(0, Enum.GetValues(typeof(emotion)).Length);
                reactionEmotion = (emotion)UnityEngine.Random.Range(0, Enum.GetValues(typeof(emotion)).Length);

                ShowEmotion.StartEmotion(owner, mainEmotion, 5);
                ShowEmotion.StartEmotion(owner.target, reactionEmotion, 5);
            }

            //if it succeeded, that means the target is known, and the target is in the same room
            //invoke a reaction from the target, and start talking

            //Debug.Log("Talking...");
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                owner.animator.SetBool("Talk", false);
                talkingParticles.Stop();

                owner.characterRelations[owner.target].AddFriendshipOnEmotion(mainEmotion);
                owner.target.characterRelations[owner].AddFriendshipOnEmotion(reactionEmotion);

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
    }
}