using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model.ChessboardMain.Pieces
{
    namespace Assets.Model.ChessboardMain.Pieces
    {
        public class Knight : Piece
        {
            private static readonly HashSet<Direction> DIRECTIONS = new HashSet<Direction>
        {
            new Direction(1, -2),
            new Direction(2, -1),
            new Direction(3, 1),
            new Direction(3, 2),
            new Direction(2, 3),
            new Direction(1, 3),
            new Direction(-1, 2),
            new Direction(-2, 1),
            new Direction(-3, -1),
            new Direction(-3, -2),
            new Direction(-2, -3),
            new Direction(-1, -3)
        };

            public Knight(PieceColor color) : base(color, false) { }

            public override string GetAbbreviation()
            {
                return "N";
            }

            public override HashSet<Direction> GetDirections()
            {
                return DIRECTIONS;
            }
        }
    }
}
