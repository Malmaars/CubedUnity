using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NeedType
{
    hunger,
    comfort,
    social
}
//taking inspiration from the sims, there can be different needs, and these needs will decay over time, or by doing certain actions
public class Need
{
    string name;

    //The type of need
    NeedType[] types;

    //every need has a meter that can go up and down, and determines its importance
    int meter;

    public void AddToMeter(int _toAdd)
    {
        meter += _toAdd;
    }
}
