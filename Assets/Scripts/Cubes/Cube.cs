using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

public class Cube : MonoBehaviour, IServicable
{
    public GameObject visual;
    Character owner;

    public List<Character> currentInhabitants;

    //tracks the neighbors of this cube. 0 is the neighbor to the right of the cube, 1 is above, 2 is to left, 3 is down
    public Cube[] neighbors = new Cube[4];

    Vector3Int oldLoc;
    bool dragging = false;

    //a bool to ensure other characters don't enter the room
    public bool occupied;

    //Every cube also has to have a behaviourtree of things that characters can do in that room
    protected Node tree;

    //Iservicable data
    public Character asker { get; set; }
    public Service[] services { get; set; }

    public bool beingUsed { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        visual = this.gameObject;

        Blackboard.allCubes.Add(this);

        //for now I want to let the cubes assign their neighbors on startup, so I don't have to manually input it every time

        //if there is no collider at the overlapsphere, the query should be stopped
        Collider[] colTemp = Physics.OverlapSphere(visual.transform.position - Vector3.left, 0.1f);
        if (colTemp.Length > 0)
            ConnectNeighbor(colTemp[0].GetComponent<Cube>(), 0);

        colTemp = Physics.OverlapSphere(visual.transform.position + Vector3.up, 0.1f);
        if (colTemp.Length > 0)
            ConnectNeighbor(colTemp[0].GetComponent<Cube>(), 1);

        colTemp = Physics.OverlapSphere(visual.transform.position + Vector3.left, 0.1f);
        if (colTemp.Length > 0)
            ConnectNeighbor(colTemp[0].GetComponent<Cube>(), 2);

        colTemp = Physics.OverlapSphere(visual.transform.position - Vector3.up, 0.1f);
        if (colTemp.Length > 0)
            ConnectNeighbor(colTemp[0].GetComponent<Cube>(), 3);

        for (int i = 0; i < neighbors.Length; i++)
        {
            //also remove it as neighbor from its neighbors
            if (neighbors[i] != null)
            {
                switch (i)
                {
                    case 0:
                        neighbors[i].ConnectNeighbor(this, 2);
                        break;
                    case 1:
                        neighbors[i].ConnectNeighbor(this, 3);
                        break;
                    case 2:
                        neighbors[i].ConnectNeighbor(this, 0);
                        break;
                    case 3:
                        neighbors[i].ConnectNeighbor(this, 1);
                        break;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            visual.transform.position = new Vector3(worldPosition.x, worldPosition.y, visual.transform.position.z);

            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;

                Vector3Int newPos = new Vector3Int(Mathf.RoundToInt(worldPosition.x), Mathf.RoundToInt(worldPosition.y), Mathf.RoundToInt(visual.transform.position.z));
                //snap the cube to the closest location if possible

                //check location, perhaps with a collider check (quick and dirty)
                Collider[] colliders = Physics.OverlapSphere(newPos, 0.1f);

                if (colliders.Length == 0)
                {
                    //there's nothing there. Put the cube there, and connect it to the neighbouring cubes
                    visual.transform.position = newPos;
                }

                else
                {
                    //put it back
                    visual.transform.position = oldLoc;
                }

                //assign neighbors


                //if there is no collider at the overlapsphere, the query should be stopped
                Collider[] colTemp = Physics.OverlapSphere(visual.transform.position - Vector3.left, 0.1f);
                if (colTemp.Length > 0)
                    ConnectNeighbor(colTemp[0].GetComponent<Cube>(), 0);

                colTemp = Physics.OverlapSphere(visual.transform.position + Vector3.up, 0.1f);
                if (colTemp.Length > 0)
                    ConnectNeighbor(colTemp[0].GetComponent<Cube>(), 1);

                colTemp = Physics.OverlapSphere(visual.transform.position + Vector3.left, 0.1f);
                if (colTemp.Length > 0)
                    ConnectNeighbor(colTemp[0].GetComponent<Cube>(), 2);
                
                colTemp = Physics.OverlapSphere(visual.transform.position - Vector3.up, 0.1f);
                if (colTemp.Length > 0)
                    ConnectNeighbor(colTemp[0].GetComponent<Cube>(), 3);

                for (int i = 0; i < neighbors.Length; i++)
                {
                    //also remove it as neighbor from its neighbors
                    if (neighbors[i] != null)
                    {
                        switch (i)
                        {
                            case 0:
                                neighbors[i].ConnectNeighbor(this, 2);
                                break;
                            case 1:
                                neighbors[i].ConnectNeighbor(this, 3);
                                break;
                            case 2:
                                neighbors[i].ConnectNeighbor(this, 0);
                                break;
                            case 3:
                                neighbors[i].ConnectNeighbor(this, 1);
                                break;
                        }
                    }
                }

                GetComponent<Collider>().enabled = true;
            }
        }
    }

    //call this to assign a neighbor to the cube
    public void ConnectNeighbor(Cube _cube, int direction)
    {
        if (direction < 0 || direction > neighbors.Length)
        {
            Debug.LogError("This direction does not exist. Not assigning neighbor.");
            return;
        }

        neighbors[direction] = _cube;
    }

    public void RemoveNeighbor(int direction)
    {
        if (direction < 0 || direction > neighbors.Length)
        {
            Debug.LogError("This direction does not exist. Not assigning neighbor.");
            return;
        }

        neighbors[direction] = null;
    }

    public Service GetService(Character _asker, int _index)
    {
        asker = _asker;
        occupied = true;
        return services[_index];
    }

    public void SetAsker(Character _asker)
    {
        asker = _asker;
        beingUsed = true;
    }

    public void EndService()
    {
        occupied = false;
    }


    //function for dragging cubes
    private void OnMouseDown()
    {
        dragging = true;
        for(int i = 0; i < neighbors.Length; i++)
        {
            //also remove it as neighbor from its neighbors
            if (neighbors[i] != null)
            {
                switch (i)
                {
                    case 0:
                        neighbors[i].RemoveNeighbor(2);
                        break;
                    case 1:
                        neighbors[i].RemoveNeighbor(3);
                        break;
                    case 2:
                        neighbors[i].RemoveNeighbor(0);
                        break;
                    case 3:
                        neighbors[i].RemoveNeighbor(1);
                        break;
                }

                RemoveNeighbor(i);
            }

            neighbors[i] = null;
        }
        oldLoc = new Vector3Int(
            Mathf.RoundToInt(visual.transform.position.x),
            Mathf.RoundToInt(visual.transform.position.y),
            Mathf.RoundToInt(visual.transform.position.z));

        GetComponent<Collider>().enabled = false;
    }
}
