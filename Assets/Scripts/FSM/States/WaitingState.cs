using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class WaitingState : State
{
    float timer;
    Node tree;
    public WaitingState(Character c)
    {
        tree = new FloorCharacter(c);
        transitions.Add(new StateTransition(typeof(BaseState), () => timer <= 0));
    }

    public override void Enter()
    {
        base.Enter();
        timer = 10f;
    }
    public override void LogicUpdate()
    {
        //the character should wait and do nothing
        timer -= Time.deltaTime;

        tree.Run();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
