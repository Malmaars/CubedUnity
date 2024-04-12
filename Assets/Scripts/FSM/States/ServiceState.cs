using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class ServiceState : State
{
    Node tree;
    bool done;

    public ServiceState()
    {
        done = false;
        transitions.Add(new StateTransition(typeof(BaseState), () => done == true));
    }

    //This serves as a way to dictate what the reaction should be.
    public void SetTree(Node _tree) { tree = _tree; }
    public override void LogicUpdate()
    {
        if (!done) 
        {
            Node.Result result = tree.Run();

        //When the character is done with the action, it will return themselves to the base state
            if (result == Node.Result.failed || result == Node.Result.success)
            {
                done = true;
            }
        }
    }

    public override void Exit()
    {
        done = false;
        base.Exit();
    }
}
