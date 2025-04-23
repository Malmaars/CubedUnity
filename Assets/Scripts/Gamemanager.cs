using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using BehaviourTree;

public class Gamemanager : MonoBehaviour
{
    public bool debugMode;
    //public Cube[] allCubes;

    bool panning;
    Vector2 mousePosition;
    Vector3 originalCameraPosition;

    private void Awake()
    {
        Blackboard.debugMode = debugMode;
        Blackboard.allCharacters = FindObjectsOfType<Character>();
        //Blackboard.allCubes = allCubes;
    }

    private void Start()
    {
        //Debug.Log(Blackboard.allCubes.Count);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            Camera.main.transform.position = new Vector3(0, 0, 3.5f);

        //scroll to zoom out, right mouse button to pan
        
        if (Input.GetMouseButtonDown(1))
        {
            //move the camera
            panning = true;
            mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            originalCameraPosition = Camera.main.transform.position;
        }

        if (Input.GetMouseButton(1))
        {
            Vector2 newPos = (new Vector2(Input.mousePosition.x, Input.mousePosition.y) - mousePosition) / 500;
            Camera.main.transform.position = originalCameraPosition + new Vector3(newPos.x, -newPos.y, 0);
        }
        
        if (Input.GetMouseButtonUp(1))
            panning = false;

        Camera.main.transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel"));

        if (Camera.main.transform.position.z < 2)
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 2);

        if (Camera.main.transform.position.z > 6)
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 6);


    }

    private void SpawnCube()
    {

    }
}
