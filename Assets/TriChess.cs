using Assets.Model;
using System.Collections.Generic;
using UnityEngine;
using Assets.Model.ChessboardMain.Pieces;

public class TriChess : MonoBehaviour
{
    // Keep your prefab declarations
    public GameObject boardPrefab;
    public List<GameObject> blackPawnPrefabs = new List<GameObject>();
    public List<GameObject> whitePawnPrefabs = new List<GameObject>();
    public List<GameObject> grayPawnPrefabs = new List<GameObject>();

    public GameObject whitekingPrefab;
    public GameObject graykingPrefab;
    public GameObject blackkingPrefab;

    public GameObject whitequeenPrefab;
    public GameObject grayqueenPrefab;  
    public GameObject blackqueenPrefab;

    public GameObject whiterookPrefab;
    public GameObject whiterookPrefab2;
    public GameObject grayrookPrefab;
    public GameObject grayrookPrefab2;
    public GameObject blackrookPrefab;
    public GameObject blackrookPrefab2;

    public GameObject whitebishopPrefab;
    public GameObject whitebishopPrefab2;
    public GameObject graybishopPrefab;
    public GameObject graybishopPrefab2;
    public GameObject blackbishopPrefab;
    public GameObject blackbishopPrefab2;

    public GameObject whiteknightPrefab;
    public GameObject whiteknightPrefab2;
    public GameObject grayknightPrefab;
    public GameObject grayknightPrefab2;
    public GameObject blackknightPrefab;
    public GameObject blackknightPrefab2;

    private Board board;
    // public GameObject pieceViewPrefab; // Add this to assign in the inspector


    // Lists to track all pieces
    public List<ChessPiece> whitePieces = new List<ChessPiece>();
    private List<ChessPiece> grayPieces = new List<ChessPiece>();
    private List<ChessPiece> blackPieces = new List<ChessPiece>();

    void Start()
    {
        // Create board
        GameObject boardObject = Instantiate(boardPrefab);
        board = boardObject.GetComponent<Board>();

        // Initialize PieceView
        // if (pieceViewPrefab != null)
        // {
        //     GameObject pieceViewObject = Instantiate(pieceViewPrefab);
        //     pieceView = pieceViewObject.GetComponent<PieceView>();
            
        //     if (pieceView == null)
        //     {
        //         Debug.LogError("PieceView component not found on pieceViewPrefab!");
        //         return;
        //     }
        // }
        // else
        // {
        //     Debug.LogError("pieceViewPrefab is not assigned!");
        //     return;
        // }

        if (board != null)
        {
            Debug.Log("board is not null");
            
            // Initialize all pieces
            InitializePieces();
        }
        else
        {
            Debug.LogError("Board prefab does not contain a Board component!");
        }
    }

