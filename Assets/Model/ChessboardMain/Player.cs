using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model.ChessboardMain
{
    public class Player
    {
        public PieceColor Color { get; }

        public Player(PieceColor color)
        {
            Color = color;
        }

        public PieceColor GetPlayerColor()
        {
            return Color;
        }
    }

}