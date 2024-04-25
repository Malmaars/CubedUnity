using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class WalkToRoom : IPoolable
{
    pathNode path;
    Character character;

    float positionOffset;
    public bool active { get ; set ; }

    public Node.Result WalkToCube()
    {
        //the destination is not in the same room (so it's the next) and the direction of the path doesn't match the neighbor
        if (character.currentRoom != path.myCube && character.currentRoom.neighbors[path.direction] != path.myCube
            || character.currentRoom != path.myCube && character.currentRoom.neighbors[path.direction].occupied)
        {
            //the route is not possible anymore.
            character.actor.transform.forward = Vector3.forward;

            //return all pathnodes to the pool and set the path back to null
            while (path != null)
            {
                pathNode temp = path.parent;
                FindRoom.ReturnPathNode(path);
                path = temp;
                positionOffset = Random.Range(-0.3f, 0.3f);
            }

            character.confused = true;
            return Node.Result.failed;
        }

        Vector3 dest;

        if (path.myCube.visual.transform.position.y != character.currentRoom.visual.transform.position.y)
            dest = new Vector3(path.myCube.visual.transform.position.x, path.myCube.visual.transform.position.y - 0.48f, path.myCube.visual.transform.position.z + 0.4f);
        else
            dest = new Vector3(path.myCube.visual.transform.position.x + positionOffset, path.myCube.visual.transform.position.y - 0.48f, path.myCube.visual.transform.position.z + 0.4f);

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
            positionOffset = Random.Range(-0.3f, 0.3f);
        }

        if (path == null)
        {
            character.actor.transform.forward = Vector3.forward;
            return Node.Result.success;
        }

        return Node.Result.running;
    }

    public void AssignData(pathNode newPath, Character c)
    {
        path = newPath;
        character = c;
        positionOffset = Random.Range(-0.3f, 0.3f);
    }

    void ClearData()
    {
        path = null;
        character = null;
    }

    public void OnDisableObject(){ ClearData(); }

    public void OnEnableObject(){}
}
