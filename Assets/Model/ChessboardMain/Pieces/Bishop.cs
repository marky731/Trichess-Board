using Assets.Model.ChessboardMain;
using Assets.Model.ChessboardMain.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Assets.Model.ChessboardMain.Pieces
{
    public class Bishop : Piece
    {
        private static readonly HashSet<Direction> DIRECTIONS = DiagonalDirections.Get();

        public Bishop(PieceColor color) : base(color, true) { }

        public override string GetAbbreviation()
        {
            return "B";
        }

        public override HashSet<Direction> GetDirections()
        {
            return DIRECTIONS;
        }
    }
}
