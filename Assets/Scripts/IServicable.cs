using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

//interface for objects that can provide a service
public interface IServicable
{
    public Character asker { get; set; }
    public Service[] services { get; set; }

    public Service GetService(Character _asker, int _index);

    public void SetAsker(Character _asker);

    public void EndService();

    public bool beingUsed { get; set; }
}

