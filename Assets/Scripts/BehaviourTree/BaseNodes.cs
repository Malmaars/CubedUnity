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
        InventoryItem itemResultFromAction;

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
        Character character;
        public Idle(Character c) { character = c; }

        float timer = Random.Range(5,20);
        public override Result Run()
        {
            UpdateTracking(character);
            //play an idle animation, for now we can keep this empty
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                timer = Random.Range(5, 20);
                return Result.success;
            }

           return Result.running;
        }
    }
}