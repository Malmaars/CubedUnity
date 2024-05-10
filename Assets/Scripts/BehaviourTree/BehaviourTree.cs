using UnityEngine;

namespace BehaviourTree
{
    //Base class for the Nodes
    public abstract class Node : ITrackableNode
    {
        //Every Node needs to be able to relay a result, of which there are three options
        public enum Result { success, failed, running }

        //Every Node also needs to be able to run
        public abstract Result Run();

        public void UpdateTracking(Character c)
        {
            c.currentBehaviour = this.GetType().ToString();
        }
    }

    //interface for trackable nodes, ones that can let a character show what their current node is. Made for leaf nodes
    public interface ITrackableNode
    {
        public void UpdateTracking(Character c);
    }

    //A node that can grant an item to something
    public class ItemNode : Node
    {
        InventoryItem item;
        public override Result Run()
        {
            throw new System.NotImplementedException();
        }
    }

    //service nodes are nodes that can be invoked gathered by a character, and they will invoke it on themselves
    public class ServiceNode : Node
    {
        bool running;

        IServicable servicable;
        //the action they will perform. If it returns failed or success, put the character back into their base state
        Node child;

        public ServiceNode(IServicable _servicable ,Node _child) 
        {
            servicable = _servicable;
            child = _child; 
        }

        public override Result Run()
        {
            Result result = child.Run();

            if (result == Result.failed || result == Result.success)
            {
                EndService();
                return result;
            }

            servicable.beingUsed = true;
            servicable.asker.interacting = true;
            return result;
        }

        protected void EndService()
        {
            servicable.EndService();
            servicable.beingUsed = false;
            if (servicable.asker != null)
            {
                servicable.asker.interacting = false;
                servicable.asker = null;
            }
        }
    }

    //these are nodes that invoke reactions from other characters. 
    public class InvokeNode : Node
    {
        //Every invoke node requires an owner (the one that invokes)
        protected Character owner;

        //These Nodes will have a child node (the reaction), but they will assign it themselves
        protected ReactionNode child;

        public override Result Run()
        {
            throw new System.NotImplementedException();
        }

        protected void EndReaction()
        {
            owner.interacting = false;
            owner.target.ResetTree();
            owner.target = null;
            child.done = true;
        }
    }

    //A node for a node that serves as a reaction. They always return running, and are dependent on the InvokeNode parent.
    public class ReactionNode : Node
    {
        protected Character character;
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

        public virtual void AssignNewOwner(Character c) { character = c; }
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

    //A forced sequence will go through each child regardless if they return success or failed
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

        bool runningActions;

        public Interruptor(ConditionalNode _condition, Node _child)
        {
            condition = _condition;
            child = _child;
        }

        Node actionsUponExit;

        //Interruptor version that performs specific action(s) when interrupting
        public Interruptor(ConditionalNode _condition, Node _actionsUponExit, Node _child)
        {
            condition = _condition;
            child = _child;
            actionsUponExit = _actionsUponExit;
        }

        public override Result Run()
        {
            if (runningActions)
            {
                Result actionResult = actionsUponExit.Run();
                if (actionResult == Result.failed || actionResult == Result.success)
                {
                    runningActions = false;
                    return actionResult;
                }
                return Result.running;
            }

            Result result = condition.Run();

            if (result == Result.success)
            {
                if (actionsUponExit != null)
                {
                    runningActions = true;
                    return Result.running;
                }
                return Result.failed;
            }

            result = child.Run();

            return result;
        }
    }
    
    //based on the result of its child, the splitter will pick one of two routes
    public class Splitter : Node
    {
        bool split;
        Node currentChild, conditionChild, failedChild, succesChild;

        public Splitter(Node condition, Node failed, Node succes)
        {
            conditionChild = condition;
            failedChild = failed;
            succesChild = succes;
        }

        public override Result Run()
        {
            if (!split)
            {
                Result result = conditionChild.Run();

                if (result == Result.failed)
                {
                    currentChild = failedChild;
                    split = true;
                }

                else if (result == Result.success)
                {
                    currentChild = succesChild;
                    split = true;
                }

                return Result.running;
            }

            else
            {
                //the splitter has been split, run the split end
                Result result = currentChild.Run();

                if(result == Result.failed || result == Result.success)
                {
                    split = false;
                    currentChild = null;
                    return result;
                }
                return result;
            }
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
