using Assets.Model;
using System.Collections.Generic;
using UnityEngine;
using Assets.Model.ChessboardMain.Pieces;

public class GameManager : MonoBehaviour
{
    // Add these fields at the top with your other fields
    [SerializeField] private Material highlightMaterial;  // Assign this in inspector
    private Piece selectedPiece;
    private Material originalMaterial;
    
    // Change the Instance property to return GameManager type
    public static GameManager Instance { get; private set; }
    
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
    public List<Piece> whitePieces = new List<Piece>();
    private List<Piece> grayPieces = new List<Piece>();
    private List<Piece> blackPieces = new List<Piece>();

    // Reference to the TurnManager
    private Assets.Controller.TurnManager turnManager;
    
    // CurrentPlayerId property to track the current player's turn
    public int CurrentPlayerId 
    { 
        get 
        {
            // First try to use the singleton instance
            if (Assets.Controller.TurnManager.Instance != null)
            {
                return Assets.Controller.TurnManager.Instance.currentPlayer;
            }
            // If singleton is not available, use the cached reference
            else if (turnManager != null)
            {
                return turnManager.currentPlayer;
            }
            
            // Fallback to default value if TurnManager is not available
            Debug.LogWarning("No TurnManager available in CurrentPlayerId getter. Defaulting to Player 1.");
            return 1; // Default to player 1
        }
    }

    void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Find the TurnManager using the singleton instance
        turnManager = Assets.Controller.TurnManager.Instance;
        if (turnManager == null)
        {
            Debug.LogWarning("TurnManager.Instance is null, trying to find it with FindFirstObjectByType");
            turnManager = FindFirstObjectByType<Assets.Controller.TurnManager>();
            
            if (turnManager == null)
            {
                Debug.LogWarning("TurnManager not found! Creating one...");
                GameObject turnManagerObj = new GameObject("TurnManager");
                turnManager = turnManagerObj.AddComponent<Assets.Controller.TurnManager>();
            }
        }
        
        Debug.Log($"Current player: {CurrentPlayerId}");
        
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
            
