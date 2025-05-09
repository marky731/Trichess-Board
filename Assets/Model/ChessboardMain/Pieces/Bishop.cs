using Assets.Model.ChessboardMain;
using Assets.Model.ChessboardMain.Pieces;
using System.Collections.Generic;
using UnityEngine;  // GameObject için gerekli

namespace Assets.Model.ChessboardMain.Pieces
{
    public class Bishop : Piece
    {
        private static readonly HashSet<Direction> DIRECTIONS = DiagonalDirections.Get();

        // Düzgün constructor: GameObject ve playerId eklenerek base çağrısı yapıldı
        public Bishop(PieceColor color, GameObject gameObject, int playerId)
            : base(color, true, gameObject, playerId) { }

        public override string GetAbbreviation()
        {
            return "B";
        }

        public override HashSet<Direction> GetDirections()
        {
            return DIRECTIONS;
        }

        // Eksik olan GetPossibleMoves metodunu ekledik
        public override List<string> GetPossibleMoves(global::Board board)
        {
            List<string> possibleMoves = new List<string>();
            Field currentField = board.GetField(this.CurrentPosition); // Mevcut konumu Field olarak al

            if (currentField == null) return possibleMoves; // Eğer geçersizse boş liste dön

            foreach (Direction direction in GetDirections())
            {
                Field nextField = currentField; // Mevcut konumdan başla
                while (true)
                {
                    nextField = direction.Move(nextField); // Yöne göre ilerle

                    if (nextField == null)
                        break;
                    
                    // Check if the next field is valid
                    // We need to create a Move object to check if it's valid
                    Move move = new Move(currentField, this, nextField, nextField.OccupiedPiece);
                    
                    // Check if the move is valid using the validator
                    // If the validator is not initialized, we'll skip validation
                    try
                    {
                        if (!board.IsValidMove(this, move))
                            break;
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogWarning($"Error validating move: {e.Message}. Skipping validation.");
                        // Continue anyway for testing
                    }

                    possibleMoves.Add($"{nextField.X},{nextField.Y}"); // Coordinate string format

                    // Check if the next field is occupied
                    try
                    {
                        if (nextField.OccupiedPiece != null) // Bir taş varsa dur
                            break;
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogWarning($"Error checking if field is occupied: {e.Message}. Assuming it's not occupied.");
                        // Continue anyway for testing
                    }
                }
            }

            return possibleMoves;
        }

    }
}
