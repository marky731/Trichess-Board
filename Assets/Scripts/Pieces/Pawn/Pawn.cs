using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Model.ChessboardMain.Pieces.Pawn
{
    public abstract class Pawn : Piece
    {
        protected Pawn(PieceColor color, GameObject gameObject, int playerId)
            : base(color, false, gameObject, playerId) { }

        
      
        public abstract HashSet<Direction> GetTakingDirections();

        
        public abstract override List<string> GetPossibleMoves(Board board);

        public override string GetAbbreviation()
        {
            return "P";
        }
    }
}

