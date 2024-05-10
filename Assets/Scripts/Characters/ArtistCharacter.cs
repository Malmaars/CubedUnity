using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class ArtistCharacter : Character
{
    protected override void Initialize()
    {
        base.Initialize();

        List<Service> updatedServices = personalServices.ToList();

        updatedServices.Add(new Service(this, NeedType.comfort, 15, new Sequence(new CharacterToCenterOfRoom(this), new FaceRandomDirection(this), new Paint(this))));
        updatedServices.Add(new Service(this, NeedType.social, 15, new Sequence(new Sequence(
                                                                                    new LookForTarget(this),
                                                                                    new LockTarget(this),
                                                                                    new GoToTarget(this),
                                                                                    new FaceOwnerAndTarget(this), 
                                                                                    new PaintSomeone(this)))));

        personalServices = updatedServices.ToArray();
    }
}
