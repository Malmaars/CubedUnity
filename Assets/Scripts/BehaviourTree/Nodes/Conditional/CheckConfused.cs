using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
namespace BehaviourTree
{
    public class CheckConfused : ConditionalNode
    {

        Character character;
        public CheckConfused(Character c) { character = c; }

        public override Result Run()
        {
            if (character.confused)
                return Result.success;

            if (child == null)
                return Result.running;

            return child.Run();
        }
    }
}