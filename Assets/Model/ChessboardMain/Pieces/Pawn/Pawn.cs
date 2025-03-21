using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model.ChessboardMain.Pieces.Pawn
{
    public abstract class Pawn : Piece
    {
        protected Pawn(PieceColor color) : base(color, false) { }

        /// <summary>
        /// Piyonlar, taş alma için farklı yönlere sahiptir.
        /// </summary>
        public abstract HashSet<Direction> GetTakingDirections();

        public override string GetAbbreviation()
        {
            return "P";
        }
    }
}
