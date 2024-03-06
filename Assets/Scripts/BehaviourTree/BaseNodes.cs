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

    public class WalkToRoom : Node
    {
        pathNode path;
        Character character;
        public WalkToRoom(Character _character)
        {
            character = _character;
        }

        public override Result Run()
        {
            if(path == null)
            {
                path = FindRoom.Run(character);

                //there is no possible path, operation failed
                if (path == null)
                    return Result.failed;
            }

            //there is a path, walk to the designated room
            return Result.running;
        }
    }
}