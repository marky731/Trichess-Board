using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

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

            public override List<string> GetPossibleMoves(Board board)
            {
                List<string> possibleMoves = new List<string>();
                Field currentField = board.GetField(this.CurrentPosition); // Mevcut konumu al

                if (currentField == null) return possibleMoves; // Eğer geçersizse boş liste dön

                Debug.Log($"Checking possible moves for Gray Pawn at {currentField.X},{currentField.Y}");

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
                    Debug.Log($"Gray Pawn can move to {nextField.X},{nextField.Y}");
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
                        Debug.Log($"Gray Pawn can attack enemy piece at {nextField.X},{nextField.Y}");
                    }
                }

                // Tüm geçerli hamleleri döndür
                return possibleMoves;
            }
        }
    }
