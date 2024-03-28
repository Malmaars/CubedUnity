using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class ContractorCube : Cube
{
    protected override void Initialize()
    {
        base.Initialize();
        services = new Service[] { new Service(NeedType.comfort, 1, new ServiceNode(this, new BuildSomething(this))) };
    }
}
