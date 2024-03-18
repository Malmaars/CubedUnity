using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Talk : InvokeNode
{
    Character owner;

    ParticleSystem talkingParticles;
    GameObject particlesObject;

    //We will keep this saved, to prevent having to make a new one constantly
    TalkReaction tReaction;

    float timer;
    bool talking;

    public Talk(Character _owner) 
    {
        owner = _owner;
        particlesObject = Object.Instantiate(Resources.Load("TalkParticles") as GameObject);
        talkingParticles = particlesObject.GetComponent<ParticleSystem>();
        timer = 5;
    }

    public override Result Run()
    {
        //look for a character to talk to
        if (talking == false)
        {
            Debug.Log("Starting new conversation");
            if (SearchForCharacter() == Result.failed)
            {
                owner.target.sm.ResetState();
                return Result.failed;
            }

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

        //this is very much a temporary solution. If I don't check this, this function won't run properly, and the target will be locked in the waiting state. AND I DON'T KNOW WHY
        if(owner.target.actor.transform.position != new Vector3(owner.target.currentRoom.visual.transform.position.x, owner.target.currentRoom.visual.transform.position.y - 0.48f, owner.target.currentRoom.visual.transform.position.z + 0.4f))
        {
            //wait a little, let them walk to their place
            return Result.running;
        }

        Debug.Log("Talking...");
        timer -= Time.deltaTime;

        if(timer <= 0)
        {
            owner.animator.SetBool("Talk", false);
            talkingParticles.Stop();
            EndReaction();
            owner.interacting = false;
            owner.target = null;
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
        if(done)
        {
            character.animator.SetBool("Talk", false);
            return Result.success;
        }

        //play an animation
        character.animator.SetBool("Talk", true);

        return Result.running;
    }

    public void AssignNewOwner(Character c) { character = c; }
}
