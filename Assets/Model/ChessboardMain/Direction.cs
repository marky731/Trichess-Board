using System;
using System.Collections.Generic;

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

            foreach (var dir in GetAllDirections())
            {
                if (dir.X == dx && dir.Y == dy)
                    return dir;
            }

            throw new ArgumentException("Invalid move direction.");
        }
    }
}
