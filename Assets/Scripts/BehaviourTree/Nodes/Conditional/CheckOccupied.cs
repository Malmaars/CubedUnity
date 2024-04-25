using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviourTree
{
    //this conditional node will check if the current room is made occupied. Primarily for use as an interruptor node and as a prompt to leave the room
    public class CheckOccupied : ConditionalNode
    {
        Character character;
        public CheckOccupied(Character c) { character = c; }

        public override Result Run()
        {
            if (character.currentRoom.occupied)
                return Result.success;

            if (child == null)
                return Result.running;

            return child.Run();
        }
    }
}