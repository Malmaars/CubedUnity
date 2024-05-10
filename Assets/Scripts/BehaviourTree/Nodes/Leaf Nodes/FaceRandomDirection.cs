using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTree
{
    public class FaceRandomDirection : Node
    {
        Character character;

        public FaceRandomDirection(Character c) { character = c; }
        public override Result Run()
        {
            if (character == null)
                return Result.failed;

            character.actor.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360f), 0);
            return Result.success;
        }
    }
}