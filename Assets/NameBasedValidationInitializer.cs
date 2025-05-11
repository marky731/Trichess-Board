using UnityEngine;
using Assets.Controller;
using Assets.Model.ChessboardMain;
using Assets.Model.ChessboardMain.Pieces;

public class NameBasedValidationInitializer : MonoBehaviour
{
    // Reference to the board
    private Board board;
    
    // Reference to the name-based components
    private NameBasedMoveValidator moveValidator;
    private NameBasedPieceMover pieceMover;
    private NameBasedBoardClickHandler boardClickHandler;
    
    void Awake()
    {
        Debug.Log("NameBasedValidationInitializer Awake called.");
        
        // Create the components in the correct order
        CreateMoveValidator();
        CreatePieceMover();
        AttachBoardClickHandler();
    }
    
    void Start()
    {
        Debug.Log("NameBasedValidationInitializer Start called.");
        
        // Find the board
        board = FindFirstObjectByType<Board>();
        if (board == null)
        {
            Debug.LogError("Board not found!");
            return;
        }
        
        // Initialize the components
        InitializeComponents();
    }
    
    // Create the NameBasedMoveValidator
    private void CreateMoveValidator()
    {
        // Check if the validator already exists
        moveValidator = FindFirstObjectByType<NameBasedMoveValidator>();
        if (moveValidator == null)
        {
            // Create a new GameObject for the validator
            GameObject validatorObj = new GameObject("NameBasedMoveValidator");
            moveValidator = validatorObj.AddComponent<NameBasedMoveValidator>();
            Debug.Log("Created NameBasedMoveValidator");
        }
        else
        {
            Debug.Log("NameBasedMoveValidator already exists");
        }
    }
    
    // Create the NameBasedPieceMover
    private void CreatePieceMover()
    {
        // Check if the piece mover already exists
        pieceMover = FindFirstObjectByType<NameBasedPieceMover>();
        if (pieceMover == null)
        {
            // Create a new GameObject for the piece mover
            GameObject pieceMoverObj = new GameObject("NameBasedPieceMover");
            pieceMover = pieceMoverObj.AddComponent<NameBasedPieceMover>();
            Debug.Log("Created NameBasedPieceMover");
        }
        else
        {
            Debug.Log("NameBasedPieceMover already exists");
        }
    }
    
    // Attach the NameBasedBoardClickHandler to the board
    private void AttachBoardClickHandler()
    {
        // Find the board
        Board boardComponent = FindFirstObjectByType<Board>();
        if (boardComponent == null)
        {
            Debug.LogError("Board not found!");
            return;
        }
        
        GameObject boardObject = boardComponent.gameObject;
        
        // Check if the board already has a NameBasedBoardClickHandler
        boardClickHandler = boardObject.GetComponent<NameBasedBoardClickHandler>();
        if (boardClickHandler == null)
        {
            // Add the NameBasedBoardClickHandler to the board
            boardClickHandler = boardObject.AddComponent<NameBasedBoardClickHandler>();
            Debug.Log("Attached NameBasedBoardClickHandler to the board");
            
            // Make sure the board has a collider for click detection
            if (boardObject.GetComponent<Collider2D>() == null)
            {
                BoxCollider2D collider = boardObject.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(10, 10); // Adjust size as needed
                Debug.Log("Added BoxCollider2D to the board for click detection");
            }
        }
        else
        {
            Debug.Log("NameBasedBoardClickHandler already attached to the board");
        }
        
    }
    
    // Initialize the components
    private void InitializeComponents()
    {
        // Find the Chessboard
        Chessboard chessboard = null;
        
        // Try to get the Chessboard from the Board
        if (board != null)
        {
            // This is a simplified approach - in a real implementation, you would need to
            // access the Chessboard from the Board in a more direct way
            // For now, we'll create a new Chessboard
            chessboard = new Chessboard();
            Debug.Log("Created new Chessboard");
        }
        
        // Set the Chessboard in the MoveValidator
        if (moveValidator != null && chessboard != null)
        {
            moveValidator.SetChessboard(chessboard);
            Debug.Log("Set Chessboard in NameBasedMoveValidator");
        }
        
        // Initialize the FieldDirectionMap
        if (board != null)
        {
            FieldDirectionMap.Instance.Initialize(board);
            Debug.Log("Initialized FieldDirectionMap");
        }
        
        Debug.Log("NameBasedValidationInitializer initialization complete");
    }
}
