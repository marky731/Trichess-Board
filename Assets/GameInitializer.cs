using UnityEngine;
using Assets.Controller;

public class GameInitializer : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("GameInitializer Awake called");
        
        // Check if BoardClickHandlerAttacher already exists
        BoardClickHandlerAttacher existingAttacher = FindFirstObjectByType<BoardClickHandlerAttacher>();
        if (existingAttacher == null)
        {
            // Add the BoardClickHandlerAttacher to the scene
            GameObject attacher = new GameObject("BoardClickHandlerAttacher");
            attacher.AddComponent<BoardClickHandlerAttacher>();
            Debug.Log("BoardClickHandlerAttacher added to the scene.");
        }
        else
        {
            Debug.Log("BoardClickHandlerAttacher already exists in the scene.");
        }
        
        // Check if PieceMover already exists
        PieceMover existingPieceMover = FindFirstObjectByType<PieceMover>();
        if (existingPieceMover == null)
        {
            // Add the PieceMover to the scene
            GameObject pieceMoverObj = new GameObject("PieceMover");
            pieceMoverObj.AddComponent<PieceMover>();
            Debug.Log("PieceMover added to the scene.");
        }
        else
        {
            Debug.Log("PieceMover already exists in the scene.");
        }
        
        // Check if MoveValidator already exists
        MoveValidator existingMoveValidator = FindFirstObjectByType<MoveValidator>();
        if (existingMoveValidator == null)
        {
            // Add the MoveValidator to the scene
            GameObject moveValidatorObj = new GameObject("MoveValidator");
            moveValidatorObj.AddComponent<MoveValidator>();
            Debug.Log("MoveValidator added to the scene.");
        }
        else
        {
            Debug.Log("MoveValidator already exists in the scene.");
        }
        
        // Check if TurnManager already exists
        TurnManager existingTurnManager = FindFirstObjectByType<TurnManager>();
        if (existingTurnManager == null)
        {
            // Add the TurnManager to the scene
            GameObject turnManagerObj = new GameObject("TurnManager");
            turnManagerObj.AddComponent<TurnManager>();
            Debug.Log("TurnManager added to the scene.");
        }
        else
        {
            Debug.Log("TurnManager already exists in the scene.");
        }
    }
}
