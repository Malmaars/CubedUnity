using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class SamuraiCharacter : Character
{
    protected override void Initialize()
    {
        base.Initialize();

        List<Service> updatedServices = personalServices.ToList();

        updatedServices.Add(new Service(this, NeedType.social, 15, new Sequence(new Sequence(
                                                                                    new LookForTarget(this),
                                                                                    new LockTarget(this),
                                                                                    new GoToTarget(this),
                                                                                    new FaceOwnerAndTarget(this),
                                                                                    new SwordDuel(this)))));

        personalServices = updatedServices.ToArray();
    }
}
