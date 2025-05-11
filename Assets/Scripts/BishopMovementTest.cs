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
