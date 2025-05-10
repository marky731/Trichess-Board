using Assets.Model.ChessboardMain.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Model
{
    public class Field
    {
        public double X { get; }
        public double Y { get; }
        private Piece occupiedPiece;
        public string Position => $"{X},{Y}";

        public Piece OccupiedPiece
        {
            get { return occupiedPiece; }
            internal set
            {
                // If there's already a piece here and we're setting a new one
                if (occupiedPiece != null && value != null && occupiedPiece != value)
                {
                    Debug.Log($"Field at {Position} already occupied by {occupiedPiece.GetType().Name}, replacing with {value.GetType().Name}");
                    
                    // Disable the previous piece's GameObject if it has one
                    if (occupiedPiece.GameObject != null)
                    {
                        occupiedPiece.GameObject.SetActive(false);
                    }
                }
                
                // If we're clearing the piece (setting to null)
                if (value == null && occupiedPiece != null)
                {
                    Debug.Log($"Clearing piece {occupiedPiece.GetType().Name} from field {Position}");
                }
                
                // If we're setting a new piece
                if (value != null)
                {
                    Debug.Log($"Setting piece {value.GetType().Name} on field {Position}");
                    
                    // Update the piece's position property
                    value.position = Position;
                }
                
                occupiedPiece = value;
            }
        }

        public Field(double x, double y)
        {
            X = x;
            Y = y;
            occupiedPiece = null;
        }

        public bool Equals(double x, double y)
        {
            return X == x && Y == y;
        }

        public override bool Equals(object obj)
        {
            if (obj is Field other)
            {
                return X == other.X && Y == other.Y;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return (X, Y).GetHashCode();
        }
        
        public override string ToString()
        {
            return $"Field({X},{Y}){(occupiedPiece != null ? $" occupied by {occupiedPiece.GetType().Name}" : "")}";
        }
    }
}
