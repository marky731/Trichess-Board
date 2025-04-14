using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Experimental.GraphView;

namespace Assets.Model.ChessboardMain.Pieces
{
    public static class DiagonalDirections
    {
        public static HashSet<Direction> Get()
        {
            return new HashSet<Direction>
            {
                Direction. WhiteForwardRight,
                Direction.WhiteForwardLeft,
                Direction.WhiteBackwardRight,
                Direction.WhiteBackwardLeft,
                Direction.BlackForwardRight,
                Direction.BlackForwardLeft,
                Direction.BlackBackwardRight,
                Direction.BlackBackwardLeft,
                Direction.GrayForwardRight,
                Direction.GrayForwardLeft,
                Direction.GrayBackwardRight,
                Direction.GrayBackwardLeft,

            };
        }
    }
}