using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

//Conditional node that checks for a boolean to be equal to a given value
public class CheckBoolean : ConditionalNode
{
    Node child;
    bool boolToCheck;
    bool boolValueWeWant;

    //Conditional Version
    public CheckBoolean(bool _boolToCheck, bool _boolValueWeWant, Node _child)
    {
        child = _child;
        boolToCheck = _boolToCheck;
        boolValueWeWant = _boolValueWeWant;
    }

    //Interruptor version
    public CheckBoolean(bool _boolToCheck, bool _boolValueWeWant)
    {
        boolToCheck = _boolToCheck;
        boolValueWeWant = _boolValueWeWant;
    }

    public override Result Run()
    {
        Debug.Log("running boolean: " + boolToCheck + ", " + boolValueWeWant);

        if (boolToCheck != boolValueWeWant)
        {
            return Result.failed;
        }


        if (child != null)
        {
            Result result = child.Run();

            return result;
        }

        return Result.success;
    }
}

