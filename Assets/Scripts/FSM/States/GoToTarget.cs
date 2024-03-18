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
            path = FindRoom.Run(character, character.target.currentRoom);
            path = FindRoom.ReverseList(path);

            //there is no possible path, operation failed
            if (path == null)
                return Result.failed;
        }

        Vector3 dest = new Vector3(path.myCube.visual.transform.position.x, path.myCube.visual.transform.position.y - 0.48f, path.myCube.visual.transform.position.z + 0.4f);


        //the destination should always be above, to the right, to the left, or below the character. Never multiple
        if (dest.x < character.actor.transform.position.x)
        {
            //face and walk left
            character.actor.transform.forward = Vector3.left;
            character.animator.SetBool("Walking", true);
            character.actor.transform.position += Vector3.ClampMagnitude((dest - character.actor.transform.position).normalized * Time.deltaTime * 0.5f, Vector3.Distance(dest, character.actor.transform.position));

            //if(path.myCube != character.currentRoom && path.myCube != character.currentRoom.neighbors[2])
            //{
            //    Debug.Log("Route has changed.");
            //    //the route has changed. Back up
            //    //set path to null?
            //    path = null;
            //    return Result.failed;
            //}
        }

        else if (dest.x > character.actor.transform.position.x)
        {
            //face and walk right
            character.actor.transform.forward = Vector3.right;
            character.animator.SetBool("Walking", true);
            character.actor.transform.position += Vector3.ClampMagnitude((dest - character.actor.transform.position).normalized * Time.deltaTime * 0.5f, Vector3.Distance(dest, character.actor.transform.position));
        }

        if (dest.y > character.actor.transform.position.y)
        {
            //jump up
            character.animator.SetBool("Jump", true);
            character.actor.transform.position += Vector3.ClampMagnitude((dest - character.actor.transform.position).normalized * Time.deltaTime * 2f, Vector3.Distance(dest, character.actor.transform.position));
        }

        else if (dest.y < character.actor.transform.position.y)
        {
            //jump down
            character.animator.SetBool("Jump", true);
            character.actor.transform.position += Vector3.ClampMagnitude((dest - character.actor.transform.position).normalized * Time.deltaTime * 2f, Vector3.Distance(dest, character.actor.transform.position));
        }

        //set the new cube as the current room when the character breaches the threshold the wall. Not when he just reached the destination
        if (path != null && character.currentRoom != path.myCube &&
            Vector3.Distance(character.actor.transform.position, path.myCube.visual.transform.position) < Vector3.Distance(character.actor.transform.position, character.currentRoom.visual.transform.position))
        {
            character.SetRoom(path.myCube);
        }

        if (Vector3.Distance(character.actor.transform.position, dest) < 0.01f)
        {
            character.animator.SetBool("Walking", false);
            character.animator.SetBool("Jump", false);

            //pause a second until the next step?
            character.actor.transform.position = dest;
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
