using BehaviourTree;
using System;

[Serializable]
public class Service
{
    public IServicable parent;
    public Service(IServicable _parent, NeedType _type, int _amount, Node _tree)
    {
        parent = _parent;
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