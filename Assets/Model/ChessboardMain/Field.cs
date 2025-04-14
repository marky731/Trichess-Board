using Assets.Model.ChessboardMain.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Assets.Model
{
    public class Field
    {

        public double X { get; }
        public double Y { get; }
        public Piece OccupiedPiece { get; internal set; }
        public string Position => $"{X},{Y}";


        public Field(double x, double y)
        {
            X = x;
            Y = y;
        }

        public bool Equals(double x, double y)
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
