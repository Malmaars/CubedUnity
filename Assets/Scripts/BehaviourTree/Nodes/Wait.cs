using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Wait : Node
{
    public override Result Run()
    {
        //do nothing
        return Result.running;
    }
}
