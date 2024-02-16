using System;
using UnityEngine;

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

        public Vector2Int Pos => new(X, Y);

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

        public int CountConnection()
        {
            int count = 0;
            if (Top) count++;
            if (Right) count++;
            if (Bottom) count++;
            if (Left) count++;
            return count;
        }

        public static void Connect(Node node1, Node node2)
        {
            if (Math.Abs(node1.X - node2.X) + Math.Abs(node1.Y - node2.Y) != 1)
            {
                return;
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
                node1.Right = true;
                node2.Left = true;
            }
            else if (node2.Y == node1.Y + 1)
            {
                node1.Top = true;
                node2.Bottom = true;
            }
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
                return node1.Top == node2.Bottom;
            }

            return true;
        }
    }
}