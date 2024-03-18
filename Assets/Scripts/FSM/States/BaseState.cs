using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

//This state will serve as the base state for any character. It will contain their behaviourtree, and run it every update
public class BaseState : State
{
    Node tree;

    public BaseState(Node _tree)
    {
        tree = _tree;

        //transitions.Add(new StateTransition(typeof(ReactionState), () => reacting == true));
    }
    public override void LogicUpdate()
    {
        //run the behaviourtree
        tree.Run();
    }
}
