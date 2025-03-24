using System;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private Dictionary<string, (double x, double y)> positions;
    private Dictionary<string, string> player1InitialPos;
    private Dictionary<string, string> player2InitialPos;
    private Dictionary<string, string> player3InitialPos;

    public Board()
    {
        positions = new Dictionary<string, (double, double)>
        {
            // First block
            { "L8", (2.25, 3.5) }, { "K8", (2.00, 3.10) }, { "J8", (1.85, 2.75) }, { "I8", (1.70, 2.25) },
            { "L7", (2.70, 3.45) }, { "K7", (2.57, 2.97) }, { "J7", (2.45, 2.45) }, { "I7", (2.30, 1.95) },
            { "L6", (3.15, 3.40) }, { "K6", (3.07, 2.84) }, { "J6", (3.05, 2.25) }, { "I6", (2.90, 1.65) },
            { "L5", (3.60, 3.35) }, { "K5", (3.57, 2.72) }, { "J5", (3.55, 2.05) }, { "I5", (3.50, 1.35) },

            // Second block
            { "L9", (4.05, 3.35) }, { "K9", (4.07, 3.10) }, { "J9", (4.05, 2.05) }, { "I9", (4.10, 1.35) },
            { "L10", (4.5, 3.40) }, { "K10", (4.57, 2.97) }, { "J10", (4.55, 2.25) }, { "I10", (4.70, 1.65) },
            { "L11", (4.95, 3.45) }, { "K11", (5.07, 2.84) }, { "J11", (5.05, 2.45) }, { "I11", (5.30, 1.95) },
            { "L12", (5.30, 3.50) }, { "K12", (5.57, 2.72) }, { "J12", (5.55, 2.75) }, { "I12", (5.90, 2.25) },

            // Third block
            { "D8", (1.45, 1.85) }, { "C8", (1.25, 1.50) }, { "B8", (0.95, 1.15) }, { "A8", (0.67, 0.74) },
            { "D7", (2.05, 1.50) }, { "C7", (1.70, 1.15) }, { "B7", (1.30, 0.80) }, { "A7", (0.93, 0.39) },
            { "D6", (2.65, 1.15) }, { "C6", (2.15, 0.80) }, { "B6", (1.65, 0.43) }, { "A6", (1.20, 0.07) },
            { "D5", (3.25, 0.80) }, { "C5", (2.60, 0.45) }, { "B5", (2.00, 0.00) }, { "A5", (1.46, -0.38) },

            // Fourth block
            { "E9", (4.46, 0.80) }, { "F9", (5.05, 0.40) }, { "G9", (5.61, 0.00) }, { "H9", (6.19, -0.38) },
            { "E10", (5.00, 1.15) }, { "F10", (5.50, 0.73) }, { "G10", (5.97, 0.39) }, { "H10", (6.48, -0.02) },
            { "E11", (5.61, 1.50) }, { "F11", (6.00, 1.15) }, { "G11", (6.37, 0.73) }, { "H11", (6.73, 0.37) },
            { "E12", (6.20, 1.85) }, { "F12", (6.50, 1.50) }, { "G12", (6.74, 1.13) }, { "H12", (7.00, 0.74) },

            // Fifth block
            { "H1", (5.45, -2.02) }, { "G1", (4.97, -1.98) }, { "F1", (4.53, -1.95) }, { "E1", (4.05, -1.88) },
            { "H2", (5.62, -1.62) }, { "G2", (5.09, -1.48) }, { "F2", (4.59, -1.36) }, { "E2", (4.06, -1.20) },
            { "H3", (5.78, -1.21) }, { "G3", (5.23, -0.98) }, { "F3", (4.68, -0.76) }, { "E3", (4.10, -0.50) },
            { "H4", (5.99, -0.75) }, { "G4", (5.36, -0.45) }, { "F4", (4.77, -0.13) }, { "E4", (4.13, 0.20) },

            // Sixth block
            { "A4", (1.70, -0.77) }, { "B4", (2.32, -0.42) }, { "C4", (2.93, -0.13) }, { "D4", (3.50, 0.20) },
            { "A3", (1.88, -1.23) }, { "B3", (2.45, -0.92) }, { "C3", (3.00, -0.67) }, { "D3", (3.56, -0.47) },
            { "A2", (2.08, -1.58) }, { "B2", (2.57, -1.42) }, { "C2", (3.07, -1.34) }, { "D2", (3.60, -1.15) },
            { "A1", (2.26, -1.99) }, { "B1", (2.70, -1.92) }, { "C1", (3.17, -1.90) }, { "D1", (3.63, -1.88) }
        };

        player1InitialPos = new Dictionary<string, string>
        {
            { "rook1", "L8" }, { "knight1", "K8" }, { "bishop1", "J8" }, { "queen", "I8" },
            { "king", "D8" }, { "bishop2", "C8" }, { "knight2", "B8" }, { "rook2", "A8" },
            { "pawn1", "L7" }, { "pawn2", "K7" }, { "pawn3", "J7" }, { "pawn4", "I7" },
            { "pawn5", "D7" }, { "pawn6", "C7" }, { "pawn7", "B7" }, { "pawn8", "A7" }
        };

        player2InitialPos = new Dictionary<string, string>
        {
            { "rook1", "A1" }, { "knight1", "B1" }, { "bishop1", "C1" }, { "queen", "D1" },
            { "king", "E1" }, { "bishop2", "F1" }, { "knight2", "G1" }, { "rook2", "H1" },
            { "pawn1", "A2" }, { "pawn2", "B2" }, { "pawn3", "C2" }, { "pawn4", "D2" },
            { "pawn5", "E2" }, { "pawn6", "F2" }, { "pawn7", "G2" }, { "pawn8", "H2" }
        };

        player3InitialPos = new Dictionary<string, string>
        {
            { "rook1", "H12" }, { "knight1", "G12" }, { "bishop1", "F12" }, { "queen", "E12" },
            { "king", "I11" }, { "bishop2", "J12" }, { "knight2", "K12" }, { "rook2", "L12" },
            { "pawn1", "H11" }, { "pawn2", "G11" }, { "pawn3", "F11" }, { "pawn4", "E10" },
            { "pawn5", "I11" }, { "pawn6", "J11" }, { "pawn7", "K11" }, { "pawn8", "L11" }
        };
    }

    public (double x, double y)? GetPosition(string cell)
    {
        return positions.TryGetValue(cell, out var position) ? position : null;
    }

    public void PrintBoard()
    {
        foreach (var kvp in positions)
        {
            Console.WriteLine($"{kvp.Key}: ({kvp.Value.x}, {kvp.Value.y})");
        }
    }
}
