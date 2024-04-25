using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree {
    public class FloorCharacter : Node
    {
        //this node exists to put the character on the ground of their current cube, made specifically for the waiting state so character don't randomly float in the air
        Character character;
        public FloorCharacter(Character c) { character = c; }

        public override Result Run()
        {
            UpdateTracking(character);
            if (character.actor.transform.position.y != character.currentRoom.visual.transform.position.y - 0.48f)
            {
                WalkToLocation.WalkCharacter(character, new Vector3(character.actor.transform.position.x, character.currentRoom.visual.transform.position.y - 0.48f, character.actor.transform.position.z));
                return Result.running;
            }

            else
                return Result.success;
        }
    }
}
