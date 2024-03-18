using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace BehaviourTree
{
    //I want to be able to return items to a character when they perform certain actions

    //i can accomplish this by adding parameters to the nodes, as they need to be initialized anyway
    //This way, I can pass the character's inventory to the node.

    //example

    public class ItemExample : Node
    {
        Item itemResultFromAction;

        IInventory holder;
        public ItemExample(IInventory inventoryHolder)
        {
            holder = inventoryHolder;
        }
        public override Result Run()
        {
            //if(the action succeeds)
            holder.AddItemToInventory(itemResultFromAction);
            return Result.success;

            //else
            //return Result.failed

        }
    }

    public class Idle : Node
    {
        float timer = Random.Range(2,10);
        public override Result Run()
        {
            //play an idle animation, for now we can keep this empty
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = Random.Range(2, 10);
                return Result.success;
            }

           return Result.running;
        }
    }

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

            else if(dest.y < character.actor.transform.position.y)
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
}