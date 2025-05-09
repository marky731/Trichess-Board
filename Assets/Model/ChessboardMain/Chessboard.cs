using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Assets.Model.ChessboardMain.Pieces
{
    public class Chessboard
    {
        private readonly Dictionary<Field, Piece> fieldMap = new Dictionary<Field, Piece>();

        // Constructor to initialize the chessboard with all valid positions
        public Chessboard()
        {
            Debug.Log("Initializing Chessboard with all valid positions...");
            InitializeFields();
        }

        // Initialize the fieldMap with all valid positions from the Board class
        private void InitializeFields()
        {
            try
            {
                // Find the Board component
                Board board = UnityEngine.Object.FindFirstObjectByType<Board>();
                if (board == null)
                {
                    Debug.LogError("Board not found! Cannot initialize Chessboard.");
                    return;
                }

                // Get all positions from the Board
                List<string> allPositions = board.GetAllPositions();
                if (allPositions == null || allPositions.Count == 0)
                {
                    Debug.LogError("No positions found in Board! Cannot initialize Chessboard.");
                    return;
                }

                Debug.Log($"Found {allPositions.Count} positions in Board.");

                // Create a Field for each position and add it to the fieldMap
                foreach (string position in allPositions)
                {
                    Vector2 pos = board.GetPosition(position);
                    Field field = new Field(pos.x, pos.y);
                    fieldMap[field] = null;
                    Debug.Log($"Added field at position {position} ({pos.x}, {pos.y}) to Chessboard.");
                }

                Debug.Log($"Chessboard initialized with {fieldMap.Count} fields.");
            }
            catch (Exception e)
            {
                Debug.LogError($"Error initializing Chessboard: {e.Message}\n{e.StackTrace}");
            }
        }

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
            // This is a simplified implementation that always returns false for now
            // In a full implementation, we would:
            // 1. Make a copy of the current board state
            // 2. Apply the move to the copy
            // 3. Find the king of the current player
            // 4. Check if any opponent piece can capture the king
            
            // For now, we'll just log that we're checking and return false
            Debug.Log($"Checking if king would be in check after move from {move.Source.X},{move.Source.Y} to {move.Destination.X},{move.Destination.Y}");
            
            // In a real implementation, we would do something like:
            /*
            // Save the current state
            Piece capturedPiece = move.CapturedPiece;
            
            // Temporarily apply the move
            fieldMap[move.Destination] = move.MovedPiece;
            fieldMap[move.Source] = null;
            
            // Find the king of the current player
            Field kingField = null;
            foreach (var entry in fieldMap)
            {
                if (entry.Value is King && entry.Value.GetColor() == CurrentPlayerColor)
                {
                    kingField = entry.Key;
                    break;
                }
            }
            
            // Check if any opponent piece can capture the king
            bool kingInCheck = false;
            if (kingField != null)
            {
                foreach (var entry in fieldMap)
                {
                    if (entry.Value != null && entry.Value.GetColor() != CurrentPlayerColor)
                    {
                        // Check if this piece can move to the king's position
                        // This would require implementing a method to check if a move is valid
                        // without considering check
                        if (CanPieceMoveToPosition(entry.Value, entry.Key, kingField))
                        {
                            kingInCheck = true;
                            break;
                        }
                    }
                }
            }
            
            // Restore the original state
            fieldMap[move.Source] = move.MovedPiece;
            fieldMap[move.Destination] = capturedPiece;
            
            return kingInCheck;
            */
            
            return false; // For now, we'll just return false
        }

        // Verilen kaynaktan hedefe doğru boş mu?
        public List<Field> GetPath(Field source, Field destination)
        {
            List<Field> path = new List<Field>();
            
            // Calculate the direction vector from source to destination
            double dx = destination.X - source.X;
            double dy = destination.Y - source.Y;
            
            // Calculate the number of steps needed
            double distance = Math.Max(Math.Abs(dx), Math.Abs(dy));
            int steps = (int)Math.Ceiling(distance * 10); // Multiply by 10 for more precision
            
            // Normalize the direction vector
            double stepX = dx / steps;
            double stepY = dy / steps;
            
            // Generate intermediate points along the path
            for (int i = 1; i < steps; i++) // Skip source and destination
            {
                double x = source.X + stepX * i;
                double y = source.Y + stepY * i;
                
                // Find the closest field to this point
                Field closestField = null;
                double closestDistance = double.MaxValue;
                
                // Check with the Board class to find the closest field
                Board board = UnityEngine.Object.FindFirstObjectByType<Board>();
                if (board != null)
                {
                    List<string> allPositions = board.GetAllPositions();
                    foreach (string position in allPositions)
                    {
                        Vector2 pos = board.GetPosition(position);
                        double distance2 = Math.Pow(pos.x - x, 2) + Math.Pow(pos.y - y, 2);
                        
                        if (distance2 < closestDistance)
                        {
                            closestDistance = distance2;
                            closestField = board.GetField(position);
                        }
                    }
                    
                    // If we found a field and it's not the source or destination, add it to the path
                    if (closestField != null && 
                        !(Math.Abs(closestField.X - source.X) < 0.01 && Math.Abs(closestField.Y - source.Y) < 0.01) &&
                        !(Math.Abs(closestField.X - destination.X) < 0.01 && Math.Abs(closestField.Y - destination.Y) < 0.01))
                    {
                        // Check if this field is already in the path
                        bool alreadyInPath = false;
                        foreach (Field f in path)
                        {
                            if (Math.Abs(f.X - closestField.X) < 0.01 && Math.Abs(f.Y - closestField.Y) < 0.01)
                            {
                                alreadyInPath = true;
                                break;
                            }
                        }
                        
                        if (!alreadyInPath)
                        {
                            path.Add(closestField);
                        }
                    }
                }
            }
            
            return path;
        }

        // Tahta sınırları içinde mi?
        public bool IsWithinBounds(Field field)
        {
            // For hexagonal board, we need to check if the field exists in our positions dictionary
            // rather than using simple rectangular bounds
            foreach (var existingField in fieldMap.Keys)
            {
                if (Math.Abs(existingField.X - field.X) < 0.01 && Math.Abs(existingField.Y - field.Y) < 0.01)
                {
                    return true;
                }
            }
            
            // If we didn't find a matching field, check with the Board class
            Board board = UnityEngine.Object.FindFirstObjectByType<Board>();
            if (board != null)
            {
                // Try to get all positions and check if any matches our field coordinates
                List<string> allPositions = board.GetAllPositions();
                foreach (string position in allPositions)
                {
                    Vector2 pos = board.GetPosition(position);
                    if (Math.Abs(pos.x - field.X) < 0.01 && Math.Abs(pos.y - field.Y) < 0.01)
                    {
                        return true;
                    }
                }
            }
            
            return false; // Field not found in valid positions
        }


    }
}
