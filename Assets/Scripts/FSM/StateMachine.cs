﻿using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class StateMachine
{
    public IState currenstate;

    //The dictionary kind of functions as an objectpool. We fetch the states we want, and put them back when they're not in use
    private Dictionary<System.Type, IState> stateCollection;

    //A character will only have one reaction active at a time, so this serves as a singular objectpool
    private ReactionState reactState;

    public StateMachine(Node _tree, Character _owner)
    {
        stateCollection = new Dictionary<System.Type, IState>();
        currenstate = new BaseState(_tree);
        stateCollection.Add(typeof(BaseState), currenstate);
        stateCollection.Add(typeof(WaitingState), new WaitingState(_owner));
    }

    public void SwitchState(IState _newState)
    {
        currenstate.OnSwitch();
        currenstate.Exit();
        currenstate = _newState;
        currenstate.Enter();
    }

    public void StateUpdate()
    {
        currenstate.LogicUpdate();


        //Check each transition of a state, and if it wants to switch, switch.
        foreach (StateTransition transition in currenstate.transitions)
        {
            if (transition.condition())
            {
                SwitchState(GetState(transition.target));
                break;
            }
        }
    }

    public IState GetState(System.Type _t)
    {
        return GetOrCreateState(_t);
    }

    private IState GetOrCreateState(System.Type _t)
    {
        IState state;

        //if statecollection has that type, fetch it
        if (stateCollection.TryGetValue(_t, out state))
        {
            return state;
        }

        //if it doesn't, make a new one.
        else
        { 
            object obj = System.Activator.CreateInstance(_t);
            IState _instance = (IState)obj;

            stateCollection.Add(_t, _instance);

            return _instance;
        }
    }

    public void SetToWait()
    {
        SwitchState(GetState(typeof(WaitingState)));
    }

    public void ResetState()
    {
        SwitchState(GetState(typeof(BaseState)));
    }


    //If a character wants to invoke a reaction, it can call this function on the character they want to invoke the reaction on
    public void InvokeReaction(Node reactionNode)
    {
        //set the current state to a Reaction State

        if(currenstate.GetType() == typeof(ReactionState))
        {
            //the character is already in a reaction state, so it can't react
            return;
        }

        //Break the current state and switch to the preferred reaction
        //The reaction will consist of a node, and we can run it like we would a behaviourtree. This means a reaction can also be a sequence of actions.

        if(reactState == null)
        {
            //make a new reaction state
            reactState = new ReactionState();
            reactState.SetTree(reactionNode);
        }

        else
        {
            reactState.SetTree(reactionNode);
        }

        SwitchState(reactState);
    }
}