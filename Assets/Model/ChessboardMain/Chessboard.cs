using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.Rendering.DebugUI;

namespace Assets.Model.ChessboardMain.Pieces
{
    public class Chessboard
    {
        private readonly Dictionary<Field, Piece> fieldMap = new Dictionary<Field, Piece>();

        public void CreateField(int x, int y)
        {
            fieldMap[new Field(x, y)] = null;
        }

        public Field GetField(int x, int y)
        {
            foreach (var field in fieldMap.Keys)
            {
                if (field.Equals(x, y))
                {
                    return field;
                }
            }
            return null;
        }

        public Piece GetPiece(Field field)
        {
            fieldMap.TryGetValue(field, out var piece);
            return piece;
        }

        public void PutPiece(Field field, Piece piece)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field), "Field may not be null.");
            if (piece == null)
                throw new ArgumentNullException(nameof(piece), "Piece may not be null.");

            CheckFieldExists(field);
            fieldMap[field] = piece;
        }

        public void RemovePiece(Field field)
        {
            CheckFieldExists(field);
            fieldMap[field] = null;
        }

        public void ClearPieces()
        {
            var keys = new List<Field>(fieldMap.Keys);
            foreach (var field in keys)
            {
                fieldMap[field] = null;
            }
        }

        public int GetFieldCount()
        {
            return fieldMap.Keys.Count;
        }

        public int GetPieceCount()
        {
            int pieceCount = 0;

            foreach (var entry in fieldMap)
            {
                if (entry.Value != null)
                {
                    pieceCount++;
                }
            }

            return pieceCount;
        }

        private void CheckFieldExists(Field field)
        {
            if (!fieldMap.ContainsKey(field))
            {
                throw new InvalidOperationException($"The field ({field.X},{field.Y}) doesn't exist.");
            }
        }

        public bool ContainsPiece(Piece piece)
        {
            if (piece == null)
                throw new ArgumentNullException(nameof(piece));

            foreach (var pieceOnTheBoard in fieldMap.Values)
            {
                if (piece.Equals(pieceOnTheBoard))
                {
                    return true;
                }
            }

            return false;
        }

        public List<Field> GetAllTakenFields()
        {
            var fields = new List<Field>();
            foreach (var entry in fieldMap)
            {
                if (entry.Value != null)
                {
                    fields.Add(entry.Key);
                }
            }
            return fields;
        }

        public Field GetField(Piece piece)
        {
            if (piece == null)
                throw new ArgumentNullException(nameof(piece));

            foreach (var entry in fieldMap)
            {
                if (piece.Equals(entry.Value))
                {
                    return entry.Key;
                }
            }
            return null;

        }



        // Oyuncunun sırası kimde?
        public PieceColor CurrentPlayerColor { get; private set; } = PieceColor.White; // Başlangıçta beyaz başlasın
        public void NextTurn()
        {
            if (CurrentPlayerColor == PieceColor.White)
                CurrentPlayerColor = PieceColor.Gray;
            else if (CurrentPlayerColor == PieceColor.Gray)
                CurrentPlayerColor = PieceColor.Black;
            else
                CurrentPlayerColor = PieceColor.White;
        }

        // Hamle sonrası şah tehdit altında mı?
        public bool WouldKingBeInCheckAfterMove(Move move)
        {
            // Buraya şahın tehdit altında olup olmadığını kontrol eden kod gelecek
            return false; // Geçici olarak false dönüyoruz
        }

        // Verilen kaynaktan hedefe doğru boş mu?
        public List<Field> GetPath(Field source, Field destination)
        {
            List<Field> path = new List<Field>();
            // Buraya taşların hareket ederken yolu kapatıp kapatmadığını kontrol eden kod yazılacak
            return path;
        }

        // Tahta sınırları içinde mi?
        public bool IsWithinBounds(Field field)
        {
            return field.X >= 0 && field.X < 8 && field.Y >= 0 && field.Y < 8; // 8x8 satranç tahtası
        }


    }
}