using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BehaviourTree;

public class Gamemanager : MonoBehaviour
{
    public bool debugMode;
    public Cube[] allCubes;

    private void Awake()
    {
        Blackboard.debugMode = debugMode;
        Blackboard.allCubes = allCubes;
    }

    private void OnDrawGizmos()
    {
        if(Blackboard.route != null)
        {
            pathNode nodeTemp = Blackboard.route;
            int index = 0;
            //Debug.Log(Blackboard.route.myCube.visual.name);
            //Debug.Log(Blackboard.route.dist);
            while (nodeTemp.parent != null)
            {
                if (index == 0)
                    Gizmos.color = Color.green;

                else
                    Gizmos.color = Color.green;

                GUIStyle style = new GUIStyle();
                style.fontSize = Mathf.RoundToInt(20);

                Gizmos.DrawSphere(nodeTemp.myCube.visual.transform.position, 0.01f);
                Handles.Label(nodeTemp.myCube.visual.transform.position, index.ToString(), style);
                index++;

                //Debug.Log(nodeTemp.parent);
                nodeTemp = nodeTemp.parent;
                if (nodeTemp.parent == null)
                    break;
            }
        }
    }

    private void SpawnCube()
    {

    }
}
