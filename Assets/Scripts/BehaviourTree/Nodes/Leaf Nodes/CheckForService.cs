using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CheckForService : Node
{
    Character character;
    public CheckForService(Character c) { character = c; }

    public override Result Run()
    {
        //for now we'll simply check if the cube the character is in can provide a service

        if(character.currentRoom.services == null || character.currentRoom.services.Length == 0 || character.currentRoom.beingUsed)
        {
            //there are no services. Search failed
            return Result.failed;
        }

        //there are services
        return Result.success;
    }
}
