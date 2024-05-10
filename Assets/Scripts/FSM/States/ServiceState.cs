using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class ServiceState : State
{
    Node tree;
    Service service;
    Character servicedCharacter;
    bool done;

    public ServiceState()
    {
        done = false;
        transitions.Add(new StateTransition(typeof(BaseState), () => done == true));
    }

    //This serves as a way to dictate what the reaction should be.
    public void SetService(Service s, Character c) 
    {
        service = s;
        tree = s.service;
        servicedCharacter = c;
    }
    public override void LogicUpdate()
    {
        if (!done) 
        {
            Node.Result result = tree.Run();

        //When the character is done with the action, it will return themselves to the base state
            if (result == Node.Result.failed || result == Node.Result.success)
            {
                if (result == Node.Result.success)
                    servicedCharacter.ResolveService(service);

                done = true;
            }
        }
    }

    public override void Exit()
    {
        done = false;
        tree = null;
        service = null;
        servicedCharacter = null;
        base.Exit();
    }
}
