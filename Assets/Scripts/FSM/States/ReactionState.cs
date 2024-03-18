using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class ReactionState : State
{
    Node tree;
    bool done;

    public ReactionState()
    {
        done = false;
        transitions.Add(new StateTransition(typeof(BaseState), () => done == true));
    }

    //This serves as a way to dictate what the reaction should be.
    public void SetTree(Node _tree) { tree = _tree; }
    public override void LogicUpdate()
    {
        Node.Result result = tree.Run();

        //I'm not sure this will do. Perhaps the reaction should be dependend on the character that invoked it?
        if(result == Node.Result.failed || result == Node.Result.success)
        {
            done = true;
        }
    }

    public override void Exit()
    {
        base.Exit();
        done = false;
    }
}
