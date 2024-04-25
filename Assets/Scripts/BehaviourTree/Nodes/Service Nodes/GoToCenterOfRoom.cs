using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    public class GoToCenterOfRoom : Node
    {
        IServicable servicable;
        Cube room;

        Character character;
        WalkToRoom walker;

        bool running = false;

        public GoToCenterOfRoom(IServicable _servicable, Cube _room) { servicable = _servicable; room = _room; }

        public override Result Run()
        {
            if (!running)
            {
                if(servicable.asker == null)
                {
                    Debug.LogWarning("Servicable.asker not found");
                    return Result.failed;
                }
                character = servicable.asker;
                running = true;
            }

            UpdateTracking(character);

            if (character.currentRoom != room)
            {
                pathNode path = FindRoom.Run(character, room);
                path = FindRoom.ReverseList(path);
                walker = Blackboard.walkPool.RequestItem();
                walker.AssignData(path, character);
                running = true;

                Result result = walker.WalkToCube();
                if (result == Result.failed)
                {
                    running = false;
                    return result;
                }

                if (character.currentRoom == room)
                {
                    //the character has entered the room, go to the rest of the node
                    Blackboard.walkPool.ReturnObjectToPool(walker);
                    walker = null;
                }

                return Result.running;
            }
            else
            {

                Vector3 dest = new Vector3(room.visual.transform.position.x, room.visual.transform.position.y - 0.48f, room.visual.transform.position.z + 0.4f);

                bool walkBool = WalkToLocation.WalkCharacter(character, dest);

                if (walkBool)
                {
                    character.SetRoom(room);
                    running = false;
                    return Result.success;
                }

                return Result.running;
            }
        }
    }
}
