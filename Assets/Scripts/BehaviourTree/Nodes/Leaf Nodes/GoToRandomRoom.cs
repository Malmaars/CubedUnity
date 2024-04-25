namespace BehaviourTree
{
    public class GoToRandomRoom : Node
    {
        bool running;
        Character character;
        WalkToRoom walker;
        public GoToRandomRoom(Character _character)
        {
            character = _character;
        }

        //I need some way to check when the arrangement of the cubes changes. Even while the character is walking
        //I could actively check if the neighbor of the cube is the same as the cube it's going to. When that changes, the character knows its path has changed
        public override Result Run()
        {
            UpdateTracking(character);

            if (running == false)
            {
                pathNode path = FindRoom.Run(character);
                path = FindRoom.ReverseList(path);

                //there is no possible path, operation failed
                if (path == null)
                    return Result.failed;

                walker = Blackboard.walkPool.RequestItem();
                walker.AssignData(path, character);
                running = true;
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
    }
}