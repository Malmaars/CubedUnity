using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

namespace BehaviourTree
{
    //this conditional node will check if the behaviourtree of the character should be reset. Primarily for use as an interruptor node
    public class CheckReset : ConditionalNode
    {
        Character character;
        public CheckReset(Character c) { character = c; }

        public override Result Run()
        {
            if (character.resetTree)
                return Result.success;

            if (child == null)
                return Result.running;

            return child.Run();
        }
    }

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
}