//GameManager sýnýfý, oyun baþlamadan önce tahtada oyuncularýn taþlarýný kuran ve yönetmeye baþlayan ana sýnýftýr.
//Bu sýnýf, oyunun baþýnda üç oyuncunun taþlarýný baþlatmak için çeþitli fonksiyonlar içerir.
//Bu taþlar, PieceView sýnýfýný kullanarak görsel olarak sahnede yerleþtirilir.
//Oyunculara ait taþlar (Beyaz, Gri ve Siyah) uygun pozisyonlarda kurulup, her taþýn rengi ve türü belirlenir.

using Assets.Model;
using Assets.Model.ChessboardMain.Pieces;
using Assets.View;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Board board;  // Oyun tahtasý
    public PieceView pieceView;  // Taþlarý yönetmek için

    void Start()
    {
            Debug.Log("Game started");
        // Eðer board veya pieceView atanmadýysa, hata mesajý basýlýr
        if (board == null || pieceView == null)
        {
            Debug.LogError("GameManager: Board veya PieceView eksik!");
            return;
        }

        // Oyuncular (taþlar) kurulumu yapýlýr
        SetupPlayers();
    }

    // Tüm oyuncularý kurar
    void SetupPlayers()
    {
        SetupPlayer1();  // Beyaz taþlar (Player 1)
        SetupPlayer2();  // Gri taþlar (Player 2)
        SetupPlayer3();  // Siyah taþlar (Player 3)
    }
    void SetupPlayer1()
    {
        pieceView.SpawnPiece(PieceType.Rook, "L8", PieceColor.White);
        pieceView.SpawnPiece(PieceType.Knight, "K8", PieceColor.White);
        pieceView.SpawnPiece(PieceType.Bishop, "J8", PieceColor.White);
        pieceView.SpawnPiece(PieceType.Queen, "I8", PieceColor.White);
        pieceView.SpawnPiece(PieceType.King, "D8", PieceColor.White);
        pieceView.SpawnPiece(PieceType.Bishop, "C8", PieceColor.White);
        pieceView.SpawnPiece(PieceType.Knight, "B8", PieceColor.White);
        pieceView.SpawnPiece(PieceType.Rook, "A8", PieceColor.White);

        pieceView.SpawnPiece(PieceType.Pawn, "L7", PieceColor.White);
        pieceView.SpawnPiece(PieceType.Pawn, "K7", PieceColor.White);
        pieceView.SpawnPiece(PieceType.Pawn, "J7", PieceColor.White);
        pieceView.SpawnPiece(PieceType.Pawn, "I7", PieceColor.White);
        pieceView.SpawnPiece(PieceType.Pawn, "D7", PieceColor.White);
        pieceView.SpawnPiece(PieceType.Pawn, "C7", PieceColor.White);
        pieceView.SpawnPiece(PieceType.Pawn, "B7", PieceColor.White);
        pieceView.SpawnPiece(PieceType.Pawn, "A7", PieceColor.White);
    }

    void SetupPlayer2()
    {
        pieceView.SpawnPiece(PieceType.Rook, "A1", PieceColor.Gray);
        pieceView.SpawnPiece(PieceType.Knight, "B1", PieceColor.Gray);
        pieceView.SpawnPiece(PieceType.Bishop, "C1", PieceColor.Gray);
        pieceView.SpawnPiece(PieceType.Queen, "D1", PieceColor.Gray);
        pieceView.SpawnPiece(PieceType.King, "E1", PieceColor.Gray);
        pieceView.SpawnPiece(PieceType.Bishop, "F1", PieceColor.Gray);
        pieceView.SpawnPiece(PieceType.Knight, "G1", PieceColor.Gray);
        pieceView.SpawnPiece(PieceType.Rook, "H1", PieceColor.Gray);

        pieceView.SpawnPiece(PieceType.Pawn, "A2", PieceColor.Gray);
        pieceView.SpawnPiece(PieceType.Pawn, "B2", PieceColor.Gray);
        pieceView.SpawnPiece(PieceType.Pawn, "C2", PieceColor.Gray);
        pieceView.SpawnPiece(PieceType.Pawn, "D2", PieceColor.Gray);
        pieceView.SpawnPiece(PieceType.Pawn, "E2", PieceColor.Gray);
        pieceView.SpawnPiece(PieceType.Pawn, "F2", PieceColor.Gray);
        pieceView.SpawnPiece(PieceType.Pawn, "G2", PieceColor.Gray);
        pieceView.SpawnPiece(PieceType.Pawn, "H2", PieceColor.Gray);
    }

    void SetupPlayer3()
    {
        pieceView.SpawnPiece(PieceType.Rook, "L12", PieceColor.Black);
        pieceView.SpawnPiece(PieceType.Knight, "K12", PieceColor.Black);
        pieceView.SpawnPiece(PieceType.Bishop, "J12", PieceColor.Black);
        pieceView.SpawnPiece(PieceType.Queen, "I12", PieceColor.Black);
        pieceView.SpawnPiece(PieceType.King, "E12", PieceColor.Black);
        pieceView.SpawnPiece(PieceType.Bishop, "F12", PieceColor.Black);
        pieceView.SpawnPiece(PieceType.Knight, "G12", PieceColor.Black);
        pieceView.SpawnPiece(PieceType.Rook, "H12", PieceColor.Black);

        pieceView.SpawnPiece(PieceType.Pawn, "L11", PieceColor.Black);
        pieceView.SpawnPiece(PieceType.Pawn, "K11", PieceColor.Black);
        pieceView.SpawnPiece(PieceType.Pawn, "J11", PieceColor.Black);
        pieceView.SpawnPiece(PieceType.Pawn, "I11", PieceColor.Black);
        pieceView.SpawnPiece(PieceType.Pawn, "E11", PieceColor.Black);
        pieceView.SpawnPiece(PieceType.Pawn, "F11", PieceColor.Black);
        pieceView.SpawnPiece(PieceType.Pawn, "G11", PieceColor.Black);
        pieceView.SpawnPiece(PieceType.Pawn, "H11", PieceColor.Black);
    }
}

