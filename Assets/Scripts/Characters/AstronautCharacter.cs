using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class AstronautCharacter : Character
{
    protected override void Initialize()
    {
        base.Initialize();
        
        List<Service> updatedServices = personalServices.ToList();

        updatedServices.Add(new Service(this, NeedType.energy, 15, new Sequence(new WalkToNearestRoom(this), new FloatAround(this))));
        //updatedServices.Add(new Service(this, NeedType.social, 1, new Sequence(new LookForTarget(this),
        //                                                                        new LockTarget(this),
        //                                                                        new GoToTarget(this),
        //                                                                        new FaceOwnerAndTarget(this),
        //                                                                        new ZeroGravTennis(this))));

        personalServices = updatedServices.ToArray();
    }
}
