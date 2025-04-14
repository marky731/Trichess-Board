using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Model.ChessboardMain.Pieces
{
    public abstract class Piece
    {
        private PieceColor color;
        private bool isSliding;
        public bool IsMoved { get; private set; } = false;

        public int PlayerId { get; set; }  // Taşı kontrol eden oyuncunun ID'si
        public string position;  // Taşın tahtadaki konumu
        public GameObject GameObject { get; set; }  // Unity GameObject referansı

        public Piece(PieceColor color, bool isSliding, GameObject gameObject, int playerId)
        {
            this.color = color;
            this.isSliding = isSliding;
            this.GameObject = gameObject;
            this.PlayerId = playerId;
        }

        public string Position
        {
            get => position;
            set => position = value;
        }

        public string CurrentPosition => Position; // Alternatif erişim için

        public abstract string GetAbbreviation();
        public PieceColor GetColor() => color;
        public bool IsSliding() => isSliding;
        public abstract HashSet<Direction> GetDirections();

        // Taşın hareket ettiğini güncelleyen metod
        public void SetMoved()
        {
            IsMoved = true;
        }

        // Taşın tahtadaki konumunu belirleme
        public void SetPosition(string newPosition, Board board)
        {
            position = newPosition;
        }

        // Taşın mevcut konumunu almak
        public string GetPosition()
        {
            return position;
        }

        // Bu metod türetilmiş sınıflar tarafından uygulanacak
        public abstract List<string> GetPossibleMoves(Board board);
    }
}
