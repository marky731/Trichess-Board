using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Model.ChessboardMain.Pieces
{
    
        public class Knight : Piece
        {
            private static readonly HashSet<Direction> DIRECTIONS = new HashSet<Direction>
        {
            new Direction(1, -2),
            new Direction(2, -1),
            new Direction(3, 1),
            new Direction(3, 2),
            new Direction(2, 3),
            new Direction(1, 3),
            new Direction(-1, 2),
            new Direction(-2, 1),
            new Direction(-3, -1),
            new Direction(-3, -2),
            new Direction(-2, -3),
            new Direction(-1, -3)
        };

            public Knight(PieceColor color, GameObject gameObject, int playerId) : base(color, false, gameObject, playerId) { }

            public override string GetAbbreviation()
            {
                return "N";
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
                    Field nextField = direction.Move(currentField); 

                    if (nextField == null) continue; 

                    if (board.IsValidMove(this, new Move(currentField, this, nextField, nextField.OccupiedPiece)))
                    {
                        possibleMoves.Add($"{nextField.X},{nextField.Y}");
                    }
                }

                return possibleMoves;
            }
        }
    }

        

