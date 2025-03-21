using System.Collections.Generic;

namespace Assets.Model.ChessboardMain.Pieces
{
    namespace Assets.Model.ChessboardMain.Pieces
    {
        public class Rook : Piece
        {
            private static readonly HashSet<Direction> DIRECTIONS = (HashSet<Direction>)StraightDirections.Get();

            public Rook(PieceColor color) : base(color, true) { }

            public override string GetAbbreviation()
            {
                return "R";
            }

            public override HashSet<Direction> GetDirections()
            {
                return DIRECTIONS;
            }
        }
    }
}
