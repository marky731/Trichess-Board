using Assets.Model.ChessboardMain.Pieces.Pawn;
using Assets.Model.ChessboardMain;
using Assets.Model.ChessboardMain.Pieces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriChess : MonoBehaviour
{
    // We don't need this prefab anymore since we're using the GameManager to create pieces
    // public GameObject blackPawnPrefab;
    
    private Board board;  // Board objesi

    // Start is called before the first frame update
    void Start()
    {
        // Board objesini sahnede bul
        board = Object.FindFirstObjectByType<Board>();  // Yeni önerilen yöntem

        if (board == null)
        {
            Debug.LogError("Board object not found in the scene!");
            return;
        }
        
        Debug.Log("TriChess: Board found successfully!");
        
        // Instead of trying to instantiate a BlackPawn directly, we'll use the GameManager
        // The GameManager will handle creating all the pieces with the proper setup
    }

    // Update is called once per frame
    void Update()
    {
        // Bu metot her frame'de çalışır, şu an herhangi bir işlevsellik yok
    }
}
