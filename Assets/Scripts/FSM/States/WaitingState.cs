using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class WaitingState : State
{
    Node tree;
    public WaitingState(Character c)
    {
        tree = new WalkToNearestRoom(c);
    }
    public override void LogicUpdate()
    {
        //the character should wait and do nothing
        //tree.Run();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
