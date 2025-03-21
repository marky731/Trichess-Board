using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model.ChessboardMain
{
    public class Direction
    {
        public int X { get; }
        public int Y { get; }

        public Direction(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static readonly Direction Up = new Direction(0, 1);
        public static readonly Direction Down = new Direction(0, -1);
        public static readonly Direction Left = new Direction(-1, 0);
        public static readonly Direction Right = new Direction(1, 0);
        public static readonly Direction DiagonalUpLeft = new Direction(-1, 1);
        public static readonly Direction DiagonalUpRight = new Direction(1, 1);
        public static readonly Direction DiagonalDownLeft = new Direction(-1, -1);
        public static readonly Direction DiagonalDownRight = new Direction(1, -1);

        public static List<Direction> GetAllDirections()
        {
            return new List<Direction>
            {
                Up, Down, Left, Right,
                DiagonalUpLeft, DiagonalUpRight,
                DiagonalDownLeft, DiagonalDownRight
            };
        }

        // Eski DirectionHelper.GetDirection metodunu buraya taşıdık
        public static Direction GetDirection(Field source, Field destination)
        {
            int dx = destination.X - source.X;
            int dy = destination.Y - source.Y;

            foreach (var dir in GetAllDirections())
            {
                if (dir.X == dx && dir.Y == dy)
                    return dir;
            }

            throw new ArgumentException("Invalid move direction.");
        }
    }
}