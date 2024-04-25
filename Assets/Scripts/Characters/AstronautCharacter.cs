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

        updatedServices.Add(new Service(this, NeedType.energy, 1, new Sequence(new WalkToNearestRoom(this), new FloatAround(this))));

        personalServices = updatedServices.ToArray();
    }
}
