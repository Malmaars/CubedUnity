using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//interface for objects that can provide a service
public interface IServicable
{
    //the type of service
    public NeedType serviceType { get; set; }

    //The amount of service this provides
    public int serviceAmount { get; set; }
}
