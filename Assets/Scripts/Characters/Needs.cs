using System;
using UnityEngine;
using NaughtyAttributes;

public enum NeedType
{
    hunger,
    comfort,
    social,
    energy,
}

//taking inspiration from the sims, there can be different needs, and these needs will decay over time, or by doing certain actions
[Serializable]
public class Need
{
    [SerializeField]
    string name;

    //The type of need
    [SerializeField]
    public NeedType type;

    //every need has a meter that can go up and down, and determines its importance

    [SerializeField]
    [Range(0, 100f)]
    float meter = 100;

    [SerializeField]
    [ProgressBar("Importance", 100, color: EColor.Violet)]
    float importance;

    //if the future, a custom variable to determine the worth of a need might be nice

    //constructor for a single type need
    public Need(string _name, NeedType _type)
    {
        name = _name;
        type =  _type;
    }

    /*
    //constructor for a need of multiple types
    public Need(string _name, NeedType[] _types)
    {
        name = _name;
        types = _types;
    }
    */

    public void AddToMeter(float _toAdd)
    {
        meter += _toAdd;
        if (meter < 0)
            meter = 0;

        if (meter > 100)
            meter = 100;
    }
    public float CalculatePriority()
    {
        float temp = 0;

        switch (type)
        {
            case NeedType.hunger:
                temp = Mathf.Pow(2, -(meter - 135) * 0.05f);
                break;
            case NeedType.energy:
                temp = Mathf.Pow(2, -(meter - 86) * 0.075f);
                break;
            case NeedType.comfort:
                temp = Mathf.Pow((meter - 50) * 0.15f, 2) + 2;
                break;
            case NeedType.social:
                temp = Mathf.Pow((meter - 57) * 0.14f, 2) + 2;
                break;
        }

        UpdateImportance(temp);
        return importance;
    }

    public void UpdateImportance(float newValue) { importance = newValue; }
}
