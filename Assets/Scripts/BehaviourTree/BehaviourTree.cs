using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //these are nodes that invoke reactions from other characters. 
    public class InvokeNode : Node
    {
        //These Nodes will have a child node (the reaction), but they will assign it themselves
        protected ReactionNode child;

        public override Result Run()
        {
            throw new System.NotImplementedException();
        }

        protected void EndReaction()
        {
            child.done = true;
        }
    }

    //A node for a node that serves as a reaction. They always return running, and are dependent on the InvokeNode parent.
    public class ReactionNode : Node
    {
        //bool accessed by Invoke Node. When it is called, the ReactionNode knows to finish
        public bool done;
        public override Result Run()
        {
            if(done)
            {
                return Result.success;
            }

            return Result.running;
        }

        public void ExitNode()
        {
            done = false;
        }
    }

    //Need nodes are nodes that are dependent on what the character needs.
    public class NeedNode : Node
    {
        public override Result Run()
        {
            throw new System.NotImplementedException();
        }

        public float CalculatePriority()
        {
            return 0;
        }
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

    //Random Selector
    //The Random Selector randomly selects one of its children to run
    public class RandomSelector : Node
    {
        private Node selectedChild;
        private Node[] children;
        private int currentIndex = 0;
        private bool running = false;
        public RandomSelector(params Node[] _children)
        {
            children = _children;
        }
        public override Result Run()
        {
            if (!running)
            {
                selectedChild = children[Random.Range(0, children.Length)];
                running = true;
            }

            while(running)
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

    //A forced sequence will go through each child regardless if they retrun success or failed
    public class ForcedSequence : Node
    {
        private Node[] children;
        private int currentIndex = 0;
        public ForcedSequence(params Node[] _children)
        {
            children = _children;
        }
        public override Result Run()
        {
            for (; currentIndex < children.Length; currentIndex++)
            {
                Result result = children[currentIndex].Run();

                switch (result)
                {
                    case Result.failed:
                        break;
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
        //a conditional node has a condition to run its child node. If the condition is failed, it can stop its child node

        //every conditional node needs a child it can run, and it can only be one child
        //if you want more children make the child a sequence or selector etc.
        protected Node child;

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

    //a decorator can have only one child, and will change the outcome, or the way that child is running
    public class Decorator : Node
    {
        Node child;

        public Decorator(Node _child)
        {
            child = _child;
        }

        public override Result Run()
        {
            return child.Run();
        }
    }
}
