using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Model.ChessboardMain
{
    public static class StraightDirections
    {
        private static readonly HashSet<Direction> DIRECTIONS = new HashSet<Direction>
        {
            //  Beyaz taşları için düz yönler
            Direction.WhiteForward,
            Direction.WhiteBackward,

            //  Siyah taşları için düz yönler
            Direction.BlackForward,
            Direction.BlackBackward,

            //  Gri taşları için düz yönler
            Direction.GrayForward,
            Direction.GrayBackward
        };

        public static IReadOnlyCollection<Direction> Get()
        {
            return DIRECTIONS;
        }
    }
}