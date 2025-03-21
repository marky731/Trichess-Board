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
            new Direction(0, 1),
            new Direction(-1, 0),
            new Direction(-1, -1),
            new Direction(0, -1),
            new Direction(1, 0),
            new Direction(1, 1)
        };

        public static IReadOnlyCollection<Direction> Get()
        {
            return DIRECTIONS;
        }
    }
}
