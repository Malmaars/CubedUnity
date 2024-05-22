using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Relationship
{
    [SerializeField]
    Character character;

    [SerializeField]
    [Range(-100f, 100f)]
    float friendship = 0;

    public Relationship(Character c)
    {
        character = c;
    }

    public void AddFriendshipAmount(float _addAmount)
    {
        friendship += _addAmount;
        if(friendship < -100) 
            friendship = -100;
        if(friendship > 100) 
            friendship = 100;
    }

    public void AddFriendshipOnEmotion(emotion _emo)
    {
        switch (_emo)
        {
            case emotion.Happy:
                AddFriendshipAmount(5f);
                break;
            case emotion.Angry:
                AddFriendshipAmount(-5f);
                break;
            case emotion.Shy:
                AddFriendshipAmount(5f);
                break;
            case emotion.Dumb:
                AddFriendshipAmount(-5f);
                break;
            case emotion.Delight:
                AddFriendshipAmount(5f);
                break;
            case emotion.Upset:
                AddFriendshipAmount(-5f);
                break;

        }
    }
}
