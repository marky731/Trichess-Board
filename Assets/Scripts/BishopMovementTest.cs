using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Model.ChessboardMain;

public class BishopMovementTest : MonoBehaviour
{
    private Board _board;
    private FieldDirectionMap _fieldDirectionMap;

    void Start()
    {
        // Initialize the board
        _board = new Board();
        
        // Initialize the field direction map
        _fieldDirectionMap = FieldDirectionMap.Instance;
        _fieldDirectionMap.Initialize(_board);
        
        // Run the tests
        TestBishopDiagonalMovement();
    }
    
    void TestBishopDiagonalMovement()
    {
        Debug.Log("Testing Bishop Diagonal Movement...");
        
        // Test cases from the examples
        TestDiagonalMove("A1", "C3", "A1 to C3");
        TestDiagonalMove("E9", "H12", "E9 to H12");
        TestDiagonalMove("I5", "L8", "I5 to L8");
        TestDiagonalMove("A1", "L8", "A1 to L8 (special case)");
        
        // Additional test cases
        TestDiagonalMove("B2", "D4", "B2 to D4");
        TestDiagonalMove("C3", "A1", "C3 to A1 (reverse)");
        TestDiagonalMove("L8", "A1", "L8 to A1 (special case reverse)");
        TestDiagonalMove("J6", "L8", "J6 to L8");
        TestDiagonalMove("I5", "K7", "I5 to K7");
        
        // Test some non-diagonal moves to ensure they're rejected
        TestNonDiagonalMove("A1", "A2", "A1 to A2 (same column)");
        TestNonDiagonalMove("A1", "B1", "A1 to B1 (same row)");
        TestNonDiagonalMove("A1", "D5", "A1 to D5 (not diagonal)");
        
        // Additional test cases based on the images
        Debug.Log("Testing Additional Bishop Diagonal Movements from Images...");
        
        // Region A-D to E-H diagonal movements
        TestDiagonalMove("A5", "E9", "A5 to E9 (A-D to E-H region)");
        TestDiagonalMove("B4", "F10", "B4 to F10 (A-D to E-H region)");
        TestDiagonalMove("C3", "G9", "C3 to G9 (A-D to E-H region)");
        
        // Region E-H to I-L diagonal movements
        TestDiagonalMove("E4", "I8", "E4 to I8 (E-H to I-L region)");
        TestDiagonalMove("F3", "J7", "F3 to J7 (E-H to I-L region)");
        TestDiagonalMove("G2", "K6", "G2 to K6 (E-H to I-L region)");
        
        // Special diagonal paths with different row-column ratios
        TestDiagonalMove("D3", "I9", "D3 to I9 (special diagonal path)");
        TestDiagonalMove("E4", "J10", "E4 to J10 (special diagonal path)");
        TestDiagonalMove("F5", "K11", "F5 to K11 (special diagonal path)");
        
        // Diagonal paths that cross the missing coordinates
        TestDiagonalMove("B2", "K7", "B2 to K7 (crossing missing coordinates)");
        TestDiagonalMove("C3", "J6", "C3 to J6 (crossing missing coordinates)");
        TestDiagonalMove("D4", "I5", "D4 to I5 (crossing missing coordinates)");
        
        // Special cases from feedback
        Debug.Log("Testing Special Cases from Feedback...");
        TestDiagonalMove("L7", "J5", "L7 to J5 (special case)");
        TestDiagonalMove("F4", "H10", "F4 to H10 (special case)");
        TestDiagonalMove("K9", "J5", "K9 to J5 (special case)");
        TestDiagonalMove("J6", "E9", "J6 to E9 (special case)");
        TestDiagonalMove("C5", "I7", "C5 to I7 (special case)");
        TestDiagonalMove("A6", "C4", "A6 to C4 (special case)");
        
        // Test the reverse directions as well
        TestDiagonalMove("J5", "L7", "J5 to L7 (reverse special case)");
        TestDiagonalMove("H10", "F4", "H10 to F4 (reverse special case)");
        TestDiagonalMove("J5", "K9", "J5 to K9 (reverse special case)");
        TestDiagonalMove("E9", "J6", "E9 to J6 (reverse special case)");
        TestDiagonalMove("I7", "C5", "I7 to C5 (reverse special case)");
        TestDiagonalMove("C4", "A6", "C4 to A6 (reverse special case)");
        
        // Test additional points along the same diagonal lines
        Debug.Log("Testing Additional Points Along Diagonal Lines...");
        
        // Points along the L7-J5 diagonal line
        TestDiagonalMove("L7", "K6", "L7 to K6 (along L7-J5 diagonal)");
        TestDiagonalMove("K6", "J5", "K6 to J5 (along L7-J5 diagonal)");
        
        // Points along the F4-H10 diagonal line
        TestDiagonalMove("F4", "G7", "F4 to G7 (along F4-H10 diagonal)");
        TestDiagonalMove("G7", "H10", "G7 to H10 (along F4-H10 diagonal)");
        
        // Points along the K9-J5 diagonal line
        TestDiagonalMove("K9", "J7", "K9 to J7 (along K9-J5 diagonal)");
        TestDiagonalMove("J7", "J5", "J7 to J5 (along K9-J5 diagonal)");
        
        // Points along the J6-E9 diagonal line
        TestDiagonalMove("J6", "H7", "J6 to H7 (along J6-E9 diagonal)");
        TestDiagonalMove("H7", "G8", "H7 to G8 (along J6-E9 diagonal)");
        TestDiagonalMove("G8", "F9", "G8 to F9 (along J6-E9 diagonal)");
        TestDiagonalMove("F9", "E9", "F9 to E9 (along J6-E9 diagonal)");
        
        // Points along the C5-I7 diagonal line
        TestDiagonalMove("C5", "D5", "C5 to D5 (along C5-I7 diagonal)");
        TestDiagonalMove("D5", "E6", "D5 to E6 (along C5-I7 diagonal)");
        TestDiagonalMove("E6", "F6", "E6 to F6 (along C5-I7 diagonal)");
        TestDiagonalMove("F6", "G7", "F6 to G7 (along C5-I7 diagonal)");
        TestDiagonalMove("G7", "H7", "G7 to H7 (along C5-I7 diagonal)");
        TestDiagonalMove("H7", "I7", "H7 to I7 (along C5-I7 diagonal)");
        
        // Test additional diagonal patterns
        Debug.Log("Testing Additional Diagonal Patterns...");
        
        // Pattern: 4 columns change, 2 rows change
        TestDiagonalMove("B3", "F5", "B3 to F5 (4 columns, 2 rows)");
        TestDiagonalMove("E6", "I8", "E6 to I8 (4 columns, 2 rows)");
        
        // Pattern: 3 columns change, 6 rows change
        TestDiagonalMove("E3", "H9", "E3 to H9 (3 columns, 6 rows)");
        TestDiagonalMove("G4", "J10", "G4 to J10 (3 columns, 6 rows)");
        
        // Pattern: 2 columns change, 4 rows change
        TestDiagonalMove("D2", "F6", "D2 to F6 (2 columns, 4 rows)");
        TestDiagonalMove("H5", "J9", "H5 to J9 (2 columns, 4 rows)");
        
        // Test cases from the latest feedback
        Debug.Log("Testing Cases from Latest Feedback...");
        TestDiagonalMove("J12", "F9", "J12 to F9 (4 columns, 3 rows)");
        TestDiagonalMove("L10", "I6", "L10 to I6 (3 columns, 4 rows)");
        TestDiagonalMove("F12", "K5", "F12 to K5 (5 columns, 7 rows)");
        TestDiagonalMove("G9", "H4", "G9 to H4 (1 column, 5 rows)");
        TestDiagonalMove("D7", "L10", "D7 to L10 (8 columns, 3 rows)");
        TestDiagonalMove("F9", "G4", "F9 to G4 (1 column, 5 rows)");
        TestDiagonalMove("H10", "F4", "H10 to F4 (2 columns, 6 rows)");
        
        // Test the reverse directions as well
        TestDiagonalMove("F9", "J12", "F9 to J12 (reverse 4 columns, 3 rows)");
        TestDiagonalMove("I6", "L10", "I6 to L10 (reverse 3 columns, 4 rows)");
        TestDiagonalMove("K5", "F12", "K5 to F12 (reverse 5 columns, 7 rows)");
        TestDiagonalMove("H4", "G9", "H4 to G9 (reverse 1 column, 5 rows)");
        TestDiagonalMove("L10", "D7", "L10 to D7 (reverse 8 columns, 3 rows)");
        TestDiagonalMove("G4", "F9", "G4 to F9 (reverse 1 column, 5 rows)");
        TestDiagonalMove("F4", "H10", "F4 to H10 (reverse 2 columns, 6 rows)");
        
        // Test additional cases from the latest feedback
        Debug.Log("Testing Additional Cases from Latest Feedback...");
        TestDiagonalMove("L10", "J5", "L10 to J5 (3 columns, 5 rows)");
        TestDiagonalMove("J9", "D6", "J9 to D6 (6 columns, 3 rows)");
        TestDiagonalMove("J12", "G4", "J12 to G4 (3 columns, 8 rows)");
        TestDiagonalMove("E9", "I10", "E9 to I10 (4 columns, 1 row)");
        TestDiagonalMove("I10", "E9", "I10 to E9 (4 columns, 1 row)");
        TestDiagonalMove("E9", "F10", "E9 to F10 (1 column, 1 row)");
        
        // Test the reverse directions as well
        TestDiagonalMove("J5", "L10", "J5 to L10 (reverse 3 columns, 5 rows)");
        TestDiagonalMove("D6", "J9", "D6 to J9 (reverse 6 columns, 3 rows)");
        TestDiagonalMove("G4", "J12", "G4 to J12 (reverse 3 columns, 8 rows)");
        
        // Test additional cases from the latest feedback
        Debug.Log("Testing More Special Cases from Latest Feedback...");
        TestDiagonalMove("H10", "E3", "H10 to E3 (3 columns, 7 rows)");
        TestDiagonalMove("F12", "I10", "F12 to I10 (3 columns, 2 rows)");
        TestDiagonalMove("I10", "J9", "I10 to J9 (1 column, 1 row)");
        
        // Test the reverse directions as well
        TestDiagonalMove("E3", "H10", "E3 to H10 (reverse 3 columns, 7 rows)");
        TestDiagonalMove("I10", "F12", "I10 to F12 (reverse 3 columns, 2 rows)");
        TestDiagonalMove("J9", "I10", "J9 to I10 (reverse 1 column, 1 row)");
    }
    
    void TestDiagonalMove(string source, string target, string description)
    {
        bool isDiagonal = IsInDiagonalLine(source, target);
        string result = isDiagonal ? "PASS" : "FAIL";
        Debug.Log($"Test {description}: {result} - IsInDiagonalLine returned {isDiagonal}");
    }
    
    void TestNonDiagonalMove(string source, string target, string description)
    {
        bool isDiagonal = IsInDiagonalLine(source, target);
        string result = !isDiagonal ? "PASS" : "FAIL";
        Debug.Log($"Test {description}: {result} - IsInDiagonalLine returned {isDiagonal}");
    }
    
    bool IsInDiagonalLine(string position1, string position2)
    {
        // We need to access the private method in FieldDirectionMap
        // For testing purposes, we'll use reflection to call the private method
        
        System.Type type = typeof(FieldDirectionMap);
        System.Reflection.MethodInfo method = type.GetMethod("IsInDiagonalLine", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (method != null)
        {
            return (bool)method.Invoke(_fieldDirectionMap, new object[] { position1, position2 });
        }
        
        Debug.LogError("Could not find IsInDiagonalLine method via reflection");
        return false;
    }
}
