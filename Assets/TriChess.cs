using Assets.Model.ChessboardMain.Pieces.Pawn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriChess : MonoBehaviour
{
    public GameObject blackPawnPrefab; // BlackPawn prefab'� sahnede yer alacak
    private Board board;  // Board objesi

    // Start is called before the first frame update
    void Start()
    {
        // Board objesini sahnede bul
        board = Object.FindFirstObjectByType<Board>();  // Yeni �nerilen y�ntem

        if (board != null)
        {
            // BlackPawn'� olu�tur ve GameObject referans� ile ba�lat
            GameObject blackPawnObject = Instantiate(blackPawnPrefab);  // BlackPawn prefab'�n� instantiate et
            BlackPawn blackPawn = blackPawnObject.GetComponent<BlackPawn>();  // BlackPawn scriptine eri�im sa�la

            if (blackPawn != null)
            {
                // Ge�ebilece�i hamleleri al ve konsola yazd�r
                List<string> possibleMoves = blackPawn.GetPossibleMoves(board);

                // Olas� hamleleri konsola yazd�r
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
            Debug.LogError("Board object not found in the scene!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Bu metot her frame'de �al���r, �u an herhangi bir i�levsellik yok
    }
}
