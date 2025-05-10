using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Model.ChessboardMain.Pieces
{

    public class Queen : Piece
    {
        private static readonly HashSet<Direction> DIRECTIONS = new HashSet<Direction>(
            DiagonalDirections.Get().Union(StraightDirections.Get())
        );

        public Queen(PieceColor color, GameObject gameObject, int playerId) : base(color, true, gameObject, playerId) { }

        public override string GetAbbreviation()
        {
            return "Q";
        }

        public override HashSet<Direction> GetDirections()
        {
            return DIRECTIONS;
        }


        public override List<string> GetPossibleMoves(Board board)
        {
            List<string> possibleMoves = new List<string>();
            Field currentField = board.GetField(this.CurrentPosition); 

            if (currentField == null) return possibleMoves; 

            foreach (Direction direction in GetDirections())
            {
                Field nextField = currentField; 
                while (true)
                {
                    nextField = direction.Move(nextField); 

                    if (nextField == null || !board.IsValidMove(this, new Move(currentField, this, nextField, nextField.OccupiedPiece)))
                        break;

                    possibleMoves.Add($"{nextField.X},{nextField.Y}"); 

                    if (board.IsOccupied(nextField)) 
                        break;
                }
            }

            return possibleMoves;
        }
    }

}