//PieceMover sınıfı, satranç taşlarının hareketini yöneten ve kontrol eden bir sınıftır.
//Bu sınıf, taşların geçerli bir hamle yapıp yapmadığını kontrol eder, taşı hareket ettirir ve oyuncuların sırasını değiştirir.
//MovePiece metoduyla:
//Kaynak ve hedef kareleri alır.
//Hedef karede bir taş varsa, o taşı alır (yemek için).
//Yeni bir Move nesnesi oluşturur ve hamlenin geçerliliğini MoveValidator ile kontrol eder.
//Eğer hamle geçerliyse, taşı yeni konumuna taşır ve TurnManager ile sıradaki oyuncuya geçer.

using Assets.Model;
using Assets.Model.ChessboardMain.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Controller
{
    public class PieceMover : MonoBehaviour
    {
        // Taş hareketlerini doğrulayan sınıf
        private MoveValidator moveValidator;

        // Sıra yöneticisini kontrol eden sınıf
        private TurnManager turnManager;

        // Tahtayı yöneten sınıf
        private Board board;

        // Başlangıçta gerekli bileşenleri bulur ve başlatır
        void Start()
        {
            moveValidator = FindFirstObjectByType<MoveValidator>();
            turnManager = FindFirstObjectByType<TurnManager>();
            board = FindFirstObjectByType<Board>();
        }

        // Taşın hareketini gerçekleştiren ana metod
        public void MovePiece(Piece piece, string targetPosition)
        {
            // 1. Taşın mevcut pozisyonu (kaynak alan) alınır
            Field sourceField = board.GetField(piece.position);

            // 2. Hedef pozisyonu (hedef alan) alınır, targetPosition string olduğu için Field objesine dönüştürülür
            Field targetField = board.GetField(targetPosition);

            // Eğer kaynak veya hedef alan geçersizse hata mesajı verir
            if (sourceField == null || targetField == null)
            {
                Debug.LogError("Kaynak veya hedef alan geçersiz!");
                return;
            }

            // 3. Hedef karede taş varsa (yemek için), o taşı al
            Piece takenPiece = (Piece)targetField.OccupiedPiece;

            // 4. Yeni bir Move nesnesi oluşturulur, bu hamleyi temsil eder
            Move move = new Move(sourceField, piece, targetField, takenPiece);

            // 5. Hamlenin geçerli olup olmadığı MoveValidator ile kontrol edilir
            if (!moveValidator.IsValidMove(piece, move))
            {
                Debug.LogError("Geçersiz hamle!");
                return;
            }

            // 6. Taşı yeni konumuna taşır, GameObject'inin pozisyonunu günceller
            piece.GameObject.transform.position = board.GetPosition(targetPosition);

            // 7. Sıradaki oyuncuya geçiş yapılır
            turnManager.NextTurn();
        }
    }
}
