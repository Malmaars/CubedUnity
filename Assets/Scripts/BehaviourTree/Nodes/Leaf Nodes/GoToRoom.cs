using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class GoToRoom : Node
    {
        Character character;
        Cube target;
        bool running;
        WalkToRoom walker;
        public GoToRoom(Character c, Cube cb)
        {
            character = c;
            target = cb;
        }

        public override Result Run()
        {
            UpdateTracking(character);

            if (!running)
            {
                pathNode path = FindRoom.Run(character, target);
                path = FindRoom.ReverseList(path);
                walker = Blackboard.walkPool.RequestItem();
                walker.AssignData(path, character);
                running = true;
            }

            Result result = walker.WalkToCube();

            if (result == Result.failed || result == Result.success)
            {
                Blackboard.walkPool.ReturnObjectToPool(walker);
                walker = null;
                running = false;
            }
            return result;
        }
    }
}
