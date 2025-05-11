# Rook Movement Rules in Trichess Board

This document explains the movement rules for rooks in the Trichess Board project.

## Basic Rule

Rooks move in straight lines, where either:
- The column changes but the row stays the same (horizontal movement)
- The row changes but the column stays the same (vertical movement)

In other words, only one of column or row should change. If both the column and row change, it's not a valid rook move (with some special exceptions).

## Special Cases (Considered as Straight Lines)

In the hexagonal chess board, there are three special cases that are considered straight lines for rooks:

1. **I(5,6,7,8) to E(1,2,3,4)**
   - Rooks can move from I5, I6, I7, or I8 to E1, E2, E3, or E4
   - Rooks can also move in the opposite direction, from E1, E2, E3, or E4 to I5, I6, I7, or I8

2. **I(9,10,11,12) to D(1,2,3,4)**
   - Rooks can move from I9, I10, I11, or I12 to D1, D2, D3, or D4
   - Rooks can also move in the opposite direction, from D1, D2, D3, or D4 to I9, I10, I11, or I12

3. **D(5,6,7,8) to E(9,10,11,12)**
   - Rooks can move from D5, D6, D7, or D8 to E9, E10, E11, or E12
   - Rooks can also move in the opposite direction, from E9, E10, E11, or E12 to D5, D6, D7, or D8

## Implementation

The rook movement rules are implemented in the `IsInStraightLine` method in the `FieldDirectionMap` class. This method checks if two positions are in a straight line according to the rules above.

```csharp
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
```

Note that each special case checks both directions of movement, ensuring that rooks can move in both directions along these special lines.
