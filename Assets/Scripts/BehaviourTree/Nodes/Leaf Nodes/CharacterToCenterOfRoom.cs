using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class CharacterToCenterOfRoom : Node
    {
        Character character;
        IServicable servicable;
        public CharacterToCenterOfRoom(Character c) { character = c; }
        public CharacterToCenterOfRoom(IServicable i) { servicable = i; }

        public override Result Run()
        {
            if (character != null)
            {
                UpdateTracking(character);

                Vector3 dest = new Vector3(character.currentRoom.visual.transform.position.x, character.currentRoom.visual.transform.position.y - 0.48f, character.currentRoom.visual.transform.position.z + 0.4f);

                bool walkBool = WalkToLocation.WalkCharacter(character, dest);

                if (walkBool)
                    return Result.success;

                return Result.running;
            }

            else if(servicable != null)
            {
                if (servicable.asker == null)
                    return Result.failed;

                UpdateTracking(servicable.asker);

                Vector3 dest = new Vector3(servicable.asker.currentRoom.visual.transform.position.x, servicable.asker.currentRoom.visual.transform.position.y - 0.48f, servicable.asker.currentRoom.visual.transform.position.z + 0.4f);

                bool walkBool = WalkToLocation.WalkCharacter(servicable.asker, dest);

                if (walkBool)
                    return Result.success;

                return Result.running;
            }

            else
            {
                return Result.failed;
            }

        }
    }
}