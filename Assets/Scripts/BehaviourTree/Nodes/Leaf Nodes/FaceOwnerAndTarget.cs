using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class FaceOwnerAndTarget : Node
    {
        Character owner;

        Vector3 dest, targetDest;
        int randomSide;

        public FaceOwnerAndTarget(Character c) { owner = c; }
        public override Result Run()
        {
            UpdateTracking(owner);

            if (owner == null || owner.target == null)
                return Result.failed;

            if (dest == null || dest == Vector3.zero)
                randomSide = Random.Range(0, 2);

            if (randomSide == 0)
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


            bool ownerWalkBool = WalkToLocation.WalkCharacter(owner, dest);
            bool targetWalkBool = WalkToLocation.WalkCharacter(owner.target, targetDest);

            if (ownerWalkBool && targetWalkBool)
            {
                owner.actor.transform.forward = owner.target.actor.transform.position - owner.actor.transform.position;
                owner.target.actor.transform.forward = owner.actor.transform.position - owner.target.actor.transform.position;
                return Result.success;
            }

            return Result.running;
        }
    }
}