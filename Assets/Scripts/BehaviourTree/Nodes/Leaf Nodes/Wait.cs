using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
namespace BehaviourTree
{
    public class Wait : Node
    {
        public override Result Run()
        {
            //do nothing
            return Result.running;
        }
    }
}