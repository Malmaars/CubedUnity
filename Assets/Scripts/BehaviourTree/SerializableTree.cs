using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;

//It's more effort than it's worth. Don't use the serializable tree
//I'll keep it here for documentation
public enum ScriptableNode
{
    Sequence,
    ForcedSequence,
    Selector,
    Interruptor,
    Idle,
    CheckConfused,
    CheckReset,
    Confused,
    FaceOwnerAndTarget,
    LockTarget,
    LookForTarget,
    SolveNeed,
    Talk,
    TargetCondition,
    Wait,
    WalkToNearestRoom,
    GoToTarget,
    GoToRandomRoom
}

[Serializable]
public class SerializableNode
{
    [SerializeField]
    public ScriptableNode thisNode;

    public SerializableNode[] children;
}

public static class SerializableTree
{
    public static Node[] MakeTree(SerializableNode[] _nodes, Character c)
    {
        Node[] temp = new Node[_nodes.Length];

        for(int i = 0; i <_nodes.Length; i++)
        {
            temp[i] = MakeNode(_nodes[i], c);
        }

        return temp;
    }

    public static Node MakeNode(SerializableNode _node, Character c)
    {
        //check what the parameters of the constructors are

        Debug.Log(_node.thisNode.ToString());
        Type node = Type.GetType("BehaviourTree." + _node.thisNode.ToString());

        //get each possible constructor of the node
        foreach (ConstructorInfo constructor in node.GetConstructors())
        {
            //Get all parameters of the constructor
            ParameterInfo[] parameters = constructor.GetParameters();

            bool characterBool = false;
            bool nodeBool = false;
            bool nodesBool = false;

            bool interruptor = false;

            foreach (ParameterInfo parameter in parameters)
            {
                //all of the tree uses Character as a parameter, or Node for children

                if(parameter.ParameterType == typeof(Character))
                {
                    characterBool = true;
                }
                if(parameter.ParameterType == typeof(Node))
                {
                    nodeBool = true;
                }
                if(parameter.ParameterType == typeof(Node[]))
                {
                    nodesBool = true;
                }
                if(parameter.ParameterType == typeof(ConditionalNode))
                {
                    interruptor = true;
                }
            }


            Node instance = null;
            if (interruptor)
            {
                //the interruptor node is very specific. If I want to make other nodes that are this specific, going the serializable route may not be the best option.
                //It's more effort than it's worth

                instance = (Node)constructor.Invoke(new object[] { c, MakeNode(_node.children[0], c) });
                return instance;
            }

            if (characterBool)
            {
                if (nodeBool)
                {
                    instance = (Node)constructor.Invoke(new object[] { c, MakeNode(_node.children[0], c)});
                    return instance;
                }

                if (nodesBool)
                {
                    Node[] childrenNodes = new Node[_node.children.Length];
                    for (int i = 0; i < _node.children.Length; i++)
                    {
                        childrenNodes[i] = MakeNode(_node.children[i], c);
                    }
                    instance = (Node)constructor.Invoke(new object[] { c, childrenNodes });
                    return instance;
                }

                instance = (Node)constructor.Invoke(new object[] { c });
                return instance;
            }

            if (nodeBool)
            {
                instance = (Node)constructor.Invoke(new object[] { MakeNode(_node.children[0], c) });
                return instance;
            }

            if (nodesBool)
            {
                Node[] childrenNodes = new Node[_node.children.Length];
                for(int i = 0; i < _node.children.Length; i++)
                {
                    childrenNodes[i] = MakeNode(_node.children[i], c);
                }
                instance = (Node)constructor.Invoke(new object[] { childrenNodes });
                return instance;
            }

            instance = (Node)Activator.CreateInstance(node);
            return instance;
        }
        return null;
    }
}
