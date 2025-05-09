using System;
using System.Collections.Generic;
using UnityEngine;  // Added for Debug.Log

namespace Assets.Model.ChessboardMain
{
    public class Direction
    {
        public double X { get; }
        public double Y { get; }

        public Direction(double x, double y)
        {
            X = x;
            Y = y;
        }

        //Beyaz için yönler
        public static readonly Direction WhiteForward = new Direction(1, -1);
        public static readonly Direction WhiteForwardRight = new Direction(1, 0);  // İleri Sağ Çapraz
        public static readonly Direction WhiteForwardLeft = new Direction(0, -1);  // İleri Sol Çapraz
        public static readonly Direction WhiteBackward = new Direction(-1, 1);
        public static readonly Direction WhiteBackwardRight = new Direction(-1, 0); // Geri Sağ Çapraz
        public static readonly Direction WhiteBackwardLeft = new Direction(0, 1);  // Geri Sol Çapraz

        //Siyah için yönler
        public static readonly Direction BlackForward = new Direction(-1, -1);
        public static readonly Direction BlackForwardRight = new Direction(0, -1); // İleri Sağ Çapraz
        public static readonly Direction BlackForwardLeft = new Direction(-1, 0); // İleri Sol Çapraz
        public static readonly Direction BlackBackward = new Direction(1, 1);
        public static readonly Direction BlackBackwardRight = new Direction(1, 0); // Geri Sağ Çapraz
        public static readonly Direction BlackBackwardLeft = new Direction(0, 1); // Geri Sol Çapraz

        // Gri için yönler
        public static readonly Direction GrayForward = new Direction(0, 1);
        public static readonly Direction GrayForwardRight = new Direction(1, 1);  // İleri Sağ Çapraz
        public static readonly Direction GrayForwardLeft = new Direction(-1, 1);  // İleri Sol Çapraz
        public static readonly Direction GrayBackward = new Direction(0, -1);
        public static readonly Direction GrayBackwardRight = new Direction(1, -1); // Geri Sağ Çapraz
        public static readonly Direction GrayBackwardLeft = new Direction(-1, -1); // Geri Sol Çapraz
        public Field Move(Field currentPosition)
        {
            return new Field(currentPosition.X + X, currentPosition.Y + Y);
        }

        // Tüm yönleri liste olarak döndüren fonksiyon
        public static List<Direction> GetAllDirections()
        {
            return new List<Direction>
            {
                WhiteForward, WhiteForwardRight, WhiteForwardLeft, WhiteBackward, WhiteBackwardRight, WhiteBackwardLeft,
                BlackForward, BlackForwardRight, BlackForwardLeft, BlackBackward, BlackBackwardRight, BlackBackwardLeft,
                GrayForward, GrayForwardRight, GrayForwardLeft, GrayBackward, GrayBackwardRight, GrayBackwardLeft
            };
        }

        // Yönü bulma fonksiyonu (altıgen için)
        public static Direction GetDirection(Field source, Field destination)
        {
            double dx = destination.X - source.X;
            double dy = destination.Y - source.Y;

            // Check if the piece is trying to move to its current position
            if (Math.Abs(dx) < 0.01 && Math.Abs(dy) < 0.01)
            {
                // Return a special "no movement" direction
                return new Direction(0, 0);
            }

            // Normalize the direction for longer moves
            double length = Math.Sqrt(dx * dx + dy * dy);
            
            // Increase this threshold from 0.3 to 0.5 for more lenient matching
            double matchThreshold = 0.5;
            
            // Try to find the closest standard direction
            double bestMatch = double.MaxValue;
            Direction bestDir = null;

            foreach (var dir in GetAllDirections())
            {
                // Calculate the angle between the actual direction and this standard direction
                double dotProduct = dx * dir.X + dy * dir.Y;
                double dirLength = Math.Sqrt(dir.X * dir.X + dir.Y * dir.Y);
                
                // Avoid division by zero
                if (length < 0.01 || dirLength < 0.01)
                    continue;
                    
                double angleDiff = Math.Abs(dotProduct / (length * dirLength) - 1.0);
                
                if (angleDiff < bestMatch)
                {
                    bestMatch = angleDiff;
                    bestDir = dir;
                }
            }

            // More lenient matching - if we found a reasonably close match
            if (bestDir != null && bestMatch < matchThreshold)
            {
                Debug.Log($"Matched direction ({dx}, {dy}) to standard direction ({bestDir.X}, {bestDir.Y}) with difference {bestMatch}");
                return bestDir;
            }

            // If we couldn't find a matching direction, create a custom one
            Debug.Log($"Created custom direction ({dx}, {dy}) as no standard direction matched within threshold {matchThreshold}");
            return new Direction(dx, dy);
        }
    }
}
