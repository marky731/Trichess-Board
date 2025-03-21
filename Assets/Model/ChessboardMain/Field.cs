using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Assets.Model
{
    public class Field
    {
        public int X { get; }
        public int Y { get; }
        public object OccupiedPiece { get; internal set; }

        public Field(int x, int y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(int x, int y)
        {
            return X == x && Y == y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Field other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (X, Y).GetHashCode();
        }
    }

}
