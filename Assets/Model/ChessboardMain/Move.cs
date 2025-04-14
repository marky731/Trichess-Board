//Bu Field sınıfı, satranç tahtasında bir hücreyi temsil eder ve her hücrenin koordinatlarını (X ve Y) saklar.
//Aynı zamanda, bu hücrede bir taş olup olmadığını kontrol etmek için OccupiedPiece özelliği kullanılır.
using Assets.Model.ChessboardMain;
using Assets.Model.ChessboardMain.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Assets.Model.ChessboardMain.Pieces
{
    public class Move
    {
        public Field Source { get; }
        public Piece MovedPiece { get; }
        public Field Destination { get; }
        public Piece TakenPiece { get; }
        public bool Moved { get; }
        public Piece PromotionPiece { get; private set; }

        // Constructor: Taşın hareketini temsil eder
        public Move(Field source, Piece movedPiece, Field destination, Piece takenPiece = null)
        {
            MovedPiece = movedPiece ?? throw new ArgumentNullException(nameof(movedPiece));
            Source = source;
            Destination = destination;
            TakenPiece = takenPiece;
            Moved = movedPiece.IsMoved;
            //Taş hareket ettiğinde işaretle
            MovedPiece.SetMoved();
        }

        // Boş hamle için constructor
        public Move()
        {
            MovedPiece = null;
            Source = null;
            Destination = null;
            TakenPiece = null;
            Moved = false;
        }

        // Piyon terfisini ayarlar
        public void SetPawnPromotion(Piece promotionPiece)
        {
            PromotionPiece = promotionPiece;
        }
    }
}