    void InitializePieces()
    {
        // Initialize all white pawns
        foreach (GameObject prefab in whitePawnPrefabs)
        {
            GameObject pawnObject = Instantiate(prefab);
            ChessPiece pawn = pawnObject.GetComponent<ChessPiece>();
            
            if (pawn != null)
            {
                whitePieces.Add(pawn);
                List<string> possibleMoves = pawn.GetPossibleMoves(board);
                foreach (string move in possibleMoves)
                {
                    Debug.Log($"Possible move for White Pawn: {move}");
                }
            }
            else
            {
                Debug.LogError($"ChessPiece script is not attached to the prefab: {prefab.name}");
            }
        }

        // Initialize all gray pawns
        foreach (GameObject prefab in grayPawnPrefabs)
        {
            GameObject pawnObject = Instantiate(prefab);
            ChessPiece pawn = pawnObject.GetComponent<ChessPiece>();
            
            if (pawn != null)
            {
                grayPieces.Add(pawn);
                List<string> possibleMoves = pawn.GetPossibleMoves(board);
                foreach (string move in possibleMoves)
                {
                    Debug.Log($"Possible move for Gray Pawn: {move}");
                }
            }
            else
            {
                Debug.LogError($"ChessPiece script is not attached to the prefab: {prefab.name}");
            }
        }
        
        // Initialize all black pawns
        foreach (GameObject prefab in blackPawnPrefabs)
        {
            GameObject pawnObject = Instantiate(prefab);
            ChessPiece pawn = pawnObject.GetComponent<ChessPiece>();
            
            if (pawn != null)
            {
                blackPieces.Add(pawn);
                List<string> possibleMoves = pawn.GetPossibleMoves(board);
                foreach (string move in possibleMoves)
                {
                    Debug.Log($"Possible move for Black Pawn: {move}");
                }
            }
            else
            {
                Debug.LogError($"ChessPiece script is not attached to the prefab: {prefab.name}");
            }
        }
        
        // White pieces
        InstantiatePiece(whitekingPrefab, whitePieces, "White King");
        InstantiatePiece(whitequeenPrefab, whitePieces, "White Queen");
        InstantiatePiece(whiterookPrefab, whitePieces, "White Rook 1");
        InstantiatePiece(whiterookPrefab2, whitePieces, "White Rook 2");
        InstantiatePiece(whitebishopPrefab, whitePieces, "White Bishop 1");
        InstantiatePiece(whitebishopPrefab2, whitePieces, "White Bishop 2");
        InstantiatePiece(whiteknightPrefab, whitePieces, "White Knight 1");
        InstantiatePiece(whiteknightPrefab2, whitePieces, "White Knight 2");
        
        // Gray pieces
        InstantiatePiece(graykingPrefab, grayPieces, "Gray King");
        InstantiatePiece(grayqueenPrefab, grayPieces, "Gray Queen");
        InstantiatePiece(grayrookPrefab, grayPieces, "Gray Rook 1");
        InstantiatePiece(grayrookPrefab2, grayPieces, "Gray Rook 2");
        InstantiatePiece(graybishopPrefab, grayPieces, "Gray Bishop 1");
        InstantiatePiece(graybishopPrefab2, grayPieces, "Gray Bishop 2");
        InstantiatePiece(grayknightPrefab, grayPieces, "Gray Knight 1");
        InstantiatePiece(grayknightPrefab2, grayPieces, "Gray Knight 2");
        
        // Black pieces
        InstantiatePiece(blackkingPrefab, blackPieces, "Black King");
        InstantiatePiece(blackqueenPrefab, blackPieces, "Black Queen");
        InstantiatePiece(blackrookPrefab, blackPieces, "Black Rook 1");
        InstantiatePiece(blackrookPrefab2, blackPieces, "Black Rook 2");
        InstantiatePiece(blackbishopPrefab, blackPieces, "Black Bishop 1");
        InstantiatePiece(blackbishopPrefab2, blackPieces, "Black Bishop 2");
        InstantiatePiece(blackknightPrefab, blackPieces, "Black Knight 1");
        InstantiatePiece(blackknightPrefab2, blackPieces, "Black Knight 2");
        
        // Place pieces on board positions
        // SetupPlayers();
    }
    
    // Helper method to instantiate a piece and add it to the appropriate list
    void InstantiatePiece(GameObject prefab, List<ChessPiece> piecesList, string pieceName)
    {
        if (prefab == null)
        {
            Debug.LogError($"Prefab for {pieceName} is not assigned!");
            return;
        }
        
        GameObject pieceObject = Instantiate(prefab);
        ChessPiece piece = pieceObject.GetComponent<ChessPiece>();
        
        if (piece != null)
        {
            piecesList.Add(piece);
            List<string> possibleMoves = piece.GetPossibleMoves(board);
            foreach (string move in possibleMoves)
            {
                Debug.Log($"Possible move for {pieceName}: {move}");
            }
        }
        else
        {
            Debug.LogError($"ChessPiece script is not attached to the prefab: {prefab.name}");
        }
    }

    void Update()
    {
        // Per-frame code
    }
    
    // Keep your existing SetupPlayers, SetupPlayer1, SetupPlayer2, and SetupPlayer3 methods
    // These will position the pieces on the board
    // void SetupPlayers()
    // {
    //     SetupPlayer1();  // Beyaz ta�lar (Player 1)
    //     SetupPlayer2();  // Gri ta�lar (Player 2)
    //     SetupPlayer3();  // Siyah ta�lar (Player 3)
    // }

    // void SetupPlayer1()
    // {
    //     pieceView.SpawnPiece(PieceType.Rook, "L8", PieceColor.White);
    //     pieceView.SpawnPiece(PieceType.Knight, "K8", PieceColor.White);
    //     pieceView.SpawnPiece(PieceType.Bishop, "J8", PieceColor.White);
    //     pieceView.SpawnPiece(PieceType.Queen, "I8", PieceColor.White);
    //     pieceView.SpawnPiece(PieceType.King, "D8", PieceColor.White);
    //     pieceView.SpawnPiece(PieceType.Bishop, "C8", PieceColor.White);
    //     pieceView.SpawnPiece(PieceType.Knight, "B8", PieceColor.White);
    //     pieceView.SpawnPiece(PieceType.Rook, "A8", PieceColor.White);

