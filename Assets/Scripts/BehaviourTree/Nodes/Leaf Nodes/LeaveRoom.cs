using System.Collections.Generic;

namespace BehaviourTree 
{
    public class LeaveRoom : Node
    {
        bool running;
        Character character;
        WalkToRoom walker;

        public LeaveRoom(Character c) { character = c; }
        public override Result Run()
        {
            UpdateTracking(character);

            if (!running)
            {
                //find the closest room that is unoccupied
                if (!CheckIfPossible())
                {
                    //I don't know yet what a character will do when there are no unoccupied rooms (perhaps go home)
                    return Result.failed;
                }

                else
                {
                    pathNode path = FindRoom.Run(character, targetCube);
                    path = FindRoom.ReverseList(path);
                    walker = Blackboard.walkPool.RequestItem();
                    walker.AssignData(path, character);
                    running = true;
                }
            }

            Result result = walker.WalkToCube();

            if (result == Result.failed || result == Result.success)
            {
                Blackboard.walkPool.ReturnObjectToPool(walker);
                walker = null;
                running = false;
            }
            return result;
        }

        List<Cube> Visited = new List<Cube>();

        Cube targetCube;
        bool CheckIfPossible()
        {
            targetCube = null;
            Visited.Clear();
            Visited.Add(character.currentRoom);

            return CheckNeighbours(character.currentRoom);
        }

        bool CheckNeighbours(Cube parent)
        {
            foreach (Cube cb in parent.neighbors)
            {
                if (cb == null || Visited.Contains(cb))
                    continue;

                if (!cb.occupied)
                {
                    targetCube = cb;
                    return true;
                }

                Visited.Add(cb);
                if (CheckNeighbours(cb))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
