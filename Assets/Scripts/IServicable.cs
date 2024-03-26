using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

//interface for objects that can provide a service
public interface IServicable
{

    public Service[] services { get; set; }
}

public class Service
{
    public Service(NeedType _type, int _amount, Node _tree)
    {
        serviceType = _type;
        serviceAmount = _amount;
        service = _tree;
    }

    //the type of service
    public NeedType serviceType;

    //The amount of service this provides
    public int serviceAmount;

    //the behaviour that it services
    public Node service;
}