            // Set player IDs for all pieces
            SetPiecePlayerIds();
        }
        else
        {
            Debug.LogError("Board prefab does not contain a Board component!");
        }
    }
    
    // Set player IDs for all pieces
    void SetPiecePlayerIds()
    {
        // White pieces belong to player 1
        foreach (Piece piece in whitePieces)
        {
            piece.PlayerId = 1;
        }
        
        // Gray pieces belong to player 2
        foreach (Piece piece in grayPieces)
        {
            piece.PlayerId = 2;
        }
        
        // Black pieces belong to player 3
        foreach (Piece piece in blackPieces)
        {
            piece.PlayerId = 3;
        }
        
        Debug.Log("Set player IDs for all pieces");
    }

    void InitializePieces()
    {
        // Initialize all white pawns
        foreach (GameObject prefab in whitePawnPrefabs)
        {
            GameObject pawnObject = Instantiate(prefab);
            Piece pawn = pawnObject.GetComponent<Piece>();
            
            if (pawn != null)
            {
                // Make sure GameObject reference is set
                pawn.GameObject = pawnObject;
                
                // Initialize the piece's position based on its GameObject position
                string initialPosition = FindClosestBoardPosition(board, pawnObject.transform.position, 5.0f);
                if (initialPosition != null)
                {
                    pawn.position = initialPosition;
                    Debug.Log($"Initialized White Pawn position to: {initialPosition}");
                }
                else
                {
                    Debug.LogWarning($"Could not determine initial position for White Pawn at {pawnObject.transform.position}");
                }
                
                // Add PieceController component if it doesn't exist
                if (pawnObject.GetComponent<PieceController>() == null)
                {
                    pawnObject.AddComponent<PieceController>();
                    Debug.Log("Added PieceController to White Pawn");
                }
                
                whitePieces.Add(pawn);
                
                // Don't call GetPossibleMoves here - defer until after all pieces are initialized
            }
            else
            {
                Debug.LogError($"Piece script is not attached to the prefab: {prefab.name}");
            }
        }

        // Initialize all gray pawns
        foreach (GameObject prefab in grayPawnPrefabs)
        {
            GameObject pawnObject = Instantiate(prefab);
            Piece pawn = pawnObject.GetComponent<Piece>();
            
            if (pawn != null)
            {
                // Make sure GameObject reference is set
                pawn.GameObject = pawnObject;
                
                // Initialize the piece's position based on its GameObject position
                string initialPosition = FindClosestBoardPosition(board, pawnObject.transform.position, 5.0f);
                if (initialPosition != null)
                {
                    pawn.position = initialPosition;
                    Debug.Log($"Initialized Gray Pawn position to: {initialPosition}");
                }
                else
                {
                    Debug.LogWarning($"Could not determine initial position for Gray Pawn at {pawnObject.transform.position}");
                }
                
                // Add PieceController component if it doesn't exist
                if (pawnObject.GetComponent<PieceController>() == null)
                {
                    pawnObject.AddComponent<PieceController>();
                    Debug.Log("Added PieceController to Gray Pawn");
                }
                
                grayPieces.Add(pawn);
                
                // Don't call GetPossibleMoves here - defer until after all pieces are initialized
            }
            else
            {
                Debug.LogError($"Piece script is not attached to the prefab: {prefab.name}");
            }
        }
        
        // Initialize all black pawns
        foreach (GameObject prefab in blackPawnPrefabs)
        {
            GameObject pawnObject = Instantiate(prefab);
            Piece pawn = pawnObject.GetComponent<Piece>();
            
            if (pawn != null)
            {
                // Make sure GameObject reference is set
                pawn.GameObject = pawnObject;
                
                // Initialize the piece's position based on its GameObject position
                string initialPosition = FindClosestBoardPosition(board, pawnObject.transform.position, 5.0f);
                if (initialPosition != null)
                {
                    pawn.position = initialPosition;
                    Debug.Log($"Initialized Black Pawn position to: {initialPosition}");
                }
                else
                {
                    Debug.LogWarning($"Could not determine initial position for Black Pawn at {pawnObject.transform.position}");
                }
                
                // Add PieceController component if it doesn't exist
                if (pawnObject.GetComponent<PieceController>() == null)
                {
                    pawnObject.AddComponent<PieceController>();
                    Debug.Log("Added PieceController to Black Pawn");
                }
                
                blackPieces.Add(pawn);
                
                // Don't call GetPossibleMoves here - defer until after all pieces are initialized
            }
            else
            {
                Debug.LogError($"Piece script is not attached to the prefab: {prefab.name}");
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
        
        // Schedule calculation of possible moves after a delay to ensure validator is initialized
        Invoke("CalculatePossibleMovesForAllPieces", 1.0f);
    }
    
    // Calculate possible moves for all pieces after initialization
    void CalculatePossibleMovesForAllPieces()
    {
        Debug.Log("Calculating possible moves for all pieces...");
        
        // Make sure the board and validator are initialized
        if (board == null)
        {
            Debug.LogError("Board is null in CalculatePossibleMovesForAllPieces!");
            return;
        }
        
        // Calculate possible moves for white pieces
        foreach (Piece piece in whitePieces)
        {
            if (piece != null && !string.IsNullOrEmpty(piece.position))
            {
                try
                {
                    List<string> possibleMoves = piece.GetPossibleMoves(board);
                    Debug.Log($"Possible moves for {piece.GetType().Name} at {piece.position}: {possibleMoves.Count}");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error calculating possible moves for {piece.GetType().Name}: {e.Message}");
                }
            }
        }
        
        // Calculate possible moves for gray pieces
        foreach (Piece piece in grayPieces)
        {
            if (piece != null && !string.IsNullOrEmpty(piece.position))
            {
                try
                {
                    List<string> possibleMoves = piece.GetPossibleMoves(board);
                    Debug.Log($"Possible moves for {piece.GetType().Name} at {piece.position}: {possibleMoves.Count}");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error calculating possible moves for {piece.GetType().Name}: {e.Message}");
                }
            }
        }
        
        // Calculate possible moves for black pieces
        foreach (Piece piece in blackPieces)
        {
            if (piece != null && !string.IsNullOrEmpty(piece.position))
            {
                try
                {
                    List<string> possibleMoves = piece.GetPossibleMoves(board);
                    Debug.Log($"Possible moves for {piece.GetType().Name} at {piece.position}: {possibleMoves.Count}");
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error calculating possible moves for {piece.GetType().Name}: {e.Message}");
                }
            }
        }
        
        Debug.Log("Finished calculating possible moves for all pieces");
    }
    
    // Helper method to instantiate a piece and add it to the appropriate list
    void InstantiatePiece(GameObject prefab, List<Piece> piecesList, string pieceName)
    {
        if (prefab == null)
        {
            Debug.LogError($"Prefab for {pieceName} is not assigned!");
            return;
        }
        
        GameObject pieceObject = Instantiate(prefab);
        Piece piece = pieceObject.GetComponent<Piece>();
        
        if (piece != null)
        {
            // Make sure GameObject reference is set
            piece.GameObject = pieceObject;
            
            // Initialize the piece's position based on its GameObject position
            string initialPosition = FindClosestBoardPosition(board, pieceObject.transform.position, 5.0f);
            if (initialPosition != null)
            {
                piece.position = initialPosition;
                Debug.Log($"Initialized {pieceName} position to: {initialPosition}");
            }
            else
            {
                Debug.LogWarning($"Could not determine initial position for {pieceName}");
            }
            
            // Add PieceController component if it doesn't exist
            if (pieceObject.GetComponent<PieceController>() == null)
            {
                pieceObject.AddComponent<PieceController>();
                Debug.Log($"Added PieceController to {pieceName}");
            }
            
            piecesList.Add(piece);
            
            // Don't call GetPossibleMoves here to avoid validation errors during initialization
            // We'll defer this until after all pieces are initialized and the validator is ready
        }
        else
        {
            Debug.LogError($"Piece script is not attached to the prefab: {prefab.name}");
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

    // Called when a piece is clicked
    public void SelectPiece(Piece piece)
    {
        if (piece == null)
        {
            Debug.LogError("Attempted to select a null piece");
            return;
        }

        // If we already have a piece selected, unhighlight it
        if (selectedPiece != null)
        {
            UnselectCurrentPiece();
        }

        // Select the new piece
        selectedPiece = piece;
        
        // Check if the GameObject reference exists
        if (piece.GameObject == null)
        {
            // Try to use the gameObject this component is attached to
            piece.GameObject = piece.gameObject;
            
            if (piece.GameObject == null)
            {
                Debug.LogError("Piece's GameObject is null and cannot be determined!");
                selectedPiece = null;
                return;
            }
        }
        
        // First try Renderer for 3D objects
        Renderer renderer = piece.GameObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            originalMaterial = renderer.material;
            if (highlightMaterial != null)
            {
                renderer.material = highlightMaterial;
            }
            else
            {
                // Create a highlighted version of the original material
                Material tempMaterial = new Material(originalMaterial);
                tempMaterial.color = new Color(
                    Mathf.Min(originalMaterial.color.r + 0.3f, 1f),
                    Mathf.Min(originalMaterial.color.g + 0.3f, 1f),
                    Mathf.Min(originalMaterial.color.b + 0.3f, 1f)
                );
                renderer.material = tempMaterial;
            }
        }
        else
        {
            // Try SpriteRenderer for 2D objects
            SpriteRenderer spriteRenderer = piece.GameObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // For SpriteRenderer, just change the color
                Color originalColor = spriteRenderer.color;
                spriteRenderer.color = new Color(
                    Mathf.Min(originalColor.r + 0.3f, 1f),
                    Mathf.Min(originalColor.g + 0.3f, 1f),
                    Mathf.Min(originalColor.b + 0.3f, 1f),
                    originalColor.a
                );
            }
            else
            {
                Debug.LogWarning("No Renderer or SpriteRenderer found on piece's GameObject");
            }
        }
    }

    public void UnselectCurrentPiece()
    {
        if (selectedPiece != null)
        {
            Renderer renderer = selectedPiece.GameObject.GetComponent<Renderer>();
            if (renderer != null && originalMaterial != null)
            {
                renderer.material = originalMaterial;
            }
            else
            {
                // Try SpriteRenderer for 2D objects
                SpriteRenderer spriteRenderer = selectedPiece.GameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    // Reset to original color (you'll need to store original color)
                    // For now, use white as a fallback
                    spriteRenderer.color = Color.white;
                }
            }
            selectedPiece = null;
        }
    }

    // Check if a piece is the currently selected one
    public bool IsPieceSelected(Piece piece)
    {
        return selectedPiece == piece;
    }

    // Get the currently selected piece
    public Piece GetSelectedPiece()
    {
        return selectedPiece;
    }
    
    // Helper method to find the closest board position to a world position
    private string FindClosestBoardPosition(Board board, Vector3 worldPosition, float threshold = 2.0f)
    {
        if (board == null)
        {
            Debug.LogError("Board is null in FindClosestBoardPosition!");
            return null;
        }
        
        // Get all board positions
        var positions = board.GetAllPositions();
        
        if (positions == null || positions.Count == 0)
        {
            Debug.LogError("No board positions available");
            return null;
        }
        
        string closestPosition = null;
        float closestDistance = float.MaxValue;
        
        // Find the closest position
        foreach (var position in positions)
        {
            Vector2 positionVector = board.GetPosition(position);
            float distance = Vector2.Distance(new Vector2(worldPosition.x, worldPosition.y), positionVector);
            
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPosition = position;
            }
        }
        
        // Only return if the closest position is within the specified threshold
        if (closestDistance <= threshold)
        {
            Debug.Log($"Found position {closestPosition} at distance {closestDistance}");
            return closestPosition;
        }
        
        Debug.Log($"No position found within threshold {threshold}. Closest was {closestPosition} at distance {closestDistance}");
        
        // If no position is within the threshold, return the closest one anyway
        // This ensures we always get a position, even if it's not ideal
        return closestPosition;
    }
}
