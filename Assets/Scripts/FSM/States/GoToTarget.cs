using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GoToTarget : Node
{
    pathNode path;
    Character character;
    public GoToTarget(Character _owner) { character = _owner; }
    public override Result Run()
    {
        if (character == null || character.target == null)
            return Result.failed;

        //walk to target
        if (path == null)
        {
            //it is possible that the characters are already in the same room, which would return null
            if (character.currentRoom == character.target.currentRoom)
                return Result.success;

            path = FindRoom.Run(character, character.target.currentRoom);
            path = FindRoom.ReverseList(path);

            //there is no possible path, operation failed
            if (path == null)
                return Result.failed;
        }

        Vector3 dest = new Vector3(path.myCube.visual.transform.position.x, path.myCube.visual.transform.position.y - 0.48f, path.myCube.visual.transform.position.z + 0.4f);

        bool walkBool = WalkToLocation.WalkCharacter(character, dest);

        //set the new cube as the current room when the character breaches the threshold the wall. Not when he just reached the destination
        if (path != null && character.currentRoom != path.myCube &&
            Vector3.Distance(character.actor.transform.position, path.myCube.visual.transform.position) < Vector3.Distance(character.actor.transform.position, character.currentRoom.visual.transform.position))
        {
            character.SetRoom(path.myCube);
        }

        if (walkBool)
        {
            //pause a second until the next step?
            character.SetRoom(path.myCube);
            pathNode temp = path.parent;
            FindRoom.ReturnPathNode(path);
            path = temp;
        }

        if (path == null)
        {
            character.actor.transform.forward = Vector3.forward;
            return Result.success;
        }

        return Result.running;
    }
}