    //     pieceView.SpawnPiece(PieceType.Pawn, "L7", PieceColor.White);
    //     pieceView.SpawnPiece(PieceType.Pawn, "K7", PieceColor.White);
    //     pieceView.SpawnPiece(PieceType.Pawn, "J7", PieceColor.White);
    //     pieceView.SpawnPiece(PieceType.Pawn, "I7", PieceColor.White);
    //     pieceView.SpawnPiece(PieceType.Pawn, "D7", PieceColor.White);
    //     pieceView.SpawnPiece(PieceType.Pawn, "C7", PieceColor.White);
    //     pieceView.SpawnPiece(PieceType.Pawn, "B7", PieceColor.White);
    //     pieceView.SpawnPiece(PieceType.Pawn, "A7", PieceColor.White);
    // }

    // void SetupPlayer2()
    // {
    //     pieceView.SpawnPiece(PieceType.Rook, "A1", PieceColor.Gray);
    //     pieceView.SpawnPiece(PieceType.Knight, "B1", PieceColor.Gray);
    //     pieceView.SpawnPiece(PieceType.Bishop, "C1", PieceColor.Gray);
    //     pieceView.SpawnPiece(PieceType.Queen, "D1", PieceColor.Gray);
    //     pieceView.SpawnPiece(PieceType.King, "E1", PieceColor.Gray);
    //     pieceView.SpawnPiece(PieceType.Bishop, "F1", PieceColor.Gray);
    //     pieceView.SpawnPiece(PieceType.Knight, "G1", PieceColor.Gray);
    //     pieceView.SpawnPiece(PieceType.Rook, "H1", PieceColor.Gray);

    //     pieceView.SpawnPiece(PieceType.Pawn, "A2", PieceColor.Gray);
    //     pieceView.SpawnPiece(PieceType.Pawn, "B2", PieceColor.Gray);
    //     pieceView.SpawnPiece(PieceType.Pawn, "C2", PieceColor.Gray);
    //     pieceView.SpawnPiece(PieceType.Pawn, "D2", PieceColor.Gray);
    //     pieceView.SpawnPiece(PieceType.Pawn, "E2", PieceColor.Gray);
    //     pieceView.SpawnPiece(PieceType.Pawn, "F2", PieceColor.Gray);
    //     pieceView.SpawnPiece(PieceType.Pawn, "G2", PieceColor.Gray);
    //     pieceView.SpawnPiece(PieceType.Pawn, "H2", PieceColor.Gray);
    // }

    // void SetupPlayer3()
    // {
    //     pieceView.SpawnPiece(PieceType.Rook, "L12", PieceColor.Black);
    //     pieceView.SpawnPiece(PieceType.Knight, "K12", PieceColor.Black);
    //     pieceView.SpawnPiece(PieceType.Bishop, "J12", PieceColor.Black);
    //     pieceView.SpawnPiece(PieceType.Queen, "I12", PieceColor.Black);
    //     pieceView.SpawnPiece(PieceType.King, "E12", PieceColor.Black);
    //     pieceView.SpawnPiece(PieceType.Bishop, "F12", PieceColor.Black);
    //     pieceView.SpawnPiece(PieceType.Knight, "G12", PieceColor.Black);
    //     pieceView.SpawnPiece(PieceType.Rook, "H12", PieceColor.Black);

    //     pieceView.SpawnPiece(PieceType.Pawn, "L11", PieceColor.Black);
    //     pieceView.SpawnPiece(PieceType.Pawn, "K11", PieceColor.Black);
    //     pieceView.SpawnPiece(PieceType.Pawn, "J11", PieceColor.Black);
    //     pieceView.SpawnPiece(PieceType.Pawn, "I11", PieceColor.Black);
    //     pieceView.SpawnPiece(PieceType.Pawn, "E11", PieceColor.Black);
    //     pieceView.SpawnPiece(PieceType.Pawn, "F11", PieceColor.Black);
    //     pieceView.SpawnPiece(PieceType.Pawn, "G11", PieceColor.Black);
    //     pieceView.SpawnPiece(PieceType.Pawn, "H11", PieceColor.Black);
    // }
}



