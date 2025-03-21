using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model.ChessboardMain.Pieces.Pawn
{
    public class GrayPawn : Pawn
    {
        private static readonly HashSet<Direction> DIRECTIONS = new HashSet<Direction>
        {
            new Direction(0, -1),
            new Direction(-1, -1)
        };

        private static readonly HashSet<Direction> TAKING_DIRECTIONS = new HashSet<Direction>
        {
            new Direction(-2, -1),
            new Direction(-1, -2),
            new Direction(1, -1)
        };

        public GrayPawn() : base(PieceColor.Gray) { }

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