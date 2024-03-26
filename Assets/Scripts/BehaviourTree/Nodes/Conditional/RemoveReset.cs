using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class RemoveReset : Node
{
    Character character;
    public RemoveReset(Character c) { character = c; }

    public override Result Run()
    {
        if (character == null)
            return Result.failed;

        character.resetTree = false;
        return Result.success;
    }
}
