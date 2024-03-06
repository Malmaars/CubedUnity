using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathNode : IPoolable
{
    public bool active { get; set; }
    public void OnEnableObject()
    {

    }
    public void OnDisableObject()
    {
        parent = null;
        myCube = null;
        dist = -1;
        hDist = -1;
        fDist = 1;
    }

    public Cube myCube;
    public pathNode parent;
    public float dist;
    public float hDist, fDist;

    public pathNode() { }

    public pathNode(Cube _cube, pathNode _parent)
    {
        myCube = _cube;
        parent = _parent;
    }

    public pathNode(Cube _cube, Cube destination, pathNode _parent)
    {
        myCube = _cube;
        hDist = Vector3.Distance(_cube.visual.transform.position, destination.visual.transform.position);
        fDist = Vector3.Distance(_cube.visual.transform.position, _parent.myCube.visual.transform.position) + _parent.fDist;
        dist = fDist + hDist;
        parent = _parent;
    }

    public pathNode(Cube _cube, Cube destination, Cube origin)
    {
        myCube = _cube;
        hDist = Vector3.Distance(_cube.visual.transform.position, destination.visual.transform.position);
        fDist = Vector3.Distance(_cube.visual.transform.position, origin.visual.transform.position);
        dist = fDist + hDist;
    }

    public void SetValues(Cube _cube, Cube destination, Cube origin)
    {
        myCube = _cube;
        hDist = Vector3.Distance(_cube.visual.transform.position, destination.visual.transform.position);
        fDist = Vector3.Distance(_cube.visual.transform.position, origin.visual.transform.position);
        dist = fDist + hDist;
    }
    public void SetParent(Cube _cube, Cube destination, pathNode _parent)
    {
        myCube = _cube;
        hDist = Vector3.Distance(_cube.visual.transform.position, destination.visual.transform.position);
        fDist = Vector3.Distance(_cube.visual.transform.position, _parent.myCube.visual.transform.position) + _parent.fDist;
        dist = fDist + hDist;
        parent = _parent;
    }

    public void SetNewCost(pathNode _parent)
    {
        fDist = Vector3.Distance(myCube.visual.transform.position, _parent.myCube.visual.transform.position) + _parent.fDist;
        parent = _parent;
    }
}

public static class FindRoom
{
    public enum PathResult { success, failed, running }

    static Dictionary<Cube, pathNode> open = new Dictionary<Cube, pathNode>();
    static Dictionary<Cube, pathNode> closed = new Dictionary<Cube, pathNode>();

    static Cube destination;
    static Character character;

    static pathNode path;

    static ObjectPool<pathNode> NodePool = new ObjectPool<pathNode>();

    public static PathResult SetGoal()
    {
        destination = character.RequestRoom();

        if (destination == null)
        {
            return PathResult.failed;
        }

        return PathResult.success;
    }


    public static pathNode Run(Character _character)
    {
        character = _character;
        open.Clear();
        closed.Clear();

        PathResult goalResult = SetGoal();
        if (goalResult == PathResult.failed)
            return null;

        if (!CheckIfPossible(character.currentRoom))
            return null;

        
        //pathNode originalNode = new pathNode(character.currentRoom, destination, character.currentRoom);
        
        pathNode originalNode = NodePool.RequestItem();
        originalNode.SetValues(character.currentRoom, destination, character.currentRoom);
        
        open.Add(originalNode.myCube, originalNode);
        path = FindPath();

        if (path != null)
        {
            Debug.Log("Path found");
            Blackboard.route = path;
            return path;
        }

        return null;
    }

    static List<Cube> visited = new List<Cube>();
    static bool CheckIfPossible(Cube origin)
    {
        visited.Clear();
        if (origin == destination)
            return true;

        return CheckNeighbors(origin);
    }

    static bool CheckNeighbors(Cube _cube)
    {
        foreach (Cube cb in _cube.neighbors)
        {
            if (cb == null)
                continue;

            if (visited.Contains(cb))
                continue;

            if (cb == destination)
                return true;

            visited.Add(cb);
            bool temp = CheckNeighbors(cb);

            if (temp)
                return true;
        }
        return false;
    }

    static pathNode FindPath()
    {
        pathNode endNode = null;
        pathNode current = null;
        while (endNode == null && open.Count > 0)
        {
            foreach (KeyValuePair<Cube, pathNode> node in open)
            {
                //find the one with the lowest cost
                if (current == null || current.dist > node.Value.dist)
                {
                    current = node.Value;
                }
            }

            foreach (Cube cb in current.myCube.neighbors)
            {
                if (cb == null)
                    continue;

                if (closed.ContainsKey(cb))
                    continue;

                if (!open.ContainsKey(cb) && !closed.ContainsKey(cb))
                {
                    //make a new node
                    pathNode node = NodePool.RequestItem();
                    node.SetParent(cb, destination, current);
                    open.Add(node.myCube, node);

                    if (cb == destination)
                    {
                        Debug.Log("Found the destination");
                        Debug.Log(destination.visual.name + ", " + cb.visual.name);
                        return node;
                    }
                }

                else if (open.ContainsKey(cb))
                {
                    //if the new distance is shorter, change the value
                    if (open[cb].fDist > current.fDist + Vector3.Distance(current.myCube.visual.transform.position, open[cb].myCube.visual.transform.position))
                    {
                        open[cb].SetNewCost(current);
                    }
                }
            }

            open.Remove(current.myCube);
            closed.Add(current.myCube, current);
            current = null;
        }

        return null;
    }
}
