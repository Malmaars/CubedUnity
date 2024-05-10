using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class ReactionState : State
{
    Node tree;
    bool done;

    Character character;
    NeedType needType;
    float needAmount;

    public ReactionState()
    {
        done = false;
        transitions.Add(new StateTransition(typeof(BaseState), () => done == true));
    }

    //This serves as a way to dictate what the reaction should be.
    public void SetVariables(Node _tree, Character c, NeedType _needType, float _needAmount) 
    {
        tree = _tree;
        character = c;
        needType = _needType;
        needAmount = _needAmount;
    }
    public override void LogicUpdate()
    {
        Node.Result result = tree.Run();

        //I'm not sure this will do. Perhaps the reaction should be dependend on the character that invoked it?
        if(result == Node.Result.failed || result == Node.Result.success)
        {
            //also fulfill the need of a reaction
            if (result == Node.Result.success)
            {
                character.ResolveService(needType, needAmount);
            }

            done = true;
        }
    }

    public override void Exit()
    {
        done = false;
        base.Exit();
    }
}
