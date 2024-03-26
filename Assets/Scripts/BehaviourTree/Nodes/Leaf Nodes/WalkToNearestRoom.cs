using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class WalkToNearestRoom : Node
    {

        Character character;
        public WalkToNearestRoom(Character _owner) { character = _owner; }
        public override Result Run()
        {
            if (character == null)
                return Result.failed;

            Vector3 dest = new Vector3(character.currentRoom.visual.transform.position.x, character.currentRoom.visual.transform.position.y - 0.48f, character.currentRoom.visual.transform.position.z + 0.4f);

            if (WalkToLocation.WalkCharacter(character, dest))
            {
                return Result.success;
            }

            return Result.running;
        }
    }
}