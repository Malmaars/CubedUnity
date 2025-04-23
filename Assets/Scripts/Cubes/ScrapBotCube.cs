using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class ScrapBotCube : Cube
{
    protected override void Initialize()
    {
        base.Initialize();
        services = new Service[] { new Service(this, NeedType.comfort, 1, new ServiceNode(this, new Sequence(new GoToCenterOfRoom(this, this), new BuildSomething(this)))) };
    }
}
