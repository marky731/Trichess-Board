using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model.ChessboardMain.Pieces
{
    namespace Assets.Model.ChessboardMain.Pieces
    {
        public class Queen : Piece
        {
            private static readonly HashSet<Direction> DIRECTIONS = new HashSet<Direction>(
                DiagonalDirections.Get().Union(StraightDirections.Get())
            );

            public Queen(PieceColor color) : base(color, true) { }

            public override string GetAbbreviation()
            {
                return "Q";
            }

            public override HashSet<Direction> GetDirections()
            {
                return DIRECTIONS;
            }
        }
    }
}
