using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Model.ChessboardMain
{
    /// <summary>
    /// Stores valid moves for each piece type and position on the hexagonal chess board.
    /// </summary>
    public class FieldDirectionMap
    {
        // Singleton instance
        private static FieldDirectionMap _instance;
        public static FieldDirectionMap Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FieldDirectionMap();
                }
                return _instance;
            }
        }

        // Maps for regular moves (non-capturing)
        private Dictionary<int, Dictionary<string, List<string>>> _pawnMoves;
        private Dictionary<string, List<string>> _rookMoves;
        private Dictionary<string, List<string>> _bishopMoves;
        private Dictionary<string, List<string>> _knightMoves;
        private Dictionary<string, List<string>> _queenMoves;
        private Dictionary<string, List<string>> _kingMoves;

        // Maps for capture moves (only different for pawns)
        private Dictionary<int, Dictionary<string, List<string>>> _pawnCaptures;

        // Reference to the board for position validation
        private Board _board;

        private FieldDirectionMap()
        {
            // Initialize the dictionaries
            _pawnMoves = new Dictionary<int, Dictionary<string, List<string>>>();
            _pawnCaptures = new Dictionary<int, Dictionary<string, List<string>>>();
            _rookMoves = new Dictionary<string, List<string>>();
            _bishopMoves = new Dictionary<string, List<string>>();
            _knightMoves = new Dictionary<string, List<string>>();
            _queenMoves = new Dictionary<string, List<string>>();
            _kingMoves = new Dictionary<string, List<string>>();

            // Initialize player-specific dictionaries
            for (int playerId = 1; playerId <= 3; playerId++)
            {
                _pawnMoves[playerId] = new Dictionary<string, List<string>>();
                _pawnCaptures[playerId] = new Dictionary<string, List<string>>();
            }
        }

        public void Initialize(Board board)
        {
            _board = board;
            GenerateAllMoves();
        }

        private void GenerateAllMoves()
        {
            if (_board == null)
            {
                Debug.LogError("Board reference is null in FieldDirectionMap. Cannot generate moves.");
                return;
            }

            List<string> allPositions = _board.GetAllPositions();
            if (allPositions == null || allPositions.Count == 0)
            {
                Debug.LogError("No positions found in Board. Cannot generate moves.");
                return;
            }

            Debug.Log($"Generating moves for {allPositions.Count} positions...");

            // Generate moves for each position
            foreach (string position in allPositions)
            {
                GeneratePawnMoves(position);
                GenerateRookMoves(position);
                GenerateBishopMoves(position);
                GenerateKnightMoves(position);
                GenerateQueenMoves(position);
                GenerateKingMoves(position);
            }

            Debug.Log("Move generation complete.");
        }

        /// <summary>
        /// Generate valid pawn moves for a given position.
        /// </summary>
        private void GeneratePawnMoves(string position)
        {
            // White pawns (Player 1)
            GenerateWhitePawnMoves(position);

            // Gray pawns (Player 2)
            GenerateGrayPawnMoves(position);

            // Black pawns (Player 3)
            GenerateBlackPawnMoves(position);
        }

        private void GenerateWhitePawnMoves(string position)
        {
            // Parse the position
            if (position.Length < 2) return;
            char col = position[0];
            int row;
            if (!int.TryParse(position.Substring(1), out row)) return;

            List<string> moves = new List<string>();
            List<string> captures = new List<string>();

            // Special case exceptions
            if (position == "D5")
            {
                // Special case: D5 can go to E9
                moves.Add("E9");
                Debug.Log("Added special move: D5 -> E9");
            }
            
            if (position == "I5")
            {
                // Special case: I5 can go to E4
                moves.Add("E4");
                Debug.Log("Added special move: I5 -> E4");
            }

            // Special case for I, J, K, L columns at row 5
            if ((col == 'I' || col == 'J' || col == 'K' || col == 'L') && row == 5)
            {
                // They can go to number 9 from 5, keeping the same letter
                string jumpMove = $"{col}9";
                moves.Add(jumpMove);
            }
            // For I, J, K, L columns after row 9, they start increasing
            else if ((col == 'I' || col == 'J' || col == 'K' || col == 'L') && row >= 9)
            {
                // Move forward by increasing the row number
                string forwardMove = $"{col}{row + 1}";
                moves.Add(forwardMove);
            }
            else
            {
                // Regular move: same letter, decrease number
                string forwardMove = $"{col}{row - 1}";
                moves.Add(forwardMove);
            }

            if (row == 7)
            {
                string forwardMove = $"{col}5";
                moves.Add(forwardMove);
            }

            // Store the moves
            _pawnMoves[1][position] = moves;
            _pawnCaptures[1][position] = captures; // No captures for now
        }

        private void GenerateGrayPawnMoves(string position)
        {
            // Parse the position
            if (position.Length < 2) return;
            char col = position[0];
            int row;
            if (!int.TryParse(position.Substring(1), out row)) return;

            List<string> moves = new List<string>();
            List<string> captures = new List<string>();

            // Special case exceptions
            if (position == "D4")
            {
                // Special case: D4 can go to I9
                moves.Add("I9");
            }
            
            if (position == "E4")
            {
                // Special case: E4 can go to I5
                moves.Add("I5");
            }

            if (row == 4)
            {
                moves.Add(col + "9");
            }

            // Regular move: same letter, increase number
            string forwardMove = $"{col}{row + 1}";
            moves.Add(forwardMove);

            if (row == 2)
            {
                forwardMove = $"{col}4";
                moves.Add(forwardMove);
            }

            // Store the moves
            _pawnMoves[2][position] = moves;
            _pawnCaptures[2][position] = captures; // No captures for now
        }


        private void GenerateBlackPawnMoves(string position)
        {
            // Parse the position
            if (position.Length < 2) return;
            char col = position[0];
            int row;
            if (!int.TryParse(position.Substring(1), out row)) return;

            List<string> moves = new List<string>();
            List<string> captures = new List<string>();

            // Special case exceptions
            if (position == "I9")
            {
                // Special case: I9 can go to D4
                moves.Add("D4");
            }
            if (position == "E9")
            {
                // Special case: E9 can go to D5
                moves.Add("D5");
            }
            // Special case for I, J, K, L, E, F, G, H columns at row 9
            if ((col >= 'E' && col <= 'H' || col >= 'I' && col <= 'L') && row == 9)
            {
                // For I, J, K, L columns, they can go to number 4 for E, F, G, H
                if (col >= 'I' && col <= 'L')
                {
                    string jumpMove = $"{col}4";
                    moves.Add(jumpMove);
                }

                // For E, F, G, H columns, they can go to number 4 for I, J, K, L
                if (col >= 'E' && col <= 'H')
                {
                        string jumpMove = $"{col}4";
                        moves.Add(jumpMove);
                }

                // They can also go to number 5 for the same letter
                string sameColMove = $"{col}5";
                moves.Add(sameColMove);
            }
            else
            {
                string forwardMove = $"{col}{row + 1}";

                if (row >= 5 && row <= 8)
                {
                    // Regular move: same letter, increase number
                    forwardMove = $"{col}{row + 1}";
                    moves.Add(forwardMove);
                }
                else
                {
                    // Regular move: same letter, decrease number
                    forwardMove = $"{col}{row - 1}";
                    moves.Add(forwardMove);
                }

            }

            if (row == 11)
            {
                string forwardMove = $"{col}9";
                moves.Add(forwardMove);
            }

            // Store the moves
            _pawnMoves[3][position] = moves;
            _pawnCaptures[3][position] = captures; // No captures for now
        }

        private void GenerateRookMoves(string position)
        {
            // Rooks move in straight lines (horizontally, vertically, and one diagonal in hexagonal chess)
            List<string> moves = new List<string>();

            // Get all positions
            List<string> allPositions = _board.GetAllPositions();

            // For each position, check if it's in a straight line from the current position
            foreach (string targetPosition in allPositions)
            {
                if (position == targetPosition) continue; // Skip the current position

                if (IsInStraightLine(position, targetPosition))
                {
                    moves.Add(targetPosition);
                }
            }

            // Store the moves
            _rookMoves[position] = moves;
        }

        private void GenerateBishopMoves(string position)
        {
            // Bishops move in diagonal lines (two of the three diagonals in hexagonal chess)
            List<string> moves = new List<string>();

            // Get all positions
            List<string> allPositions = _board.GetAllPositions();

            // For each position, check if it's in a diagonal line from the current position
            foreach (string targetPosition in allPositions)
            {
                if (position == targetPosition) continue; // Skip the current position

                if (IsInDiagonalLine(position, targetPosition))
                {
                    moves.Add(targetPosition);
                }
            }

            // Store the moves
            _bishopMoves[position] = moves;
        }

        private void GenerateKnightMoves(string position)
        {
            // Knights move in an L-shape (2 steps in one direction, then 1 step perpendicular)
            List<string> moves = new List<string>();

            // Parse the position
            if (position.Length < 2) return;
            char col = position[0];
            int row;
            if (!int.TryParse(position.Substring(1), out row)) return;

            // Define all possible knight moves in a hexagonal grid
            string[] possibleMoves = new string[]
            {
                // 2 steps horizontally, 1 step vertically
                $"{(char)(col + 2)}{row + 1}",
                $"{(char)(col + 2)}{row - 1}",
                $"{(char)(col - 2)}{row + 1}",
                $"{(char)(col - 2)}{row - 1}",

                // 1 step horizontally, 2 steps vertically
                $"{(char)(col + 1)}{row + 2}",
                $"{(char)(col + 1)}{row - 2}",
                $"{(char)(col - 1)}{row + 2}",
                $"{(char)(col - 1)}{row - 2}",

                // Hexagonal-specific moves
                $"{(char)(col + 2)}{row + 2}",
                $"{(char)(col + 2)}{row - 2}",
                $"{(char)(col - 2)}{row + 2}",
                $"{(char)(col - 2)}{row - 2}",
            };

            // Add valid moves
            foreach (string move in possibleMoves)
            {
                if (IsValidPosition(move))
                {
                    moves.Add(move);
                }
            }

            // Store the moves
            _knightMoves[position] = moves;
        }
        private void GenerateQueenMoves(string position)
        {
            // Queens can move like rooks or bishops
            List<string> moves = new List<string>();

            // Get all positions
            List<string> allPositions = _board.GetAllPositions();

            // For each position, check if it's in a straight or diagonal line from the current position
            foreach (string targetPosition in allPositions)
            {
                if (position == targetPosition) continue; // Skip the current position

                if (IsInStraightLine(position, targetPosition) || IsInDiagonalLine(position, targetPosition))
                {
                    moves.Add(targetPosition);
                }
            }

            // Store the moves
            _queenMoves[position] = moves;
        }

        private void GenerateKingMoves(string position)
        {
            // Kings move one step in any direction
            List<string> moves = new List<string>();

            // Parse the position
            if (position.Length < 2) return;
            char col = position[0];
            int row;
            if (!int.TryParse(position.Substring(1), out row)) return;

            // Define all possible king moves
            string[] possibleMoves = new string[]
            {
                // Horizontal and vertical moves
                $"{(char)(col + 1)}{row}",
                $"{(char)(col - 1)}{row}",
                $"{col}{row + 1}",
                $"{col}{row - 1}",

                // Diagonal moves
                $"{(char)(col + 1)}{row + 1}",
                $"{(char)(col + 1)}{row - 1}",
                $"{(char)(col - 1)}{row + 1}",
                $"{(char)(col - 1)}{row - 1}",
            };

            // Add valid moves
            foreach (string move in possibleMoves)
            {
                if (IsValidPosition(move))
                {
                    moves.Add(move);
                }
            }

            // Store the moves
            _kingMoves[position] = moves;
        }

        /// <summary>
        /// Check if a position is valid on the board.
        /// </summary>
        private bool IsValidPosition(string position)
        {
            if (_board == null) return false;
            return _board.GetAllPositions().Contains(position);
        }

        /// <summary>
        /// Check if two positions are in a straight line (horizontal, vertical, or special cases in hexagonal chess).
        /// </summary>
        private bool IsInStraightLine(string position1, string position2)
        {
            // Parse the positions
            if (position1.Length < 2 || position2.Length < 2) return false;
            char col1 = position1[0];
            char col2 = position2[0];
            int row1, row2;
            if (!int.TryParse(position1.Substring(1), out row1) || !int.TryParse(position2.Substring(1), out row2)) return false;

            // Same column
            if (col1 == col2) return true;

            // Same row
            if (row1 == row2) return true;

            // Special case 1: I(5,6,7,8) to E(1,2,3,4)
            if ((col1 == 'I' && col2 == 'E' && row1 >= 5 && row1 <= 8 && row2 >= 1 && row2 <= 4) ||
                (col1 == 'E' && col2 == 'I' && row1 >= 1 && row1 <= 4 && row2 >= 5 && row2 <= 8))
            {
                return true;
            }

            // Special case 2: I(9,10,11,12) to D(1,2,3,4)
            if ((col1 == 'I' && col2 == 'D' && row1 >= 9 && row1 <= 12 && row2 >= 1 && row2 <= 4) ||
                (col1 == 'D' && col2 == 'I' && row1 >= 1 && row1 <= 4 && row2 >= 9 && row2 <= 12))
            {
                return true;
            }

            // Special case 3: D(5,6,7,8) to E(9,10,11,12)
            if ((col1 == 'D' && col2 == 'E' && row1 >= 5 && row1 <= 8 && row2 >= 9 && row2 <= 12) ||
                (col1 == 'E' && col2 == 'D' && row1 >= 9 && row1 <= 12 && row2 >= 5 && row2 <= 8))
            {
                return true;
            }

            // No other diagonal movements are allowed for rooks
            return false;
        }

        /// <summary>
        /// Check if two positions are in a diagonal line (two of the three diagonals in hexagonal chess).
        /// </summary>
        private bool IsInDiagonalLine(string position1, string position2)
        {
            // Parse the positions
            if (position1.Length < 2 || position2.Length < 2) return false;
            char col1 = position1[0];
            char col2 = position2[0];
            int row1, row2;
            if (!int.TryParse(position1.Substring(1), out row1) || !int.TryParse(position2.Substring(1), out row2)) return false;

            // Northwest/Southeast diagonal
            if ((col2 - col1) == -(row2 - row1)) return true;

            // Northeast/Southwest diagonal
            if ((col2 - col1) == (row2 - row1)) return true;

            // Special case: Diagonal across the missing coordinates (E5, F6, G7, H8)
            // A1 to L8 diagonal and similar paths
            if ((col1 >= 'A' && col1 <= 'D' && col2 >= 'I' && col2 <= 'L') ||
                (col1 >= 'I' && col1 <= 'L' && col2 >= 'A' && col2 <= 'D'))
            {
                // Calculate the expected row difference based on column difference
                int colDiff = Math.Abs(col2 - col1);
                int rowDiff = Math.Abs(row2 - row1);

                // For the special diagonal that crosses the missing coordinates
                // The row difference should be less than the column difference
                // because some coordinates are missing
                if (colDiff > rowDiff)
                {
                    // Check if the positions are on the same diagonal line
                    // by verifying if they follow the pattern of the special diagonal

                    // Map columns to numerical values (A=0, B=1, etc.)
                    int colVal1 = col1 - 'A';
                    int colVal2 = col2 - 'A';

                    // Calculate expected row values for the diagonal
                    int expectedRow1, expectedRow2;

                    // If moving from A-D to I-L
                    if (col1 <= 'D' && col2 >= 'I')
                    {
                        // A1->L8 pattern: row increases with column, but jumps over missing coordinates
                        expectedRow1 = colVal1 + 1; // A->1, B->2, C->3, D->4
                        expectedRow2 = (colVal2 - 8) + 5; // I->5, J->6, K->7, L->8
                    }
                    // If moving from I-L to A-D
                    else
                    {
                        // L8->A1 pattern (reverse direction)
                        expectedRow1 = (colVal1 - 8) + 5; // I->5, J->6, K->7, L->8
                        expectedRow2 = colVal2 + 1; // A->1, B->2, C->3, D->4
                    }

                    // Check if the actual rows match the expected rows for this diagonal
                    return (row1 == expectedRow1 && row2 == expectedRow2);
                }
            }

            // Special case: E-H columns to I-L columns diagonal jumps
            if ((col1 >= 'E' && col1 <= 'H' && col2 >= 'I' && col2 <= 'L') ||
                (col1 >= 'I' && col1 <= 'L' && col2 >= 'E' && col2 <= 'H'))
            {
                // Handle diagonal jumps between E-H and I-L columns
                // For example: E9 to H12 or I5 to L8

                char firstCol, secondCol;
                int firstRow, secondRow;

                // Ensure firstCol is the smaller column
                if (col1 <= col2)
                {
                    firstCol = col1;
                    firstRow = row1;
                    secondCol = col2;
                    secondRow = row2;
                }
                else
                {
                    firstCol = col2;
                    firstRow = row2;
                    secondCol = col1;
                    secondRow = row1;
                }

                // Check if the column difference matches the row difference
                int colDiff = secondCol - firstCol;
                int rowDiff = secondRow - firstRow;

                // For standard diagonals within these regions
                return colDiff == rowDiff;
            }

            return false;
        }

        /// <summary>
        /// Get valid pawn moves for a given position and player.
        /// </summary>
        public List<string> GetPawnMoves(string position, int playerId)
        {
            if (_pawnMoves.ContainsKey(playerId) && _pawnMoves[playerId].ContainsKey(position))
            {
                return _pawnMoves[playerId][position];
            }
            return new List<string>();
        }

        /// <summary>
        /// Get valid pawn capture moves for a given position and player.
        /// </summary>
        public List<string> GetPawnCaptures(string position, int playerId)
        {
            if (_pawnCaptures.ContainsKey(playerId) && _pawnCaptures[playerId].ContainsKey(position))
            {
                return _pawnCaptures[playerId][position];
            }
            return new List<string>();
        }

        /// <summary>
        /// Get valid rook moves for a given position.
        /// </summary>
        public List<string> GetRookMoves(string position)
        {
            if (_rookMoves.ContainsKey(position))
            {
                return _rookMoves[position];
            }
            return new List<string>();
        }

        /// <summary>
        /// Get valid bishop moves for a given position.
        /// </summary>
        public List<string> GetBishopMoves(string position)
        {
            if (_bishopMoves.ContainsKey(position))
            {
                return _bishopMoves[position];
            }
            return new List<string>();
        }

        /// <summary>
        /// Get valid knight moves for a given position.
        /// </summary>
        public List<string> GetKnightMoves(string position)
        {
            if (_knightMoves.ContainsKey(position))
            {
                return _knightMoves[position];
            }
            return new List<string>();
        }

        /// <summary>
        /// Get valid queen moves for a given position.
        /// </summary>
        public List<string> GetQueenMoves(string position)
        {
            if (_queenMoves.ContainsKey(position))
            {
                return _queenMoves[position];
            }
            return new List<string>();
        }

        /// <summary>
        /// Get valid king moves for a given position.
        /// </summary>
        public List<string> GetKingMoves(string position)
        {
            if (_kingMoves.ContainsKey(position))
            {
                return _kingMoves[position];
            }
            return new List<string>();
        }

        /// <summary>
        /// Check if a move is valid for a pawn.
        /// </summary>
        public bool IsValidPawnMove(string sourcePosition, string targetPosition, int playerId, bool isCapture)
        {
            if (isCapture)
            {
                List<string> captures = GetPawnCaptures(sourcePosition, playerId);
                return captures.Contains(targetPosition);
            }
            else
            {
                List<string> moves = GetPawnMoves(sourcePosition, playerId);
                return moves.Contains(targetPosition);
            }
        }

        /// <summary>
        /// Check if a move is valid for a rook.
        /// </summary>
        public bool IsValidRookMove(string sourcePosition, string targetPosition)
        {
            List<string> moves = GetRookMoves(sourcePosition);
            return moves.Contains(targetPosition);
        }

        /// <summary>
        /// Check if a move is valid for a bishop.
        /// </summary>
        public bool IsValidBishopMove(string sourcePosition, string targetPosition)
        {
            List<string> moves = GetBishopMoves(sourcePosition);
            return moves.Contains(targetPosition);
        }

        /// <summary>
        /// Check if a move is valid for a knight.
        /// </summary>
        public bool IsValidKnightMove(string sourcePosition, string targetPosition)
        {
            List<string> moves = GetKnightMoves(sourcePosition);
            return moves.Contains(targetPosition);
        }

        /// <summary>
        /// Check if a move is valid for a queen.
        /// </summary>
        public bool IsValidQueenMove(string sourcePosition, string targetPosition)
        {
            List<string> moves = GetQueenMoves(sourcePosition);
            return moves.Contains(targetPosition);
        }

        /// <summary>
        /// Check if a move is valid for a king.
        /// </summary>
        public bool IsValidKingMove(string sourcePosition, string targetPosition)
        {
            List<string> moves = GetKingMoves(sourcePosition);
            return moves.Contains(targetPosition);
        }
    }
}
