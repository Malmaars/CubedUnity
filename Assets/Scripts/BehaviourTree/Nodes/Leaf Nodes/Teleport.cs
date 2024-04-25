using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    //A node to teleport a character to a specific room
    public class Teleport : Node
    {
        Character character;
        Cube target;
        public Teleport(Character c, Cube cb)
        {
            character = c;
            target = cb;
        }

        public override Result Run()
        {
            if (target == null || character == null)
                return Result.failed;
            //play a cool animation, for now it's instant
            UpdateTracking(character);


            Vector3 dest = new Vector3(target.visual.transform.position.x, target.visual.transform.position.y - 0.48f, target.visual.transform.position.z + 0.4f);
            character.actor.transform.position = dest;
            character.SetRoom(target);
            return Result.success;
        }
    }
}
