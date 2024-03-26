using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
namespace BehaviourTree
{
    public class SolveNeed : Node
    {
        NeedType type;
        Need toSolve;

        public SolveNeed(NeedType _type, Need _toSolve)
        {
            type = _type;
            toSolve = _toSolve;
        }

        public override Result Run()
        {
            //look for services nearby, and pick the most suitable one

            //if there are no services nearby, return failed
            return Result.running;
        }
    }
}