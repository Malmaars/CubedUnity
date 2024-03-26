using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BehaviourTree;

public class Gamemanager : MonoBehaviour
{
    public bool debugMode;
    //public Cube[] allCubes;

    private void Awake()
    {
        Blackboard.debugMode = debugMode;
        //Blackboard.allCubes = allCubes;
    }

    private void Start()
    {
        //Debug.Log(Blackboard.allCubes.Count);
    }

    private void SpawnCube()
    {

    }
}
