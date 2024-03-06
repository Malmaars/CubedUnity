using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class CoroutineRunner : MonoBehaviour
{
    private static CoroutineRunner instance;
    public static CoroutineRunner Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject container = new GameObject("CoroutineRunner");
                instance = container.AddComponent<CoroutineRunner>();
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }
}
public static class Blackboard
{
    public static bool debugMode;

    public static Cube[] allCubes;

    public static pathNode route;

    public static Cube currentTarget;
}
