using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Assets.Model.ChessboardMain.Pieces
{
    public class King : Piece
    {
        private static readonly HashSet<Direction> DIRECTIONS = new HashSet<Direction>();

        static King()
        {
            DIRECTIONS.UnionWith(DiagonalDirections.Get());
            DIRECTIONS.UnionWith(StraightDirections.Get());
        }

        public King(PieceColor color) : base(color, false) { }

        public override string GetAbbreviation()
        {
            return "K";
        }

        public override HashSet<Direction> GetDirections()
        {
            return DIRECTIONS;
        }
    }
}
