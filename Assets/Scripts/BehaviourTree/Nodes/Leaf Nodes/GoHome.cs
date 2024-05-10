using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree {
    public class GoHome : Node
    {
        Character character;
        IServicable servicable;
        WalkToRoom walker;

        bool running;
        public GoHome(Character c) { character = c; }
        public GoHome(IServicable i) { servicable = i; }

        public override Result Run()
        {
            if(character != null)
            {
                UpdateTracking(character);

                if (!running)
                {
                    if (character.currentRoom == character.home)
                        return Result.success;
                    pathNode path = FindRoom.Run(character, character.home);
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

            else  if(servicable != null)
            {
                UpdateTracking(servicable.asker);

                if (!running)
                {
                    if (servicable.asker.currentRoom == servicable.asker.home)
                        return Result.success;
                    pathNode path = FindRoom.Run(servicable.asker, servicable.asker.home);
                    path = FindRoom.ReverseList(path);
                    walker = Blackboard.walkPool.RequestItem();
                    walker.AssignData(path, servicable.asker);
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

            else
            {
                return Result.failed;
            }
        }
    }
}