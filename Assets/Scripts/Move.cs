using Assets.Model.ChessboardMain;
using Assets.Model.ChessboardMain.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Assets.Model.ChessboardMain.Pieces
{
    public class Move
    {
        public Field Source { get; }
        public Piece MovedPiece { get; }
        public Field Destination { get; }
        public Piece TakenPiece { get; }
        public bool Moved { get; }
        public Piece PromotionPiece { get; private set; }

        public Move(Field source, Piece movedPiece, Field destination, Piece takenPiece = null)
        {
            MovedPiece = movedPiece ?? throw new ArgumentNullException(nameof(movedPiece));
            Source = source;
            Destination = destination;
            TakenPiece = takenPiece;
            Moved = movedPiece.IsMoved;
            MovedPiece.SetMoved();
        }
    }
}
