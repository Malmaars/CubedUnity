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
    public static Plane depthPlane = new Plane(Vector3.forward, new Vector3(0,0,1));

    public static bool debugMode;

    public static List<Cube> allCubes = new List<Cube>();

    public static pathNode route;

    public static Cube currentTarget;

    public static ObjectPool<WalkToRoom> walkPool = new ObjectPool<WalkToRoom>();
}
