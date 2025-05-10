using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Model.ChessboardMain.Pieces
{
    public abstract class Piece :MonoBehaviour
    {
        private PieceColor color;
        private bool isSliding;
        public bool IsMoved { get; private set; } = false;

        public int PlayerId { get; set; }  
        public string position;  
        public GameObject GameObject { get; set; }  

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

        public string CurrentPosition => Position; 

        public abstract string GetAbbreviation();
        public PieceColor GetColor() => color;
        public bool IsSliding() => isSliding;
        public abstract HashSet<Direction> GetDirections();


        public void SetMoved()
        {
            IsMoved = true;
        }


        public void SetPosition(string newPosition, Board board)
        {
            position = newPosition;
        }

        public string GetPosition()
        {
            return position;
        }

        public abstract List<string> GetPossibleMoves(Board board);
    }
}
