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

        /// <summary>
        /// Initialize the field direction map with the board reference.
        /// </summary>
        public void Initialize(Board board)
        {
            _board = board;
            GenerateAllMoves();
        }

        /// <summary>
        /// Generate all valid moves for all piece types and positions.
        /// </summary>
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

        /// <summary>
        /// Generate valid white pawn moves for a given position.
        /// </summary>
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
            }
            else if (position == "I5")
            {
                // Special case: I5 can go to E4
                moves.Add("E4");
            }
            else if (position == "D4")
            {
                // Special case: D4 can go to I9
                moves.Add("I9");
            }
            else if (position == "E4")
            {
                // Special case: E4 can go to I5
                moves.Add("I5");
            }
            // Special case for I, J, K, L columns at row 5
            else if ((col == 'I' || col == 'J' || col == 'K' || col == 'L') && row == 5)
            {
                // They can go to number 9 from 5, keeping the same letter
                string jumpMove = $"{col}9";
                if (IsValidPosition(jumpMove))
                {
                    moves.Add(jumpMove);
                }
            }
            // For I, J, K, L columns after row 9, they start increasing
            else if ((col == 'I' || col == 'J' || col == 'K' || col == 'L') && row >= 9)
            {
                // Move forward by increasing the row number
                string forwardMove = $"{col}{row + 1}";
                if (IsValidPosition(forwardMove))
                {
                    moves.Add(forwardMove);
                }
            }
            else
            {
                // Regular move: same letter, decrease number
                string forwardMove = $"{col}{row - 1}";
                if (IsValidPosition(forwardMove))
                {
                    moves.Add(forwardMove);
                }
            }

            // Store the moves
            _pawnMoves[1][position] = moves;
            _pawnCaptures[1][position] = captures; // No captures for now
        }

        /// <summary>
        /// Generate valid gray pawn moves for a given position.
        /// </summary>
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
            else if (position == "E4")
            {
                // Special case: E4 can go to I5
                moves.Add("I5");
            }
            else
            {
                // Regular move: same letter, increase number
                string forwardMove = $"{col}{row + 1}";
                if (IsValidPosition(forwardMove))
                {
                    moves.Add(forwardMove);
                }
            }

            // Store the moves
            _pawnMoves[2][position] = moves;
            _pawnCaptures[2][position] = captures; // No captures for now
        }

        /// <summary>
        /// Generate valid black pawn moves for a given position.
        /// </summary>
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
            else if (position == "E9")
            {
                // Special case: E9 can go to D5
                moves.Add("D5");
            }
            // Special case for I, J, K, L, E, F, G, H columns at row 9
            else if ((col >= 'E' && col <= 'H' || col >= 'I' && col <= 'L') && row == 9)
            {
                // For I, J, K, L columns, they can go to number 4 for E, F, G, H
                if (col >= 'I' && col <= 'L')
                {
                    char newCol = (char)('E' + (col - 'I')); // Map I->E, J->F, K->G, L->H
                    if (newCol >= 'E' && newCol <= 'H') // Safety check
                    {
                        string jumpMove = $"{newCol}4";
                        if (IsValidPosition(jumpMove))
                        {
                            moves.Add(jumpMove);
                        }
                    }
                }
                
                // For E, F, G, H columns, they can go to number 4 for I, J, K, L
                if (col >= 'E' && col <= 'H')
                {
                    char newCol = (char)('I' + (col - 'E')); // Map E->I, F->J, G->K, H->L
                    if (newCol >= 'I' && newCol <= 'L') // Safety check
                    {
                        string jumpMove = $"{newCol}4";
                        if (IsValidPosition(jumpMove))
                        {
                            moves.Add(jumpMove);
                        }
                    }
                }
                
                // They can also go to number 5 for the same letter
                string sameColMove = $"{col}5";
                if (IsValidPosition(sameColMove))
                {
                    moves.Add(sameColMove);
                }
            }
            else
            {
                // Regular move: same letter, decrease number
                string forwardMove = $"{col}{row - 1}";
                if (IsValidPosition(forwardMove))
                {
                    moves.Add(forwardMove);
                }
            }

            // Store the moves
            _pawnMoves[3][position] = moves;
            _pawnCaptures[3][position] = captures; // No captures for now
        }

        /// <summary>
        /// Generate valid rook moves for a given position.
        /// </summary>
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

        /// <summary>
        /// Generate valid bishop moves for a given position.
        /// </summary>
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

        /// <summary>
        /// Generate valid knight moves for a given position.
        /// </summary>
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

        /// <summary>
        /// Generate valid queen moves for a given position.
        /// </summary>
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

        /// <summary>
        /// Generate valid king moves for a given position.
        /// </summary>
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
        /// Check if two positions are in a straight line (horizontal, vertical, or one diagonal in hexagonal chess).
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

            // Third diagonal in hexagonal grid (northeast/southwest)
            if (Math.Abs(col2 - col1) == Math.Abs(row2 - row1)) return true;

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
