using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class FaceOwnerAndTarget : Node
{
    Character owner;

    Vector3 dest, targetDest;

    public FaceOwnerAndTarget(Character c) { owner = c; }
    public override Result Run()
    {
        if (owner == null || owner.target == null)
            return Result.failed;

        //make the owner stand right or left at random, and the target on the remaining side, make them face each other
        if (dest == null || targetDest == null || dest == Vector3.zero || targetDest == Vector3.zero)
        {
            if(Random.Range(0,2) == 0)
            {
                //set owner to left
                dest = new Vector3(owner.currentRoom.visual.transform.position.x, owner.currentRoom.visual.transform.position.y - 0.48f, owner.currentRoom.visual.transform.position.z + 0.4f) + Vector3.left * 0.3f;
                targetDest = new Vector3(owner.currentRoom.visual.transform.position.x, owner.currentRoom.visual.transform.position.y - 0.48f, owner.currentRoom.visual.transform.position.z + 0.4f) - Vector3.left * 0.3f;
            }
            else
            {
                //set owner to right
                dest = new Vector3(owner.currentRoom.visual.transform.position.x, owner.currentRoom.visual.transform.position.y - 0.48f, owner.currentRoom.visual.transform.position.z + 0.4f) - Vector3.left * 0.3f;
                targetDest = new Vector3(owner.currentRoom.visual.transform.position.x, owner.currentRoom.visual.transform.position.y - 0.48f, owner.currentRoom.visual.transform.position.z + 0.4f) + Vector3.left * 0.3f;
            }
        }


        bool ownerWalkBool = WalkToLocation.WalkCharacter(owner, dest);
        bool targetWalkBool = WalkToLocation.WalkCharacter(owner.target, targetDest);

        if (ownerWalkBool && targetWalkBool)
        {
            owner.actor.transform.forward = owner.target.actor.transform.position - owner.actor.transform.position;
            owner.target.actor.transform.forward = owner.actor.transform.position - owner.target.actor.transform.position;

            dest = Vector3.zero;
            targetDest = Vector3.zero;
            return Result.success;
        }

        return Result.running;
    }
}
