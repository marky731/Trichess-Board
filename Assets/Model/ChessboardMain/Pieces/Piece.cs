using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Assets.Model.ChessboardMain.Pieces
{
    public abstract class Piece
    {
        private PieceColor color;
        private bool isSliding;
        public bool IsMoved { get; private set; } = false;  //  Yeni özellik

        public Piece(PieceColor color, bool isSliding)
        {
            this.color = color;
            this.isSliding = isSliding;
        }

        public abstract string GetAbbreviation();
        public PieceColor GetColor() => color;
        public bool IsSliding() => isSliding;
        public abstract HashSet<Direction> GetDirections();

        //  Taşın hareket ettiğini güncelleyen metod
        public void SetMoved()
        {
            IsMoved = true;
        }
    }
}
