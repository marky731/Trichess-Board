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

            // Standard diagonals
            // Northwest/Southeast diagonal
            if ((col2 - col1) == -(row2 - row1)) return true;

            // Northeast/Southwest diagonal
            if ((col2 - col1) == (row2 - row1)) return true;

            // Get column and row differences
            int colDiff = Math.Abs(col2 - col1);
            int rowDiff = Math.Abs(row2 - row1);
            
            // Determine which regions the positions are in
            bool col1InRegionA = col1 >= 'A' && col1 <= 'D';
            bool col1InRegionB = col1 >= 'E' && col1 <= 'H';
            bool col1InRegionC = col1 >= 'I' && col1 <= 'L';
            
            bool col2InRegionA = col2 >= 'A' && col2 <= 'D';
            bool col2InRegionB = col2 >= 'E' && col2 <= 'H';
            bool col2InRegionC = col2 >= 'I' && col2 <= 'L';
            
            // Determine row regions
            bool row1InLower = row1 >= 1 && row1 <= 4;
            bool row1InMiddle = row1 >= 5 && row1 <= 8;
            bool row1InUpper = row1 >= 9 && row1 <= 12;
            
            bool row2InLower = row2 >= 1 && row2 <= 4;
            bool row2InMiddle = row2 >= 5 && row2 <= 8;
            bool row2InUpper = row2 >= 9 && row2 <= 12;
            
            // Region A to Region C diagonal lines (A-D to I-L)
            if ((col1InRegionA && col2InRegionC) || (col1InRegionC && col2InRegionA))
            {
                // A1 to L8 diagonal and similar paths
                // These diagonals cross the missing coordinates (E5, F6, G7, H8)
                
                // Normalize the positions so that firstCol is in Region A and secondCol is in Region C
                char firstCol, secondCol;
                int firstRow, secondRow;
                
                if (col1InRegionA)
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
                
                // Map columns to numerical values (A=0, B=1, etc.)
                int firstColVal = firstCol - 'A';
                int secondColVal = secondCol - 'A';
                
                // Check for the A1->L8 pattern and similar diagonals
                if (firstRow == firstColVal + 1 && secondRow == (secondColVal - 8) + 5)
                {
                    return true;
                }
                
                // Check for other diagonal patterns between Region A and Region C
                
                // Pattern: 6 columns change, 2 rows change (C5 to I7)
                if (colDiff == 6 && rowDiff == 2)
                {
                    return true;
                }
                
                // Pattern: 7 columns change, 3 rows change
                if (colDiff == 7 && rowDiff == 3)
                {
                    return true;
                }
                
                // Pattern: 8 columns change, 4 rows change
                if (colDiff == 8 && rowDiff == 4)
                {
                    return true;
                }
                
                // Pattern: 8 columns change, 3 rows change (D7 to L10)
                if (colDiff == 8 && rowDiff == 3)
                {
                    return true;
                }
            }
            
            // Region A to Region B diagonal lines (A-D to E-H)
            if ((col1InRegionA && col2InRegionB) || (col1InRegionB && col2InRegionA))
            {
                // Normalize the positions so that firstCol is in Region A and secondCol is in Region B
                char firstCol, secondCol;
                int firstRow, secondRow;
                
                if (col1InRegionA)
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
                
                // Standard diagonal check
                if (colDiff == rowDiff || colDiff == -rowDiff) return true;
                
                // Special diagonal from lower A-D to upper E-H
                if (firstRow >= 1 && firstRow <= 4 && secondRow >= 9 && secondRow <= 12)
                {
                    return true;
                }
                
                // Pattern: A6 to C4 and similar moves (2 columns change, 2 rows change)
                if (colDiff == 2 && rowDiff == 2)
                {
                    return true;
                }
                
                // Pattern: 4 columns change, 8 rows change
                if (colDiff == 4 && rowDiff == 8)
                {
                    return true;
                }
            }
            
            // Region B to Region C diagonal lines (E-H to I-L)
            if ((col1InRegionB && col2InRegionC) || (col1InRegionC && col2InRegionB))
            {
                // Normalize the positions so that firstCol is in Region B and secondCol is in Region C
                char firstCol, secondCol;
                int firstRow, secondRow;
                
                if (col1InRegionB)
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
                
                // Standard diagonal check
                if (colDiff == rowDiff || colDiff == -rowDiff) return true;
                
                // Special diagonal from lower E-H to upper I-L
                if (firstRow >= 1 && firstRow <= 4 && secondRow >= 9 && secondRow <= 12)
                {
                    int expectedRowDiff = colDiff + 4; // Adjusted based on observed patterns
                    if (rowDiff == expectedRowDiff) return true;
                }
                
                // Special diagonal from upper E-H to middle I-L
                if (firstRow >= 9 && firstRow <= 12 && secondRow >= 5 && secondRow <= 8)
                {
                    int expectedRowDiff = colDiff - 4; // Adjusted based on observed patterns
                    if (Math.Abs(rowDiff) == Math.Abs(expectedRowDiff)) return true;
                }
                
                // Pattern: F4 to H10 and similar moves (2 columns change, 6 rows change)
                if (colDiff == 2 && rowDiff == 6)
                {
                    return true;
                }
                
                // Pattern: J6 to E9 and similar moves (5 columns change, 3 rows change)
                if (colDiff == 5 && rowDiff == 3)
                {
                    return true;
                }
                
                // Pattern: F12 to K5 (5 columns change, 7 rows change)
                if (colDiff == 5 && rowDiff == 7)
                {
                    return true;
                }
                
                // Pattern: L10 to I6 (3 columns change, 4 rows change)
                if (colDiff == 3 && rowDiff == 4)
                {
                    return true;
                }
                
                // Pattern: J12 to F9 (4 columns change, 3 rows change)
                if (colDiff == 4 && rowDiff == 3)
                {
                    return true;
                }
                
                // Pattern: G9 to H4 or F9 to G4 (1 column change, 5 rows change)
                if (colDiff == 1 && rowDiff == 5)
                {
                    return true;
                }
            }
            
            // Diagonal lines within Region C (I-L)
            if (col1InRegionC && col2InRegionC)
            {
                // Standard diagonal check
                if (colDiff == rowDiff || colDiff == -rowDiff) return true;
                
                // Pattern: L7 to J5 and similar moves (3 columns change, 2 rows change)
                if (colDiff == 3 && rowDiff == 2)
                {
                    return true;
                }
                
                // Pattern: K9 to J5 and similar moves (1 column change, 4 rows change)
                if (colDiff == 1 && rowDiff == 4)
                {
                    return true;
                }
                
                // Pattern: 2 columns change, 3 rows change
                if (colDiff == 2 && rowDiff == 3)
                {
                    return true;
                }
            }
            
            // Diagonal lines within Region B (E-H)
            if (col1InRegionB && col2InRegionB)
            {
                // Standard diagonal check
                if (colDiff == rowDiff || colDiff == -rowDiff) return true;
                
                // Pattern: H10 to F4 (2 columns change, 6 rows change)
                if (colDiff == 2 && rowDiff == 6)
                {
                    return true;
                }
                
                // Pattern: 1 column change, 5 rows change
                if (colDiff == 1 && rowDiff == 5)
                {
                    return true;
                }
            }
            
            // Additional patterns observed from the images
            
            // Pattern: 4 columns change, 2 rows change
            if (colDiff == 4 && rowDiff == 2)
            {
                return true;
            }
            
            // Pattern: 3 columns change, 6 rows change
            if (colDiff == 3 && rowDiff == 6)
            {
                return true;
            }
            
            // Pattern: 2 columns change, 4 rows change
            if (colDiff == 2 && rowDiff == 4)
            {
                return true;
            }
            
            // Pattern: 4 columns change, 3 rows change
            if (colDiff == 4 && rowDiff == 3)
            {
                return true;
            }
            
            // Pattern: 3 columns change, 4 rows change
            if (colDiff == 3 && rowDiff == 4)
            {
                return true;
            }
            
            // Pattern: 5 columns change, 7 rows change
            if (colDiff == 5 && rowDiff == 7)
            {
                return true;
            }
            
            // Pattern: 1 column change, 5 rows change
            if (colDiff == 1 && rowDiff == 5)
            {
                return true;
            }
            
            // Pattern: 3 columns change, 5 rows change (L10 to J5)
            if (colDiff == 3 && rowDiff == 5)
            {
                return true;
            }
            
            // Pattern: 6 columns change, 3 rows change (J9 to D6)
            if (colDiff == 6 && rowDiff == 3)
            {
                return true;
            }
            
            // Pattern: 3 columns change, 8 rows change (J12 to G4)
            if (colDiff == 3 && rowDiff == 8)
            {
                return true;
            }
            
            // Pattern: 4 columns change, 1 row change (E9 to I10, I10 to E9)
            if (colDiff == 4 && rowDiff == 1)
            {
                return true;
            }
            
            // Pattern: 1 column change, 1 row change (E9 to F10, I10 to J9)
            if (colDiff == 1 && rowDiff == 1)
            {
                return true;
            }
            
            // Pattern: 3 columns change, 7 rows change (H10 to E3)
            if (colDiff == 3 && rowDiff == 7)
            {
                return true;
            }
            
            // Pattern: 3 columns change, 2 rows change (F12 to I10)
            if (colDiff == 3 && rowDiff == 2)
            {
                return true;
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
