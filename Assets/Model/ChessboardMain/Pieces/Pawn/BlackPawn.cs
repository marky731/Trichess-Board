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
        // Siyah piyonun düz hareket yönlerini belirler (Bir kare ileri)
        private static readonly HashSet<Direction> DIRECTIONS = new HashSet<Direction>
        {
            Direction.BlackForward // Use the predefined direction for black pawns
        };

        // Siyah piyonun çapraz alma yönlerini belirler (Diagonalde taş alma)
        private static readonly HashSet<Direction> TAKING_DIRECTIONS = new HashSet<Direction>
        {
            Direction.BlackForwardRight, // Diagonal right for capturing
            Direction.BlackForwardLeft   // Diagonal left for capturing
        };

        // Eksik `gameObject` ve `playerId` parametreleri eklendi
        public BlackPawn(GameObject gameObject, int playerId)
            : base(PieceColor.Black, gameObject, playerId) { }

        // Piyonun düz hareket yönlerini döndürür
        public override HashSet<Direction> GetDirections()
        {
            return DIRECTIONS;
        }

        // Piyonun taş alırken gidebileceği yönleri döndürür
        public override HashSet<Direction> GetTakingDirections()
        {
            return TAKING_DIRECTIONS;
        }

        // Siyah piyonun geçebileceği tüm olası hamleleri döndürür
        public override List<string> GetPossibleMoves(global::Board board)
        {
            List<string> possibleMoves = new List<string>();
            Field currentField = board.GetField(this.CurrentPosition);

            if (currentField == null) return possibleMoves;  // Eğer mevcut alan geçerli değilse boş liste döndür

            Debug.Log($"Checking possible moves for Black Pawn at {currentField.Position}");

            // Düz hareket yönlerini kontrol et
            foreach (Direction direction in GetDirections())
            {
                // Mevcut alandan, belirli bir yönde hareket edilen alana geçiş yapılır
                Field nextField = direction.Move(currentField);

                // Eğer hedef alan geçerli değilse veya hamle geçerli değilse, o yönü geç
                if (nextField == null || !board.IsValidMove(this, new Move(currentField, this, nextField, nextField.OccupiedPiece)))
                    continue;

                // Geçerli bir hamle bulunursa, hedef pozisyonu listeye ekle
                possibleMoves.Add($"{nextField.X},{nextField.Y}");

                // Debug.log ile her geçerli hamleyi yazdır
                Debug.Log($"Black Pawn can move to {nextField.Position}");
            }

            // Çapraz hareket (taş alma) yönlerini kontrol et
            foreach (Direction direction in GetTakingDirections())
            {
                // Mevcut alandan, belirli bir çapraz yönde taş alma yapılabilir
                Field nextField = direction.Move(currentField);

                if (nextField == null) continue;

                // Eğer hedef alanda rakip taş varsa, taş alınabilir
                if (nextField.OccupiedPiece != null && nextField.OccupiedPiece.GetColor() != this.GetColor())
                {
                    possibleMoves.Add($"{nextField.X},{nextField.Y}");
                    Debug.Log($"Black Pawn can attack enemy piece at {nextField.X},{nextField.Y}");
                }
            }

            // Tüm geçerli hamleleri döndür
            return possibleMoves;
        }
    }
}
