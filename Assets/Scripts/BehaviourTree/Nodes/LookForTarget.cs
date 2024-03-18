using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

//Looks for the closest target that isn't already reacting to something, and tell them to wait
public class LookForTarget : Node
{
    Character owner;

    public LookForTarget(Character _owner) { owner = _owner; }
    public override Result Run()
    {
        return CheckIfPossible(owner.currentRoom);
    }

    List<Cube> visited = new List<Cube>();
    Result CheckIfPossible(Cube origin)
    {
        visited.Clear();
        if (origin.currentInhabitants.Count > 1)
        {
            foreach(Character inhabitant in origin.currentInhabitants)
            {
                if (inhabitant == owner)
                    continue;


                if(!inhabitant.interacting && inhabitant.sm.currenstate.GetType() == typeof(BaseState))
                {
                    owner.target = inhabitant;

                    Debug.Log("Found target");
                    return Result.success;
                }
            }
        }
        visited.Add(origin);

        return CheckNeighbors(origin);
    }

    Result CheckNeighbors(Cube _cube)
    {
        foreach (Cube cb in _cube.neighbors)
        {
            if (cb == null)
                continue;

            if (visited.Contains(cb))
                continue;

            if (cb.currentInhabitants.Count > 0)
            {
                foreach (Character inhabitant in cb.currentInhabitants)
                {
                    if (inhabitant == owner)
                        continue;

                    //biggest problem here is that a character invoking a reaction from another is still in its basestate.
                    if (!inhabitant.interacting && inhabitant.sm.currenstate.GetType() == typeof(BaseState))
                    {
                        owner.target = inhabitant;
                        return Result.success;
                    }
                }
            }

            visited.Add(cb);
            Result temp = CheckNeighbors(cb);

            if (temp == Result.success)
                return Result.success;
        }
        return Result.failed;
    }
}
