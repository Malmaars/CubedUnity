using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class GoToRandomRoom : Node
{
    pathNode path;
    Character character;
    public GoToRandomRoom(Character _character)
    {
        character = _character;
    }


    //I need some way to check when the arrangement of the cubes changes. Even while the character is walking
    //I could actively check if the neighbor of the cube is the same as the cube it's going to. When that changes, the character knows its path has changed
    public override Result Run()
    {
        if (path == null)
        {
            path = FindRoom.Run(character);
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
