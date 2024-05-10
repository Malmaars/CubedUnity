using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public enum StatType
{
    strength
}

[Serializable]
public class Stat 
{
    [SerializeField]
    string name;

    //The type of stat
    [SerializeField]
    public StatType type;

    [SerializeField]
    float statAmount;

    public Stat(string _name, StatType _type) { name = _name; type = _type; }

    public void AddToStat(float _toAdd)
    {
        statAmount += _toAdd;
    }

    public float GetValue()
    {
        return statAmount;
    }
}
