using UnityEngine;
using UnityEditor;
using Assets.Model;
using Assets.Model.ChessboardMain.Pieces;
using Assets.Controller;
using System.Collections.Generic;

[CustomEditor(typeof(GameManager))]
public class PieceMovementTester : Editor
{
    private string selectedPieceType = "Pawn";
    private string selectedColor = "White";
    private string sourcePosition = "";
    private string targetPosition = "";
    private bool showAllPositions = false;
    private Vector2 scrollPosition;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameManager gameManager = (GameManager)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Piece Movement Tester", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Check if required components exist
        EditorGUILayout.LabelField("Component Status:", EditorStyles.boldLabel);
        
        bool gameManagerExists = gameManager != null;
        bool boardExists = FindObjectOfType<Board>() != null;
        bool pieceMoverExists = FindObjectOfType<PieceMover>() != null;
        bool moveValidatorExists = FindObjectOfType<MoveValidator>() != null;
        bool turnManagerExists = FindObjectOfType<TurnManager>() != null;
        
        GUI.color = gameManagerExists ? Color.green : Color.red;
        EditorGUILayout.LabelField($"GameManager: {(gameManagerExists ? "Found" : "Missing")}");
        
        GUI.color = boardExists ? Color.green : Color.red;
        EditorGUILayout.LabelField($"Board: {(boardExists ? "Found" : "Missing")}");
        
        GUI.color = pieceMoverExists ? Color.green : Color.red;
        EditorGUILayout.LabelField($"PieceMover: {(pieceMoverExists ? "Found" : "Missing")}");
        
        GUI.color = moveValidatorExists ? Color.green : Color.red;
        EditorGUILayout.LabelField($"MoveValidator: {(moveValidatorExists ? "Found" : "Missing")}");
        
        GUI.color = turnManagerExists ? Color.green : Color.red;
        EditorGUILayout.LabelField($"TurnManager: {(turnManagerExists ? "Found" : "Missing")}");
        
        GUI.color = Color.white;
        EditorGUILayout.Space();

        // Create missing components button
        if (!gameManagerExists || !boardExists || !pieceMoverExists || !moveValidatorExists || !turnManagerExists)
        {
            if (GUILayout.Button("Create Missing Components"))
            {
                CreateMissingComponents();
            }
            EditorGUILayout.Space();
        }

        // Currently selected piece
        Piece selectedPiece = gameManager.GetSelectedPiece();
        EditorGUILayout.LabelField("Currently Selected Piece:", EditorStyles.boldLabel);
        if (selectedPiece != null)
        {
            EditorGUILayout.LabelField($"Type: {selectedPiece.GetType().Name}");
            EditorGUILayout.LabelField($"Position: {selectedPiece.position}");
            EditorGUILayout.LabelField($"Player ID: {selectedPiece.PlayerId}");
        }
        else
        {
            EditorGUILayout.LabelField("No piece selected");
        }
        EditorGUILayout.Space();

        // Manual piece movement
        EditorGUILayout.LabelField("Manual Piece Movement:", EditorStyles.boldLabel);
        sourcePosition = EditorGUILayout.TextField("Source Position:", sourcePosition);
        targetPosition = EditorGUILayout.TextField("Target Position:", targetPosition);

        if (GUILayout.Button("Move Piece"))
        {
            MovePieceManually(sourcePosition, targetPosition);
        }
        EditorGUILayout.Space();

        // Show all board positions
        showAllPositions = EditorGUILayout.Foldout(showAllPositions, "Show All Board Positions");
        if (showAllPositions)
        {
            Board board = FindObjectOfType<Board>();
            if (board != null)
            {
                List<string> positions = board.GetAllPositions();
                
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
                foreach (string position in positions)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(position);
                    Vector2 worldPos = board.GetPosition(position);
                    EditorGUILayout.LabelField($"({worldPos.x}, {worldPos.y})");
                    if (GUILayout.Button("Set Source"))
                    {
                        sourcePosition = position;
                    }
                    if (GUILayout.Button("Set Target"))
                    {
                        targetPosition = position;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndScrollView();
            }
            else
            {
                EditorGUILayout.LabelField("Board not found in scene");
            }
        }
    }

    private void CreateMissingComponents()
    {
        // Create GameManager if it doesn't exist
        if (FindObjectOfType<GameManager>() == null)
        {
            GameObject gameManagerObj = new GameObject("GameManager");
            gameManagerObj.AddComponent<GameManager>();
            Debug.Log("Created GameManager");
        }

        // Create GameInitializer if it doesn't exist
        if (FindObjectOfType<GameInitializer>() == null)
        {
            GameObject gameInitializerObj = new GameObject("GameInitializer");
            gameInitializerObj.AddComponent<GameInitializer>();
            Debug.Log("Created GameInitializer");
        }

        // The GameInitializer will create the other required components
    }

    private void MovePieceManually(string sourcePos, string targetPos)
    {
        if (string.IsNullOrEmpty(sourcePos) || string.IsNullOrEmpty(targetPos))
        {
            Debug.LogError("Source and target positions must be specified");
            return;
        }

        Board board = FindObjectOfType<Board>();
        if (board == null)
        {
            Debug.LogError("Board not found in scene");
            return;
        }

        Field sourceField = board.GetField(sourcePos);
        if (sourceField == null)
        {
            Debug.LogError($"Source field not found: {sourcePos}");
            return;
        }

        Piece piece = sourceField.OccupiedPiece;
        if (piece == null)
        {
            Debug.LogError($"No piece found at position: {sourcePos}");
            return;
        }

        PieceMover pieceMover = FindObjectOfType<PieceMover>();
        if (pieceMover == null)
        {
            Debug.LogError("PieceMover not found in scene");
            return;
        }

        Debug.Log($"Moving {piece.GetType().Name} from {sourcePos} to {targetPos}");
        pieceMover.MovePiece(piece, targetPos);
    }
}
