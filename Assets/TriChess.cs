using Assets.Model.ChessboardMain.Pieces.Pawn;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriChess : MonoBehaviour
{
    public GameObject blackPawnPrefab; // BlackPawn prefab'ý sahnede yer alacak
    private Board board;  // Board objesi

    // Start is called before the first frame update
    void Start()
    {
        // Board objesini sahnede bul
        board = Object.FindFirstObjectByType<Board>();  // Yeni önerilen yöntem

        if (board != null)
        {
            // BlackPawn'ý oluþtur ve GameObject referansý ile baþlat
            GameObject blackPawnObject = Instantiate(blackPawnPrefab);  // BlackPawn prefab'ýný instantiate et
            BlackPawn blackPawn = blackPawnObject.GetComponent<BlackPawn>();  // BlackPawn scriptine eriþim saðla

            if (blackPawn != null)
            {
                // Geçebileceði hamleleri al ve konsola yazdýr
                List<string> possibleMoves = blackPawn.GetPossibleMoves(board);

                // Olasý hamleleri konsola yazdýr
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
        // Bu metot her frame'de çalýþýr, þu an herhangi bir iþlevsellik yok
    }
}
