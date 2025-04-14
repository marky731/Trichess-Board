using Assets.Model.ChessboardMain.Pieces;
using Assets.Model.ChessboardMain.Pieces.Pawn;
using System.Collections.Generic;
using UnityEngine;

public class TriChess : MonoBehaviour
{
    public GameObject boardPrefab;      // Board prefab'� (Unity'de atanmal�)
    public GameObject blackPawnPrefab;  // BlackPawn prefab'� (Unity'de atanmal�)

    private Board board;  // Board objesi

    void Start()
    {
        // Board prefab'�n� instantiate et
        GameObject boardObject = Instantiate(boardPrefab);
        board = boardObject.GetComponent<Board>();

        if (board != null)
        {
            Debug.Log("board is not null");
            // BlackPawn olu�tur
            GameObject blackPawnObject = Instantiate(blackPawnPrefab);
            BlackPawn blackPawn = blackPawnObject.GetComponent<BlackPawn>();

            if (blackPawn != null)
            {
                // BlackPawn'�n ge�erli hamlelerini al
                List<string> possibleMoves = blackPawn.GetPossibleMoves(board);
                foreach (string move in possibleMoves)
                {
                    Debug.Log($"Possible move for Black Pawn: {move}");
                }
            }
            else
            {
                Debug.LogError("BlackPawn script is not attached to the prefab!");
            }
        }
        else
        {
            Debug.LogError("Board prefab does not contain a Board component!");
        }
    }

    void Update()
    {
        // Her frame �al��acak kod
    }
}
