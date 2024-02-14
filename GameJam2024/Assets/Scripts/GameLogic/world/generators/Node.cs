using System;

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

        public void SetConnections(bool top, bool right, bool bottom, bool left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public Node(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Node(int x, int y, bool top, bool right, bool bottom, bool left) : this(x, y)
        {
            SetConnections(top, right, bottom, left);
        }

        public static bool ConnectionsMatch(Node node1, Node node2)
        {
            if (Math.Abs(node2.X - node1.X) + Math.Abs(node2.Y - node1.Y) != 1)
            {
                return true;
            }
            
            if (node1.X > node2.X)
            {
                (node1, node2) = (node2, node1);
            }
            else if (node1.Y > node2.Y)
            {
                (node1, node2) = (node2, node1);
            }
            
            if (node2.X == node1.X + 1)
            {
                return node1.Right == node2.Left;
            }
            if (node2.Y == node1.Y + 1)
            {
                return node1.Top = node2.Bottom;
            }

            return true;
        }
    }
}