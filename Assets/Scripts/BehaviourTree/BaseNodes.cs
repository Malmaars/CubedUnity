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

    public class GoToRoom : Node
    {
        pathNode path;
        Character character;
        bool doneWalking;
        public GoToRoom(Character _character)
        {
            character = _character;
        }

        public override Result Run()
        {
            if (path == null && !doneWalking)
            {
                path = FindRoom.Run(character);
                path = FindRoom.ReverseList(path);

                //there is no possible path, operation failed
                if (path == null)
                    return Result.failed;
            }

            Vector3 dest = new Vector3(path.myCube.visual.transform.position.x, path.myCube.visual.transform.position.y - 0.48f, path.myCube.visual.transform.position.z + 0.4f);

            //the destination should always be above, to the right, to the left, or below the character. Never multiple
            if(dest.x < character.actor.transform.position.x)
            {
                //face and walk left
                character.actor.transform.forward = Vector3.left;
                character.animator.SetBool("Walking", true);
                character.actor.transform.position += (dest - character.actor.transform.position).normalized * Time.deltaTime * 0.5f;
            }

            else if (dest.x > character.actor.transform.position.x)
            {
                //face and walk right
                character.actor.transform.forward = Vector3.right;
                character.animator.SetBool("Walking", true);
                character.actor.transform.position += (dest - character.actor.transform.position).normalized * Time.deltaTime * 0.5f;
            }

            if (dest.y > character.actor.transform.position.y)
            {
                //jump up
                Debug.Log("Jumping up");
                character.animator.SetBool("Jump", true);
                character.actor.transform.position += (dest - character.actor.transform.position).normalized * Time.deltaTime * 2f;
            }

            else if(dest.y < character.actor.transform.position.y)
            {
                //jump down
                Debug.Log("Jumping down");
                character.animator.SetBool("Jump", true);
                character.actor.transform.position += (dest - character.actor.transform.position).normalized * Time.deltaTime * 2f;
            }

            if (Vector3.Distance(character.actor.transform.position, dest) < 0.01f)
            {
                character.actor.transform.position = dest;
                character.animator.SetBool("Walking", false);
                character.animator.SetBool("Jump", false);
                character.currentRoom = path.myCube;
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