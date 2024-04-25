using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class CheckRoute : ConditionalNode
    {
        Character character;
        Cube targetRoom;
        public CheckRoute(Character c, Cube cb)
        {
            character = c;
            targetRoom = cb;
        }

        public override Result Run()
        {
            if (FindRoom.CheckIfPossible(character.currentRoom, targetRoom))
                return Result.success;
            else
                return Result.failed;
        }
    }
}