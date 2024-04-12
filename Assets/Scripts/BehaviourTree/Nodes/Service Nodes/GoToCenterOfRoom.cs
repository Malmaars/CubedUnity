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

        bool running = false;

        public GoToCenterOfRoom(IServicable _servicable, Cube _room) { servicable = _servicable; room = _room; }

        public override Result Run()
        {
            if (!running)
            {
                character = servicable.asker;
                running = true;
            }

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
