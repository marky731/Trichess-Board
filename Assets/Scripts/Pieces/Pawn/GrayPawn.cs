using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using global::Assets.Model; // Add this for Move class

namespace Assets.Model.ChessboardMain.Pieces.Pawn
{
    public class GrayPawn : Pawn
    {

        private static readonly HashSet<Direction> DIRECTIONS = new HashSet<Direction>
        {
            Direction.GrayForward // Use the predefined direction for gray pawns
        };


        private static readonly HashSet<Direction> TAKING_DIRECTIONS = new HashSet<Direction>
        {
            Direction.GrayForwardRight, // Diagonal right for capturing
            Direction.GrayForwardLeft   // Diagonal left for capturing
        };


        public GrayPawn(GameObject gameObject, int playerId)
            : base(PieceColor.Gray, gameObject, playerId) { }


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

            Debug.Log($"Checking possible moves for Gray Pawn at {currentField.Position}");

            // Düz hareket yönlerini kontrol et
            foreach (Direction direction in GetDirections())
            {
                Field nextField = direction.Move(currentField);

                if (nextField == null || !board.IsValidMove(this, new Move(currentField, this, nextField, nextField.OccupiedPiece)))
                    continue;

                possibleMoves.Add($"{nextField.X},{nextField.Y}");

                Debug.Log($"Gray Pawn can move to {nextField.Position}");
            }

            foreach (Direction direction in GetTakingDirections())
            {
                Field nextField = direction.Move(currentField);

                if (nextField == null) continue;

                if (nextField.OccupiedPiece != null && nextField.OccupiedPiece.GetColor() != this.GetColor())
                {
                    possibleMoves.Add($"{nextField.X},{nextField.Y}");
                    Debug.Log($"Gray Pawn can attack enemy piece at {nextField.X},{nextField.Y}");
                }
            }

            return possibleMoves;
        }
    }
}
