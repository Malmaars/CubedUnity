using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class GoToTarget : Node
    {
        bool running;
        Character character;
        WalkToRoom walker;
        public GoToTarget(Character _owner) { character = _owner; }
        public override Result Run()
        {
            if (character == null || character.target == null)
                return Result.failed;

            UpdateTracking(character);

            //walk to target
            if (running == false)
            {
                //it is possible that the characters are already in the same room, which would return null
                if (character.currentRoom == character.target.currentRoom)
                    return Result.success;

                pathNode path = FindRoom.Run(character, character.target.currentRoom);
                path = FindRoom.ReverseList(path);

                //there is no possible path, operation failed
                if (path == null)
                    return Result.failed;

                walker = Blackboard.walkPool.RequestItem();
                walker.AssignData(path, character);
                running = true;
            }

            Result result = walker.WalkToCube();

            if (result == Result.failed || result == Result.success)
            {
                if(result == Result.failed)
                {
                    character.RemoveTarget();
                    character.confused = true;
                }
                Blackboard.walkPool.ReturnObjectToPool(walker);
                walker = null;
                running = false;
            }
            return result;
        }
    }
}