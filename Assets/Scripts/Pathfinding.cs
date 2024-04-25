using System;
using System.Collections.Generic;
using UnityEngine;

public class pathNode : IPoolable
{
    public bool active { get; set; }
    public void OnEnableObject(){}
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

    //direction will be an int from 0 to 3 
    public int direction = 5;

    public pathNode() { }
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


    //Run option where the destination is chosen at random
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
        
        pathNode originalNode = NodePool.RequestItem();
        originalNode.SetValues(character.currentRoom, destination, character.currentRoom);
        
        open.Add(originalNode.myCube, originalNode);
        path = FindPath();

        if (path != null)
        {
            Blackboard.route = path;
            return path;
        }

        return null;
    }

    //Run variant where we can pass through a custom destination
    public static pathNode Run(Character _character, Cube _cube)
    {
        character = _character;
        open.Clear();
        closed.Clear();

        destination = _cube;

        if (destination == null)
            return null;

        if (!CheckIfPossible(character.currentRoom))
            return null;

        pathNode originalNode = NodePool.RequestItem();
        originalNode.SetValues(character.currentRoom, destination, character.currentRoom);

        open.Add(originalNode.myCube, originalNode);
        path = FindPath();

        if (path != null)
        {
            Blackboard.route = path;
            return path;
        }

        return null;
    }

    static List<Cube> visited = new List<Cube>();
    public static bool CheckIfPossible(Cube origin)
    {
        visited.Clear();

        if (destination.occupied)
            return false;

        if (origin == destination)
            return true;

        return CheckNeighbors(origin);
    }
    
    public static bool CheckIfPossible(Cube origin, Cube _destination)
    {
        visited.Clear();
        if (origin == _destination)
            return true;

        return CheckNeighborsSeparated(origin, _destination);
    }

    static bool CheckNeighborsSeparated(Cube _cube, Cube _destination)
    {
        foreach (Cube cb in _cube.neighbors)
        {
            if (cb == null || visited.Contains(cb))
                continue;

            if (cb.occupied)
            {
                visited.Add(cb);
                continue;
            }

            if (cb == _destination)
                return true;

            visited.Add(cb);
            bool temp = CheckNeighborsSeparated(cb, _destination);

            if (temp)
                return true;
        }
        return false;
    }

    static bool CheckNeighbors(Cube _cube)
    {
        foreach (Cube cb in _cube.neighbors)
        {
            if (cb == null || visited.Contains(cb))
                continue;

            if (cb.occupied)
            {
                visited.Add(cb);
                continue;
            }

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
                if (cb == null || closed.ContainsKey(cb))
                    continue;

                if (cb.occupied)
                {
                    closed.Add(cb, null);
                    continue;
                }

                if (!open.ContainsKey(cb) && !closed.ContainsKey(cb))
                {
                    //make a new node
                    pathNode node = NodePool.RequestItem();
                    node.SetParent(cb, destination, current);
                    node.direction = Array.IndexOf(current.myCube.neighbors, cb);


                    //if the cube is the destination, end the loop
                    if (cb == destination)
                    {
                        open.Remove(current.myCube);
                        closed.Add(current.myCube, current);

                        foreach (KeyValuePair<Cube, pathNode> pair in open)
                        {
                            //I can return all the pathnodes in the open list to the pool, since they won't be used anyway
                            NodePool.ReturnObjectToPool(pair.Value);
                        }
                        open.Clear();

                        //I will also return all the unused nodes in the closed list
                        //First remove all the used nodes from the closed list
                        pathNode nodeTemp = node;
                        while(nodeTemp != null)
                        {
                            closed.Remove(nodeTemp.myCube);
                            nodeTemp = nodeTemp.parent;
                        }

                        //then remove all the other nodes in the closed list to the object pool
                        foreach (KeyValuePair<Cube, pathNode> pair in closed)
                        {
                            if (pair.Value == null)
                                continue;

                            NodePool.ReturnObjectToPool(pair.Value);
                        }
                        closed.Clear();

                        return node;
                    }

                    open.Add(node.myCube, node);
                }

                else if (open.ContainsKey(cb))
                {
                    //if the new distance is shorter, change the value
                    if (open[cb].fDist > current.fDist + Vector3.Distance(current.myCube.visual.transform.position, open[cb].myCube.visual.transform.position))
                    {
                        //value change
                        open[cb].SetNewCost(current);

                        //We pass the direction that is received. So if a character walks to the "next cube" to the right, the "next cube" knows the character should be going right when entering the cube
                        open[cb].direction = Array.IndexOf(current.myCube.neighbors, cb);
                    }
                }
            }

            open.Remove(current.myCube);
            closed.Add(current.myCube, current);
            current = null;
        }

        return null;
    }

    public static void ReturnPathNode(pathNode _node)
    {
        NodePool.ReturnObjectToPool(_node);
    }

    public static pathNode ReverseList(pathNode _head)
    {
        pathNode prev = null;
        pathNode current = _head;
        pathNode next = null;

        while (current != null)
        {
            next = current.parent;
            current.parent = prev;
            prev = current;
            current = next;
        }

        _head = prev;
        if (_head != null && _head.parent != null)
            return _head.parent;
        else
            return _head;
    }
}
