using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using global::Assets.Model; // Add this for Move class

namespace Assets.Model.ChessboardMain.Pieces.Pawn
{
    public class BlackPawn : Pawn
    {
        private static readonly HashSet<Direction> DIRECTIONS = new HashSet<Direction>
        {
            Direction.BlackForward // Use the predefined direction for black pawns
        };

        private static readonly HashSet<Direction> TAKING_DIRECTIONS = new HashSet<Direction>
        {
            Direction.BlackForwardRight, // Diagonal right for capturing
            Direction.BlackForwardLeft   // Diagonal left for capturing
        };

        public BlackPawn(GameObject gameObject, int playerId)
            : base(PieceColor.Black, gameObject, playerId) { }

        public override HashSet<Direction> GetDirections()
        {
            return DIRECTIONS;
        }

        public override HashSet<Direction> GetTakingDirections()
        {
            return TAKING_DIRECTIONS;
        }

        public override List<string> GetPossibleMoves(global::Board board)
        {
            List<string> possibleMoves = new List<string>();
            Field currentField = board.GetField(this.CurrentPosition);

            if (currentField == null) return possibleMoves;  

            Debug.Log($"Checking possible moves for Black Pawn at {currentField.Position}");

            foreach (Direction direction in GetDirections())
            {
                Field nextField = direction.Move(currentField);

                if (nextField == null || !board.IsValidMove(this, new Move(currentField, this, nextField, nextField.OccupiedPiece)))
                    continue;

                possibleMoves.Add($"{nextField.X},{nextField.Y}");

                Debug.Log($"Black Pawn can move to {nextField.Position}");
            }


            foreach (Direction direction in GetTakingDirections())
            {

                Field nextField = direction.Move(currentField);

                if (nextField == null) continue;


                if (nextField.OccupiedPiece != null && nextField.OccupiedPiece.GetColor() != this.GetColor())
                {
                    possibleMoves.Add($"{nextField.X},{nextField.Y}");
                    Debug.Log($"Black Pawn can attack enemy piece at {nextField.X},{nextField.Y}");
                }
            }


            return possibleMoves;
        }
    }
}
