namespace GameLogic.world.generators
{
    public class Node
    {
        public readonly int X;
        public readonly int Y;

        public int Value = 0;

        public bool Marked;
        public bool Top;
        public bool Right;
        public bool Bottom;
        public bool Left;

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}