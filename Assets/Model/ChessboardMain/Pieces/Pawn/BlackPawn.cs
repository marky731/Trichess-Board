using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model.ChessboardMain.Pieces.Pawn
{
    public class BlackPawn : Pawn
    {
        private static readonly HashSet<Direction> DIRECTIONS = new HashSet<Direction>
        {
            new Direction(-1, 0),
            new Direction(0, 1)
        };

        private static readonly HashSet<Direction> TAKING_DIRECTIONS = new HashSet<Direction>
        {
            new Direction(-1, 1),
            new Direction(-2, -1),
            new Direction(1, 2)
        };

        public BlackPawn() : base(PieceColor.Black) { }

        public override HashSet<Direction> GetDirections()
        {
            return DIRECTIONS;
        }

        public override HashSet<Direction> GetTakingDirections()
        {
            return TAKING_DIRECTIONS;
        }
    }
}