# Bishop Movement Rules in Trichess Board

This document explains the movement rules for bishops in the Trichess Board project.

## Basic Rule

Bishops move in diagonal lines, where both the column and row change by the same amount. In other words, the delta between the column and row should match.

Examples of standard diagonal moves:
- a1 to c3 (column and row both increase by 2)
- e9 to h12 (column and row both increase by 3)
- I5 to L8 (column and row both increase by 3)

## Special Cases

In the hexagonal chess board, there are special cases for diagonal movement due to the unique board structure:

1. **Missing Coordinates**: There are no coordinates E5, F6, G7, H8 on the board. This creates special diagonal paths.

2. **A1 to L8 Diagonal**: This is a special diagonal line that crosses the missing coordinates. The column difference (11) is greater than the row difference (7) because of the missing coordinates.

3. **Region-Specific Diagonals**: The board can be divided into regions (A-D, E-H, I-L), and diagonal movement between these regions follows special patterns.

## Implementation

The bishop movement rules are implemented in the `IsInDiagonalLine` method in the `FieldDirectionMap` class. This method checks if two positions are in a diagonal line according to the rules above.

```csharp
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
```

## Testing

The bishop movement implementation can be tested using the `BishopMovementTest` script. This script tests various diagonal moves, including the special cases mentioned above.

To run the tests:
1. Open the BishopMovementTestScene
2. Play the scene
3. Check the console for test results

The test cases include:
- A1 to C3 (standard diagonal)
- E9 to H12 (standard diagonal)
- I5 to L8 (standard diagonal)
- A1 to L8 (special case diagonal)
- And several other test cases to verify the implementation
