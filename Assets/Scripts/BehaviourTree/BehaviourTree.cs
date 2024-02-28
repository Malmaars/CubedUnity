using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

namespace BehaviourTree
{
    //Base class for the Nodes
    public abstract class Node
    {
        //Every Node needs to be able to relay a result, of which there are three options
        public enum Result { success, failed, running }

        //Every Node also needs to be able to run
        public abstract Result Run();
    }



    //Selector
    public class Selector : Node
    {
        private Node[] children;
        private int currentIndex = 0;
        public Selector(params Node[] _children)
        {
            children = _children;
        }
        public override Result Run()
        {
            //The selector goes through its children one by one until a child returns success or running
            for (; currentIndex < children.Length; currentIndex++)
            {
                Result result = children[currentIndex].Run();

                switch (result)
                {
                    case Result.failed:
                        break;
                    case Result.success:
                        return Result.success;
                    case Result.running:
                        return Result.running;
                }
            }

            currentIndex = 0;
            return Result.failed;
        }
    }

    //Sequencer
    public class Sequence : Node
    {
        private Node[] children;
        private int currentIndex = 0;
        public Sequence(params Node[] _children)
        {
            children = _children;
        }
        public override Result Run()
        {

            //The sequence goes through all its children and stops when one returns failed
            for (; currentIndex < children.Length; currentIndex++)
            {
                Result result = children[currentIndex].Run();

                switch (result)
                {
                    case Result.failed:
                        currentIndex = 0;
                        return Result.failed;
                    case Result.success:
                        break;
                    case Result.running:
                        return Result.running;
                }
            }

            currentIndex = 0;
            return Result.success;
        }
    }

        //Parallel Nodes

    //Success version
    public class SuccessParallel : Node
    {
        private Node[] children;
        private int currentIndex = 0;
        public SuccessParallel(params Node[] _children)
        {
            children = _children;
        }
        public override Result Run()
        {

            //Success Parallel runs all its children simultaneously until one of them returns success, or all of them return failed
            int amountOfFailures = 0;
            for (; currentIndex < children.Length; currentIndex++)
            {
                Result result = children[currentIndex].Run();

                switch (result)
                {
                    case Result.failed:
                        amountOfFailures++;
                        break;
                    case Result.success:
                        return Result.success;
                    case Result.running:
                        break;
                }
            }

            if (amountOfFailures == children.Length)
                return Result.failed;

            currentIndex = 0;
            return Result.running;
        }
    }

    //Fail version
    public class FailParallel : Node
    {
        private Node[] children;
        private int currentIndex = 0;
        public FailParallel(params Node[] _children)
        {
            children = _children;
        }
        public override Result Run()
        {
            //Success Parallel runs all its children simultaneously until one of them returns failed, or all of them return success
            int amountOfSuccesses = 0;
            for (; currentIndex < children.Length; currentIndex++)
            {
                Result result = children[currentIndex].Run();

                switch (result)
                {
                    case Result.failed:
                        return Result.failed;
                    case Result.success:
                        amountOfSuccesses++;
                        break;
                    case Result.running:
                        break;
                }
            }

            if (amountOfSuccesses == children.Length)
                return Result.success;

            currentIndex = 0;
            return Result.running;
        }
    }

        //Conditional 

    //Base for conditional nodes
    public abstract class ConditionalNode : Node
    {
        //every conditional node needs a child it can run, and it can only be one child
        //if you want more children make the child a sequence or selector etc.
        private Node child;

        public override Result Run()
        {

            Result result = child.Run();

            return result;

        }
    }

    //Interruptor Node which checks conditional Nodes. These nodes will have two constructors,
    //one for Conditional node usage, and one for interruptor node usage
    public class Interruptor : Node
    {
        //The interruptor stops its child node when the condition of the given conditionalnode has been met

        ConditionalNode condition;
        Node child;

        public Interruptor(ConditionalNode _condition, Node _child)
        {
            condition = _condition;
            child = _child;
        }

        Node actionsUponExit;

        //Interruptor version that performs specific action(s) when interrupting
        public Interruptor(ConditionalNode _condition, Node _child, Node _actionsUponExit)
        {
            condition = _condition;
            child = _child;
            actionsUponExit = _actionsUponExit;
        }

        public override Result Run()
        {
            Result result = condition.Run();

            if (result == Result.success)
            {
                if (actionsUponExit != null)
                {
                    actionsUponExit.Run();
                    
                }
                return Result.failed;
            }

            result = child.Run();

            return result;
        }
    }
}
