using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class ChooseService : Node
{
    Character character;

    public ChooseService(Character c) { character = c; }
    public override Result Run()
    {
        //for now we'll just pick the first service that can be provided by the current room the character is in
        character.sm.InvokeService(character.currentRoom.GetService(character, 0).service);
        
        //the character shouldn't be able to reach this line, but if it does, just return failed
        return Result.failed;
    }
}
